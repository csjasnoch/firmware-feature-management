using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages.Features;

public class EditModel : PageModel
{
    private readonly FirmwareDataService _dataService;

    public EditModel(FirmwareDataService dataService)
    {
        _dataService = dataService;
    }

    [BindProperty]
    public Feature? Feature { get; set; }
    
    public List<ParameterDefinition> AvailableParameters { get; set; } = new();
    public List<CommandDefinition> AvailableCommands { get; set; } = new();
    
    // Current firmware context
    public FeatureCollection? Collection { get; set; }
    public FirmwareVersion? CurrentLpiVersion { get; set; }
    public FirmwareVersion? CurrentPiccoloVersion { get; set; }
    
    // Parameter grouping for Settings tabs
    public List<IGrouping<string, ParameterValue>> ParamsByGroup { get; set; } = new();
    public bool HasMultipleSettings { get; set; }

    public IActionResult OnGet(Guid id)
    {
        Feature = _dataService.GetFeature(id);
        if (Feature == null)
        {
            return NotFound();
        }

        // Find collection containing this feature (First finding Eagan directionality for demo consistency, or actual parent)
        Collection = _dataService.GetCollections().FirstOrDefault(c => c.Features.Any(f => f.Id == id));

        LoadFirmwareContext();
        GroupParameters();
        return Page();
    }

    public IActionResult OnPost()
    {
        if (Feature == null)
        {
            return NotFound();
        }

        // In a real app, this would update the database
        // For now, just redirect back
        TempData["SuccessMessage"] = $"Feature '{Feature.Name}' updated successfully!";
        return RedirectToPage("/Features/Edit", new { id = Feature.Id });
    }

    public IActionResult OnPostAddParameter(Guid id, int parameterId)
    {
        Feature = _dataService.GetFeature(id);
        if (Feature == null)
        {
            return NotFound();
        }

        LoadFirmwareContext();
        
        var paramDef = CurrentLpiVersion?.Parameters.FirstOrDefault(p => p.Id == parameterId);
        if (paramDef != null && !Feature.Parameters.Any(p => p.ParameterId == parameterId))
        {
            Feature.Parameters.Add(new ParameterValue
            {
                Id = Guid.NewGuid(),
                ParameterId = paramDef.Id,
                ParameterName = paramDef.Name,
                Value = paramDef.DefaultValue,
                Notes = ""
            });
        }

        TempData["SuccessMessage"] = $"Parameter '{paramDef?.Name}' added";
        return RedirectToPage("/Features/Edit", new { id = Feature.Id });
    }

    public IActionResult OnPostRemoveParameter(Guid id, Guid parameterId)
    {
        Feature = _dataService.GetFeature(id);
        if (Feature == null)
        {
            return NotFound();
        }

        var param = Feature.Parameters.FirstOrDefault(p => p.Id == parameterId);
        if (param != null)
        {
            Feature.Parameters.Remove(param);
            TempData["SuccessMessage"] = $"Parameter '{param.ParameterName}' removed";
        }

        return RedirectToPage("/Features/Edit", new { id = Feature.Id });
    }

    private void LoadFirmwareContext()
    {
        if (Collection != null)
        {
            CurrentLpiVersion = Collection.RequiredLpiVersion;
            CurrentPiccoloVersion = Collection.RequiredPiccoloVersion;
            
            if (CurrentLpiVersion != null)
            {
                AvailableParameters = CurrentLpiVersion.Parameters;
            }
            if (CurrentPiccoloVersion != null)
            {
                AvailableCommands = CurrentPiccoloVersion.Commands;
            }
        }
        else 
        {
            // Get current Eagan firmware versions
            var eagan = _dataService.GetNpiPrograms().FirstOrDefault(p => p.Name == "Eagan");
            if (eagan != null)
            {
                CurrentLpiVersion = _dataService.GetLpiVersions(eagan.Id)
                    .FirstOrDefault(v => v.Version == "3.2.1");
                CurrentPiccoloVersion = _dataService.GetPiccoloVersions(eagan.Id)
                    .FirstOrDefault(v => v.Version == "2.5.0");

                if (CurrentLpiVersion != null)
                {
                    AvailableParameters = CurrentLpiVersion.Parameters;
                }
                if (CurrentPiccoloVersion != null)
                {
                    AvailableCommands = CurrentPiccoloVersion.Commands;
                }
            }
        }
    }
    
    private void GroupParameters()
    {
        if (Feature == null) return;
        
        ParamsByGroup = Feature.Parameters
            .GroupBy(p => GetParameterGroup(p))
            .OrderBy(g => g.Key)
            .ToList();
        
        HasMultipleSettings = ParamsByGroup.Count > 1 && ParamsByGroup.Any(g => g.Key != "General Parameters");
    }
    
    private string GetParameterGroup(ParameterValue p)
    {
        // Extract setting from parameter name (e.g., "DirectionalityPerMemory1.Mode" -> "Memory 1")
        if (p.ParameterName.Contains("Memory1")) return "Memory 1";
        if (p.ParameterName.Contains("Memory2")) return "Memory 2";
        if (p.ParameterName.Contains("Memory3")) return "Memory 3";
        if (p.ParameterName.Contains("Memory4")) return "Memory 4";
        if (p.ParameterName.Contains("Memory5")) return "Memory 5";
        if (p.ParameterName.Contains("BinauralLeft")) return "Binaural Left";
        if (p.ParameterName.Contains("BinauralRight")) return "Binaural Right";
        if (p.ParameterName.Contains("Channel"))
        {
            var match = System.Text.RegularExpressions.Regex.Match(p.ParameterName, @"Channel(\d+)");
            if (match.Success) return $"Channel {match.Groups[1].Value}";
        }
        return "General Parameters";
    }
}
