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

    public void OnGet()
    {
        Collections = _dataService.GetCollections();
        
        // Set current context (Eagan 3.2.1 / 2.5.0)
        CurrentProgram = _dataService.GetNpiPrograms().FirstOrDefault(p => p.Name == "Eagan");
        if (CurrentProgram != null)
        {
            CurrentLpiVersion = _dataService.GetLpiVersions(CurrentProgram.Id)
                .FirstOrDefault(v => v.Version == "3.2.1");
            CurrentPiccoloVersion = _dataService.GetPiccoloVersions(CurrentProgram.Id)
                .FirstOrDefault(v => v.Version == "2.5.0");
        }
    }

    public string GetCompatibilityStatus(FeatureCollection collection)
    {
        if (CurrentLpiVersion == null || CurrentPiccoloVersion == null)
            return "unknown";

        var lpiMatch = collection.RequiredLpiVersion?.Version == CurrentLpiVersion.Version;
        var piccoloMatch = collection.RequiredPiccoloVersion?.Version == CurrentPiccoloVersion.Version;

        if (lpiMatch && piccoloMatch)
            return "compatible";
        
        var lpiMajorMatch = collection.RequiredLpiVersion?.Version.Split('.')[0] == CurrentLpiVersion.Version.Split('.')[0];
        var piccoloMajorMatch = collection.RequiredPiccoloVersion?.Version.Split('.')[0] == CurrentPiccoloVersion.Version.Split('.')[0];
        
        if (lpiMajorMatch && piccoloMajorMatch)
            return "partial";
        
        return "incompatible";
    }

    public string GetCompatibilityIcon(FeatureCollection collection)
    {
        return GetCompatibilityStatus(collection) switch
        {
            "compatible" => "✓",
            "partial" => "⚠️",
            "incompatible" => "❌",
            _ => collection.RequiredLpiVersion?.Stage == FirmwareReleaseStage.T0_Test ? "🧪" : "?"
        };
    }

    public string GetCompatibilityClass(FeatureCollection collection)
    {
        return GetCompatibilityStatus(collection) switch
        {
            "compatible" => "text-success",
            "partial" => "text-warning",
            "incompatible" => "text-danger",
            _ => "text-muted"
        };
    }
}
