using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages.Collections;

public class IndexModel : PageModel
{
    private readonly FirmwareDataService _dataService;
    private readonly ContextService _contextService;

    public IndexModel(FirmwareDataService dataService, ContextService contextService)
    {
        _dataService = dataService;
        _contextService = contextService;
    }

    public List<FeatureCollection> Collections { get; set; } = new();
    public List<FeatureCollection> AllCollections { get; set; } = new();
    public FirmwareContext CurrentContext { get; set; } = null!;
    public NpiProgram? CurrentProgram { get; set; }
    public FirmwareVersion? CurrentLpiVersion { get; set; }
    public FirmwareVersion? CurrentPiccoloVersion { get; set; }

    // Dropdown lists for context switcher
    public List<NpiProgram> AvailablePrograms { get; set; } = new();
    public List<FirmwareVersion> AvailableLpiVersions { get; set; } = new();
    public List<FirmwareVersion> AvailablePiccoloVersions { get; set; } = new();

    // Multi-collection workspace
    [BindProperty(SupportsGet = true)]
    public List<Guid> SelectedCollectionIds { get; set; } = new();
    public List<FeatureCollection> SelectedCollections { get; set; } = new();

    // Chat context for AI assistance
    public ChatContext CollectionsChatContext { get; set; } = new();

    // Compatibility checking
    public string GetCompatibilityStatus(FeatureCollection collection)
    {
        return _contextService.GetCollectionCompatibilityStatus(collection, CurrentContext);
    }

    public string GetCompatibilityBadge(string status)
    {
        return status switch
        {
            "compatible" => "<span class='badge bg-success'><i class='bi bi-check-circle'></i> Compatible</span>",
            "partial" => "<span class='badge bg-warning text-dark'><i class='bi bi-exclamation-triangle'></i> Partial</span>",
            "test" => "<span class='badge bg-info'><i class='bi bi-flask'></i> Test Build</span>",
            "incompatible" => "<span class='badge bg-danger'><i class='bi bi-x-circle'></i> Incompatible</span>",
            _ => "<span class='badge bg-secondary'><i class='bi bi-question-circle'></i> Unknown</span>"
        };
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

    public void OnGet(Guid? programId, Guid? lpiId, Guid? piccoloId)
    {
        AllCollections = _dataService.GetCollections();
        
        // Get or create current context
        CurrentContext = _contextService.GetOrCreateDefaultContext();
        
        // Load context options
        AvailablePrograms = _dataService.GetNpiPrograms();
        
        // Get current program/versions from context
        CurrentProgram = AvailablePrograms.FirstOrDefault(p => p.Id == CurrentContext.NpiProgramId);
        
        if (CurrentProgram != null)
        {
            AvailableLpiVersions = _dataService.GetLpiVersions(CurrentProgram.Id);
            AvailablePiccoloVersions = _dataService.GetPiccoloVersions(CurrentProgram.Id);
            
            CurrentLpiVersion = AvailableLpiVersions.FirstOrDefault(v => v.Id == CurrentContext.LpiVersionId);
            CurrentPiccoloVersion = AvailablePiccoloVersions.FirstOrDefault(v => v.Id == CurrentContext.PiccoloVersionId);
        }

        // Filter collections by compatibility (show all by default, can be filtered by UI)
        Collections = AllCollections
            .OrderByDescending(c => _contextService.IsCollectionCompatible(c, CurrentContext))
            .ThenBy(c => c.Name)
            .ToList();

        // Load selected collections for multi-collection workspace
        if (SelectedCollectionIds.Any())
        {
            SelectedCollections = AllCollections
                .Where(c => SelectedCollectionIds.Contains(c.Id))
                .ToList();
        }

        // Setup chat context
        CollectionsChatContext = new ChatContext
        {
            PageContext = ChatPageContext.CollectionsList,
            CurrentFirmwareVersion = CurrentLpiVersion?.Version ?? CurrentContext.LpiVersionNumber,
            AvailableCollections = Collections.Select(c => c.Name).ToList(),
            CurrentNpiProgram = CurrentProgram?.Name,
            SessionId = HttpContext.Session.Id,
            AdditionalData = new Dictionary<string, object>
            {
                ["TotalCollections"] = Collections.Count,
                ["CompatibleCount"] = Collections.Count(c => _contextService.IsCollectionCompatible(c, CurrentContext))
            }
        };
    }

    public JsonResult OnGetVersions(Guid programId)
    {
        var lpi = _dataService.GetLpiVersions(programId)
            .Select(v => new { value = v.Id, text = $"{v.Version} ({v.Stage})" });
            
        var piccolo = _dataService.GetPiccoloVersions(programId)
            .Select(v => new { value = v.Id, text = $"{v.Version} ({v.Stage})" });
            
        return new JsonResult(new { lpi, piccolo });
    }
}
