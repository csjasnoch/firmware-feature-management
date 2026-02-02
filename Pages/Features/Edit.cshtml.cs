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
    public FirmwareVersion? CurrentLpiVersion { get; set; }
    public FirmwareVersion? CurrentPiccoloVersion { get; set; }

    public IActionResult OnGet(Guid id)
    {
        Feature = _dataService.GetFeature(id);
        if (Feature == null)
        {
            return NotFound();
        }

        LoadFirmwareContext();
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
