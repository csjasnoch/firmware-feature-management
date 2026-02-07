using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages.Collections;

public class IndexModel : PageModel
{
    private readonly FirmwareDataService _dataService;

    public IndexModel(FirmwareDataService dataService)
    {
        _dataService = dataService;
    }

    public List<FeatureCollection> Collections { get; set; } = new();
    public NpiProgram? CurrentProgram { get; set; }
    public FirmwareVersion? CurrentLpiVersion { get; set; }
    public FirmwareVersion? CurrentPiccoloVersion { get; set; }

    // Dropdown lists for context switcher
    public List<NpiProgram> AvailablePrograms { get; set; } = new();
    public List<FirmwareVersion> AvailableLpiVersions { get; set; } = new();
    public List<FirmwareVersion> AvailablePiccoloVersions { get; set; } = new();

    // Compatibility checking
    public string GetCompatibilityStatus(FeatureCollection collection)
    {
        if (CurrentLpiVersion == null || CurrentPiccoloVersion == null) return "unknown";

        bool lpiMatch = collection.RequiredLpiVersionId == CurrentLpiVersion.Id;
        bool piccoloMatch = collection.RequiredPiccoloVersionId == CurrentPiccoloVersion.Id;

        // Check if versions are test builds
        bool hasTestVersions = collection.RequiredLpiVersion?.Stage == FirmwareReleaseStage.T0_Test ||
                                collection.RequiredPiccoloVersion?.Stage == FirmwareReleaseStage.T0_Test;

        if (lpiMatch && piccoloMatch) return "compatible";
        if (hasTestVersions) return "test";
        if (lpiMatch || piccoloMatch) return "partial";
        return "incompatible";
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
        Collections = _dataService.GetCollections();
        
        // Load context options
        AvailablePrograms = _dataService.GetNpiPrograms();

        // Determine Program
        if (programId.HasValue)
            CurrentProgram = AvailablePrograms.FirstOrDefault(p => p.Id == programId.Value);
        else
            CurrentProgram = AvailablePrograms.FirstOrDefault(p => p.Name == "Eagan");

        if (CurrentProgram != null)
        {
            AvailableLpiVersions = _dataService.GetLpiVersions(CurrentProgram.Id);
            AvailablePiccoloVersions = _dataService.GetPiccoloVersions(CurrentProgram.Id);

            // Determine LPI Version
            if (lpiId.HasValue)
                CurrentLpiVersion = AvailableLpiVersions.FirstOrDefault(v => v.Id == lpiId.Value);
            
            if (CurrentLpiVersion == null)
            {
                // Default logic
                 CurrentLpiVersion = AvailableLpiVersions.FirstOrDefault(v => v.Version == "3.2.1") 
                                     ?? AvailableLpiVersions.FirstOrDefault();
            }

            // Determine Piccolo Version
             if (piccoloId.HasValue)
                CurrentPiccoloVersion = AvailablePiccoloVersions.FirstOrDefault(v => v.Id == piccoloId.Value);

             if (CurrentPiccoloVersion == null)
             {
                 CurrentPiccoloVersion = AvailablePiccoloVersions.FirstOrDefault(v => v.Version == "2.5.0")
                                         ?? AvailablePiccoloVersions.FirstOrDefault();
             }
        }
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
