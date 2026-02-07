using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly FirmwareDataService _firmwareService;

    public IndexModel(FirmwareDataService firmwareService)
    {
        _firmwareService = firmwareService;
    }

    public List<NpiProgram> NpiPrograms { get; set; } = new();
    public List<FirmwareVersion> LpiVersions { get; set; } = new();
    public List<FirmwareVersion> PiccoloVersions { get; set; } = new();
    public List<Feature> Features { get; set; } = new();
    public List<FeatureCollection> Collections { get; set; } = new();

    // Global Context (default to first available)
    public Guid SelectedNpiProgramId { get; set; }
    public Guid SelectedLpiVersionId { get; set; }
    public Guid SelectedPiccoloVersionId { get; set; }

    // Stats
    public int TotalFeatures => Features.Count;
    public int TotalCollections => Collections.Count;
    public int TotalNpiPrograms => NpiPrograms.Count;
    public int TotalVersions => LpiVersions.Count + PiccoloVersions.Count;

    // Compatibility matrix
    public Dictionary<Guid, Dictionary<Guid, string>> FeatureCompatibilityMatrix { get; set; } = new();

    public void OnGet()
    {
        NpiPrograms = _firmwareService.GetNpiPrograms();
        LpiVersions = _firmwareService.GetLpiVersions();
        PiccoloVersions = _firmwareService.GetPiccoloVersions();
        Features = _firmwareService.GetFeatures();
        Collections = _firmwareService.GetCollections();

        // Set default context to first Eagan versions (if available)
        var eagan = NpiPrograms.FirstOrDefault(p => p.Name == "Eagan") ?? NpiPrograms.FirstOrDefault();
        if (eagan != null)
        {
            SelectedNpiProgramId = eagan.Id;
            var eaganLpi = LpiVersions.FirstOrDefault(v => v.NpiProgramId == eagan.Id && v.Stage == FirmwareReleaseStage.T3_Released);
            var eaganPiccolo = PiccoloVersions.FirstOrDefault(v => v.NpiProgramId == eagan.Id && v.Stage == FirmwareReleaseStage.T3_Released);

            SelectedLpiVersionId = eaganLpi?.Id ?? Guid.Empty;
            SelectedPiccoloVersionId = eaganPiccolo?.Id ?? Guid.Empty;
        }

        // Build compatibility matrix
        BuildCompatibilityMatrix();
    }

    private void BuildCompatibilityMatrix()
    {
        // For each feature, determine compatibility with each firmware version combination
        foreach (var feature in Features)
        {
            var featureCompat = new Dictionary<Guid, string>();
            
            foreach (var npiProgram in NpiPrograms)
            {
                // Simple compatibility logic (can be enhanced based on actual feature requirements)
                // For POC: Features are compatible with T2/T3 versions, experimental with T1, incompatible with T0
                var lpiVersions = LpiVersions.Where(v => v.NpiProgramId == npiProgram.Id);
                var piccoloVersions = PiccoloVersions.Where(v => v.NpiProgramId == npiProgram.Id);
                
                var hasT3Lpi = lpiVersions.Any(v => v.Stage == FirmwareReleaseStage.T3_Released);
                var hasT2Lpi = lpiVersions.Any(v => v.Stage == FirmwareReleaseStage.T2_Beta);
                var hasT3Piccolo = piccoloVersions.Any(v => v.Stage == FirmwareReleaseStage.T3_Released);
                var hasT2Piccolo = piccoloVersions.Any(v => v.Stage == FirmwareReleaseStage.T2_Beta);
                
                // Compatibility status: compatible (✓), partial (⚠️), incompatible (❌), experimental (🧪)
                string status = "unknown";
                if (hasT3Lpi && hasT3Piccolo)
                    status = "compatible"; // ✓
                else if (hasT2Lpi || hasT2Piccolo)
                    status = "partial"; // ⚠️
                else if (lpiVersions.Any(v => v.Stage == FirmwareReleaseStage.T1_Alpha) || 
                         piccoloVersions.Any(v => v.Stage == FirmwareReleaseStage.T1_Alpha))
                    status = "experimental"; // 🧪
                else
                    status = "incompatible"; // ❌
                
                featureCompat[npiProgram.Id] = status;
            }
            
            FeatureCompatibilityMatrix[feature.Id] = featureCompat;
        }
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
}
