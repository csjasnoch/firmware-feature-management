using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages.Collections;

public class CreateModel : PageModel
{
    private readonly FirmwareDataService _dataService;

    public CreateModel(FirmwareDataService dataService)
    {
        _dataService = dataService;
    }

    [BindProperty]
    public FeatureCollection Collection { get; set; } = new();

    [BindProperty]
    public List<Guid> SelectedFeatureIds { get; set; } = new();

    [BindProperty]
    public string TagsInput { get; set; } = string.Empty;

    [BindProperty]
    public string SharedWithInput { get; set; } = string.Empty;

    public List<SelectListItem> NpiProgramOptions { get; set; } = new();
    public List<SelectListItem> LpiVersionOptions { get; set; } = new();
    public List<SelectListItem> PiccoloVersionOptions { get; set; } = new();
    public List<Feature>? AvailableFeatures { get; set; }

    public void OnGet()
    {
        // Load initial lists
        var programs = _dataService.GetNpiPrograms();
        NpiProgramOptions = programs.Select(p => new SelectListItem
        {
            Value = p.Id.ToString(),
            Text = $"{p.Name} ({p.HardwareGen})"
        }).ToList();
        
        // Load all features for the wizard (will be filtered client-side)
        AvailableFeatures = _dataService.GetAllFeatures();
        
        // Defaults if programs exist
        if (programs.Any())
        {
            var defaultProgram = programs.FirstOrDefault(p => p.Name == "Eagan") ?? programs.First();
            var defaultProgramId = defaultProgram.Id;
             
             // Pre-populate versions for the default program (or handle via JS)
            LpiVersionOptions = _dataService.GetLpiVersions(defaultProgramId)
                .Select(v => new SelectListItem { Value = v.Id.ToString(), Text = $"{v.Version} ({v.Stage})" })
                .ToList();
                
            PiccoloVersionOptions = _dataService.GetPiccoloVersions(defaultProgramId)
                .Select(v => new SelectListItem { Value = v.Id.ToString(), Text = $"{v.Version} ({v.Stage})" })
                .ToList();
        }
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            // Reload options if validation fails
            OnGet();
            return Page();
        }

        // Parse tags
        if (!string.IsNullOrEmpty(TagsInput))
        {
            Collection.Tags = TagsInput.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrEmpty(t))
                .ToList();
        }

        // Parse shared with
        if (Collection.Visibility == CollectionVisibility.Team && !string.IsNullOrEmpty(SharedWithInput))
        {
            Collection.SharedWith = SharedWithInput.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(u => u.Trim())
                .Where(u => !string.IsNullOrEmpty(u))
                .ToList();
        }

        // Add selected features
        if (SelectedFeatureIds.Any())
        {
            var allFeatures = _dataService.GetAllFeatures();
            Collection.Features = allFeatures.Where(f => SelectedFeatureIds.Contains(f.Id)).ToList();
        }
        else
        {
            Collection.Features = new List<Feature>();
        }

        // Set metadata
        Collection.CreatedAt = DateTime.UtcNow;
        Collection.ModifiedAt = DateTime.UtcNow;
        Collection.Owner = "Current User"; // Mock user - in real app, get from auth
        Collection.CreatedBy = "Current User";
        
        // Get the program to set reference
        if (Collection.NpiProgramId != Guid.Empty)
        {
            Collection.TargetProgram = _dataService.GetNpiPrograms()
                .FirstOrDefault(p => p.Id == Collection.NpiProgramId);
        }

        // Get version references
        if (Collection.RequiredLpiVersionId != Guid.Empty)
        {
            var lpiVersions = _dataService.GetLpiVersions(Collection.NpiProgramId);
            Collection.RequiredLpiVersion = lpiVersions.FirstOrDefault(v => v.Id == Collection.RequiredLpiVersionId);
        }

        if (Collection.RequiredPiccoloVersionId != Guid.Empty)
        {
            var piccoloVersions = _dataService.GetPiccoloVersions(Collection.NpiProgramId);
            Collection.RequiredPiccoloVersion = piccoloVersions.FirstOrDefault(v => v.Id == Collection.RequiredPiccoloVersionId);
        }
        
        _dataService.AddCollection(Collection);

        return RedirectToPage("./Index");
    }
    
    // AJAX handler to get versions for a program
    public JsonResult OnGetVersions(Guid programId)
    {
        var lpi = _dataService.GetLpiVersions(programId)
            .Select(v => new { value = v.Id, text = $"{v.Version} ({v.Stage})" });
            
        var piccolo = _dataService.GetPiccoloVersions(programId)
            .Select(v => new { value = v.Id, text = $"{v.Version} ({v.Stage})" });
            
        return new JsonResult(new { lpi, piccolo });
    }

    // AJAX handler to get compatible features
    public JsonResult OnGetFeatures(Guid lpiVersionId, Guid piccoloVersionId)
    {
        var allFeatures = _dataService.GetAllFeatures();
        
        // In a real implementation, filter features based on version compatibility
        // For now, return all features with compatibility info
        var features = allFeatures.Select(f => new
        {
            id = f.Id,
            name = f.Name,
            description = f.Description,
            category = f.Category,
            isComposite = f.IsComposite,
            requiredLpiParams = f.RequiredLpiParams?.Count ?? 0,
            requiredPiccoloCommands = f.RequiredPiccoloCommands?.Count ?? 0
        });

        return new JsonResult(features);
    }
}
