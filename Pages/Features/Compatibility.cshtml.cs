using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages.Features;

/// <summary>
/// Feature compatibility analyzer page
/// NOTE: Simplified implementation - full compatibility requires model updates
/// </summary>
public class CompatibilityModel : PageModel
{
    private readonly FirmwareDataService _firmwareService;
    private readonly ContextService _contextService;

    public CompatibilityModel(FirmwareDataService firmwareService, ContextService contextService)
    {
        _firmwareService = firmwareService;
        _contextService = contextService;
    }

    public List<Feature> AvailableFeatures { get; set; } = new();
    public List<FirmwareVersion> AvailableVersions { get; set; } = new();
    public CompatibilityAnalysis? AnalysisResult { get; set; }
    public ChatContext CompatibilityChatContext { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public Guid? SelectedFeatureId { get; set; }

    [BindProperty(SupportsGet = true)]
    public Guid? SelectedTargetVersionId { get; set; }

    public async Task OnGetAsync()
    {
        await LoadBaseDataAsync();
        SetupChatContext(null, null);
    }

    public async Task<IActionResult> OnPostAnalyzeAsync(Guid featureId, Guid targetVersionId)
    {
        SelectedFeatureId = featureId;
        SelectedTargetVersionId = targetVersionId;

        await LoadBaseDataAsync();

        var feature = AvailableFeatures.FirstOrDefault(f => f.Id == featureId);
        var targetVersion = AvailableVersions.FirstOrDefault(v => v.Id == targetVersionId);

        if (feature == null || targetVersion == null)
        {
            ModelState.AddModelError("", "Invalid feature or target version selected");
            SetupChatContext(null, null);
            return Page();
        }

        // Perform compatibility analysis
        AnalysisResult = await _firmwareService.AnalyzeCompatibilityAsync(feature, targetVersion);

        SetupChatContext(feature, targetVersion);
        return Page();
    }

    public async Task<IActionResult> OnPostApplyAsync(Guid featureId, Guid targetVersionId)
    {
        await LoadBaseDataAsync();
        
        var feature = AvailableFeatures.FirstOrDefault(f => f.Id == featureId);
        var targetVersion = AvailableVersions.FirstOrDefault(v => v.Id == targetVersionId);

        if (feature == null || targetVersion == null)
        {
            TempData["ErrorMessage"] = "Invalid feature or target version";
            return RedirectToPage();
        }

        // Perform adaptation with simplified mappings
        var adaptedFeature = await _firmwareService.AdaptFeatureAsync(feature, targetVersion, new List<ParameterMapping>());

        TempData["SuccessMessage"] = $"Feature '{feature.Name}' has been successfully adapted for {targetVersion.Version}!";
        return RedirectToPage("/Features/Index");
    }

    private async Task LoadBaseDataAsync()
    {
        AvailableFeatures = await _firmwareService.GetAllFeaturesAsync();
        AvailableVersions = await _firmwareService.GetAllFirmwareVersionsAsync();
    }

    private void SetupChatContext(Feature? sourceFeature, FirmwareVersion? targetVersion)
    {
        var currentContext = _contextService.GetOrCreateDefaultContext();
        
        CompatibilityChatContext = new ChatContext
        {
            PageContext = ChatPageContext.FeatureCompatibility,
            CurrentFeatureId = sourceFeature?.Id.ToString(),
            CurrentFeatureName = sourceFeature?.Name,
            CurrentFirmwareVersion = targetVersion?.Version ?? currentContext.LpiVersionNumber ?? "Unknown",
            CurrentNpiProgram = currentContext.NpiProgramName,
            SessionId = HttpContext.Session.Id,
            AdditionalData = new Dictionary<string, object>
            {
                ["SourceFeatureVersion"] = "N/A (features not version-specific in current model)",
                ["TargetVersion"] = targetVersion?.Version ?? "N/A",
                ["HasAnalysis"] = AnalysisResult != null
            }
        };

        // Add analysis details if available
        if (AnalysisResult != null)
        {
            CompatibilityChatContext.AdditionalData["CompatibilityScore"] = AnalysisResult.CompatibilityScore;
            CompatibilityChatContext.AdditionalData["BreakingChangesCount"] = AnalysisResult.BreakingChanges.Count;
            CompatibilityChatContext.AdditionalData["CanAutoAdapt"] = AnalysisResult.CanAutoAdapt;
        }
    }
}
