using System.Text.Json;
using WebApp.Models;

namespace WebApp.Services;

/// <summary>
/// Centralized firmware context management service.
/// Replaces ad-hoc sessionStorage handling with persistent, profile-based context management.
/// </summary>
public class ContextService
{
    private readonly FirmwareDataService _dataService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string SESSION_KEY = "FirmwareContext";
    private const string PROFILES_KEY = "ContextProfiles";

    public ContextService(FirmwareDataService dataService, IHttpContextAccessor httpContextAccessor)
    {
        _dataService = dataService;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Get the current firmware context for the user's session.
    /// Returns null if no context is set.
    /// </summary>
    public FirmwareContext? GetCurrentContext()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null) return null;

        var contextJson = session.GetString(SESSION_KEY);
        if (string.IsNullOrEmpty(contextJson)) return null;

        return JsonSerializer.Deserialize<FirmwareContext>(contextJson);
    }

    /// <summary>
    /// Set the current firmware context for the user's session.
    /// </summary>
    public void SetCurrentContext(FirmwareContext context)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null) return;

        context.LastModified = DateTime.UtcNow;
        var contextJson = JsonSerializer.Serialize(context);
        session.SetString(SESSION_KEY, contextJson);

        // Add to history
        AddToHistory(context);
    }

    /// <summary>
    /// Clear the current firmware context.
    /// </summary>
    public void ClearContext()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        session?.Remove(SESSION_KEY);
    }

    /// <summary>
    /// Get or create a default context (fallback to Eagan program).
    /// </summary>
    public FirmwareContext GetOrCreateDefaultContext()
    {
        var current = GetCurrentContext();
        if (current != null) return current;

        // Create default context with Eagan program
        var npiProgram = _dataService.GetNpiPrograms().FirstOrDefault(p => p.Name == "Eagan");
        if (npiProgram == null) npiProgram = _dataService.GetNpiPrograms().First();

        var lpiVersions = _dataService.GetLpiVersions(npiProgram.Id);
        var piccoloVersions = _dataService.GetPiccoloVersions(npiProgram.Id);

        var defaultContext = new FirmwareContext
        {
            NpiProgramId = npiProgram.Id,
            NpiProgramName = npiProgram.Name,
            LpiVersionId = lpiVersions.FirstOrDefault()?.Id ?? Guid.Empty,
            LpiVersionNumber = lpiVersions.FirstOrDefault()?.Version ?? "Unknown",
            PiccoloVersionId = piccoloVersions.FirstOrDefault()?.Id ?? Guid.Empty,
            PiccoloVersionNumber = piccoloVersions.FirstOrDefault()?.Version ?? "Unknown",
            LastModified = DateTime.UtcNow
        };

        SetCurrentContext(defaultContext);
        return defaultContext;
    }

    /// <summary>
    /// Validate that the context has valid NPI program and version combinations.
    /// </summary>
    public bool ValidateContext(FirmwareContext context)
    {
        var npiProgram = _dataService.GetNpiPrograms().FirstOrDefault(p => p.Id == context.NpiProgramId);
        if (npiProgram == null) return false;

        var lpiVersion = _dataService.GetLpiVersions(context.NpiProgramId)
            .FirstOrDefault(v => v.Id == context.LpiVersionId);
        var piccoloVersion = _dataService.GetPiccoloVersions(context.NpiProgramId)
            .FirstOrDefault(v => v.Id == context.PiccoloVersionId);

        if (lpiVersion == null || piccoloVersion == null) return false;

        // Verify versions belong to the selected NPI program
        var lpiVersions = _dataService.GetLpiVersions(context.NpiProgramId);
        var piccoloVersions = _dataService.GetPiccoloVersions(context.NpiProgramId);

        return lpiVersions.Any(v => v.Id == context.LpiVersionId) &&
               piccoloVersions.Any(v => v.Id == context.PiccoloVersionId);
    }

    /// <summary>
    /// Save a named context profile for quick switching.
    /// </summary>
    public void SaveProfile(string profileName, FirmwareContext context)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null) return;

        var profiles = GetProfiles();
        profiles[profileName] = context;

        var profilesJson = JsonSerializer.Serialize(profiles);
        session.SetString(PROFILES_KEY, profilesJson);
    }

    /// <summary>
    /// Load a saved context profile by name.
    /// </summary>
    public FirmwareContext? LoadProfile(string profileName)
    {
        var profiles = GetProfiles();
        return profiles.ContainsKey(profileName) ? profiles[profileName] : null;
    }

    /// <summary>
    /// Get all saved context profiles.
    /// </summary>
    public Dictionary<string, FirmwareContext> GetProfiles()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null) return new Dictionary<string, FirmwareContext>();

        var profilesJson = session.GetString(PROFILES_KEY);
        if (string.IsNullOrEmpty(profilesJson))
            return new Dictionary<string, FirmwareContext>();

        return JsonSerializer.Deserialize<Dictionary<string, FirmwareContext>>(profilesJson)
               ?? new Dictionary<string, FirmwareContext>();
    }

    /// <summary>
    /// Delete a saved context profile.
    /// </summary>
    public void DeleteProfile(string profileName)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null) return;

        var profiles = GetProfiles();
        profiles.Remove(profileName);

        var profilesJson = JsonSerializer.Serialize(profiles);
        session.SetString(PROFILES_KEY, profilesJson);
    }

    /// <summary>
    /// Get context history (last 5 contexts).
    /// </summary>
    public List<FirmwareContext> GetContextHistory()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null) return new List<FirmwareContext>();

        var historyJson = session.GetString("ContextHistory");
        if (string.IsNullOrEmpty(historyJson))
            return new List<FirmwareContext>();

        return JsonSerializer.Deserialize<List<FirmwareContext>>(historyJson)
               ?? new List<FirmwareContext>();
    }

    /// <summary>
    /// Add context to history (max 5 entries).
    /// </summary>
    private void AddToHistory(FirmwareContext context)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        if (session == null) return;

        var history = GetContextHistory();

        // Remove duplicates (same NPI + LPI + PICCOLO combination)
        history.RemoveAll(h =>
            h.NpiProgramId == context.NpiProgramId &&
            h.LpiVersionId == context.LpiVersionId &&
            h.PiccoloVersionId == context.PiccoloVersionId);

        // Add to front
        history.Insert(0, context);

        // Keep only last 5
        if (history.Count > 5)
            history = history.Take(5).ToList();

        var historyJson = JsonSerializer.Serialize(history);
        session.SetString("ContextHistory", historyJson);
    }

    /// <summary>
    /// Get a summary of the current context for display.
    /// </summary>
    public string GetContextDisplayName(FirmwareContext? context = null)
    {
        context ??= GetCurrentContext();
        if (context == null) return "No context set";

        return $"{context.NpiProgramName} - LPI {context.LpiVersionNumber} + PICCOLO {context.PiccoloVersionNumber}";
    }

    /// <summary>
    /// Check if a collection is compatible with the current context.
    /// </summary>
    public bool IsCollectionCompatible(FeatureCollection collection, FirmwareContext? context = null)
    {
        context ??= GetCurrentContext();
        if (context == null) return false;

        return collection.NpiProgramId == context.NpiProgramId &&
               collection.RequiredLpiVersionId == context.LpiVersionId &&
               collection.RequiredPiccoloVersionId == context.PiccoloVersionId;
    }

    /// <summary>
    /// Get compatibility status for a collection against the current context.
    /// </summary>
    public string GetCollectionCompatibilityStatus(FeatureCollection collection, FirmwareContext? context = null)
    {
        context ??= GetCurrentContext();
        if (context == null) return "unknown";

        if (IsCollectionCompatible(collection, context))
            return "compatible";

        // Check partial compatibility
        int matches = 0;
        if (collection.NpiProgramId == context.NpiProgramId) matches++;
        if (collection.RequiredLpiVersionId == context.LpiVersionId) matches++;
        if (collection.RequiredPiccoloVersionId == context.PiccoloVersionId) matches++;

        if (matches == 2) return "partial";
        if (matches == 1) return "test";

        return "incompatible";
    }
}

/// <summary>
/// Represents a firmware context (NPI program + LPI version + PICCOLO version).
/// </summary>
public class FirmwareContext
{
    public Guid NpiProgramId { get; set; }
    public string NpiProgramName { get; set; } = string.Empty;
    public Guid LpiVersionId { get; set; }
    public string LpiVersionNumber { get; set; } = string.Empty;
    public Guid PiccoloVersionId { get; set; }
    public string PiccoloVersionNumber { get; set; } = string.Empty;
    public DateTime LastModified { get; set; }
}
