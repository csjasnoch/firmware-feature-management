using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly FirmwareDataService _firmwareService;
    private readonly ContextService _contextService;

    public IndexModel(FirmwareDataService firmwareService, ContextService contextService)
    {
        _firmwareService = firmwareService;
        _contextService = contextService;
    }

    public List<NpiProgram> NpiPrograms { get; set; } = new();
    public List<FirmwareVersion> LpiVersions { get; set; } = new();
    public List<FirmwareVersion> PiccoloVersions { get; set; } = new();
    public List<FeatureCollection> Collections { get; set; } = new();
    public FirmwareContext CurrentContext { get; set; } = null!;
    public List<FirmwareContext> ContextHistory { get; set; } = new();

    // Collection insights
    public List<FeatureCollection> ActiveCollections { get; set; } = new();
    public List<FeatureCollection> RecentCollections { get; set; } = new();
    public Dictionary<string, int> FeatureReuseStats { get; set; } = new();
    public Dictionary<string, int> VersionDistribution { get; set; } = new();

    // Chat context for AI assistance
    public ChatContext DashboardChatContext { get; set; } = new();

    // Stats
    public int TotalFeatures { get; set; }
    public int TotalCollections => Collections.Count;
    public int TotalNpiPrograms => NpiPrograms.Count;
    public int CompatibleCollectionsCount { get; set; }

    public void OnGet()
    {
        // Load all base data
        NpiPrograms = _firmwareService.GetNpiPrograms();
        LpiVersions = _firmwareService.GetLpiVersions();
        PiccoloVersions = _firmwareService.GetPiccoloVersions();
        Collections = _firmwareService.GetCollections();

        // Get or create current context
        CurrentContext = _contextService.GetOrCreateDefaultContext();
        ContextHistory = _contextService.GetContextHistory();

        // Calculate stats
        TotalFeatures = _firmwareService.GetFeatures().Count;
        CompatibleCollectionsCount = Collections.Count(c => _contextService.IsCollectionCompatible(c, CurrentContext));

        // Get active collections (simulate user's active work - for POC, use first 3 collections)
        ActiveCollections = Collections.Take(3).ToList();

        // Get recent collections (last modified - for POC, use last 5)
        RecentCollections = Collections.OrderByDescending(c => c.ModifiedAt).Take(5).ToList();

        // Calculate feature reuse stats (which features appear in multiple collections)
        CalculateFeatureReuseStats();

        // Calculate version distribution (how many collections per firmware version)
        CalculateVersionDistribution();

        // Setup chat context
        DashboardChatContext = new ChatContext
        {
            PageContext = ChatPageContext.Dashboard,
            CurrentFirmwareVersion = _firmwareService.GetFirmwareVersionById(CurrentContext.LpiVersionId)?.Version ?? CurrentContext.LpiVersionNumber,  
            AvailableCollections = Collections.Select(c => c.Name).ToList(),
            CurrentNpiProgram = CurrentContext.NpiProgramName,
            SessionId = HttpContext.Session.Id,
            AdditionalData = new Dictionary<string, object>
            {
                ["TotalFeatures"] = TotalFeatures,
                ["TotalCollections"] = TotalCollections,
                ["RecentCollections"] = RecentCollections.Select(c => c.Name).ToList()
            }
        };
    }

    public IActionResult OnPost(string npiProgramId, string lpiVersionId, string piccoloVersionId)
    {
        // Update context
        var npiProgram = _firmwareService.GetNpiProgramById(Guid.Parse(npiProgramId));
        var lpiVersion = _firmwareService.GetFirmwareVersionById(Guid.Parse(lpiVersionId));
        var piccoloVersion = _firmwareService.GetFirmwareVersionById(Guid.Parse(piccoloVersionId));

        if (npiProgram != null && lpiVersion != null && piccoloVersion != null)
        {
            var context = new FirmwareContext
            {
                NpiProgramId = npiProgram.Id,
                NpiProgramName = npiProgram.Name,
                LpiVersionId = lpiVersion.Id,
                LpiVersionNumber = lpiVersion.Version,
                PiccoloVersionId = piccoloVersion.Id,
                PiccoloVersionNumber = piccoloVersion.Version
            };

            _contextService.SetCurrentContext(context);
        }

        return RedirectToPage();
    }

    private void CalculateFeatureReuseStats()
    {
        // Count how many times each feature appears across collections
        var featureCounts = new Dictionary<string, int>();

        foreach (var collection in Collections)
        {
            foreach (var feature in collection.Features)
            {
                if (!featureCounts.ContainsKey(feature.Name))
                    featureCounts[feature.Name] = 0;
                featureCounts[feature.Name]++;
            }
        }

        // Get features that appear in 2+ collections
        FeatureReuseStats = featureCounts
            .Where(kvp => kvp.Value > 1)
            .OrderByDescending(kvp => kvp.Value)
            .Take(10)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private void CalculateVersionDistribution()
    {
        // Count collections per firmware version combination
        var versionCombos = new Dictionary<string, int>();

        foreach (var collection in Collections)
        {
            var lpiVersion = LpiVersions.FirstOrDefault(v => v.Id == collection.RequiredLpiVersionId);
            var piccoloVersion = PiccoloVersions.FirstOrDefault(v => v.Id == collection.RequiredPiccoloVersionId);

            if (lpiVersion != null && piccoloVersion != null)
            {
                var key = $"LPI {lpiVersion.Version} + PICCOLO {piccoloVersion.Version}";
                if (!versionCombos.ContainsKey(key))
                    versionCombos[key] = 0;
                versionCombos[key]++;
            }
        }

        VersionDistribution = versionCombos
            .OrderByDescending(kvp => kvp.Value)
            .Take(5)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    public string GetStageBadgeClass(FirmwareReleaseStage stage)
    {
        return stage switch
        {
            FirmwareReleaseStage.T0_Test => "bg-secondary",
            FirmwareReleaseStage.T1_Alpha => "bg-warning text-dark",
            FirmwareReleaseStage.T2_Beta => "bg-info",
            FirmwareReleaseStage.T3_Released => "bg-success",
            _ => "bg-secondary"
        };
    }

    public string GetStageDisplayName(FirmwareReleaseStage stage)
    {
        return stage switch
        {
            FirmwareReleaseStage.T0_Test => "T0 Test",
            FirmwareReleaseStage.T1_Alpha => "T1 Alpha",
            FirmwareReleaseStage.T2_Beta => "T2 Beta",
            FirmwareReleaseStage.T3_Released => "T3 Released",
            _ => stage.ToString()
        };
    }

    public string GetCompatibilityBadge(FeatureCollection collection)
    {
        var status = _contextService.GetCollectionCompatibilityStatus(collection, CurrentContext);
        return status switch
        {
            "compatible" => "<span class='badge bg-success'>✓ Compatible</span>",
            "partial" => "<span class='badge bg-warning text-dark'>⚠️ Partial</span>",
            "test" => "<span class='badge bg-info'>🧪 Test</span>",
            _ => "<span class='badge bg-danger'>❌ Incompatible</span>"
        };
    }
}
