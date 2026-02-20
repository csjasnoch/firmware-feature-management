using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages.Collections;

public class DetailsModel : PageModel
{
    private readonly FirmwareDataService _dataService;
    private readonly ContextService _contextService;

    public DetailsModel(FirmwareDataService dataService, ContextService contextService)
    {
        _dataService = dataService;
        _contextService = contextService;
    }

    public FeatureCollection? Collection { get; set; }
    public FirmwareContext CurrentContext { get; set; } = null!;
    public List<FeatureCollection> AllCollections { get; set; } = new();
    public List<Feature> AllAvailableFeatures { get; set; } = new();
    
    [BindProperty(SupportsGet = true)]
    public List<Guid> CompareWithCollections { get; set; } = new();
    public List<FeatureCollection> ComparisonCollections { get; set; } = new();

    public IActionResult OnGet(Guid id)
    {
        Collection = _dataService.GetCollection(id);
        if (Collection == null)
        {
            return NotFound();
        }

        CurrentContext = _contextService.GetOrCreateDefaultContext();
        AllCollections = _dataService.GetCollections();
        
        // Get comparison collections if specified
        if (CompareWithCollections.Any())
        {
            ComparisonCollections = AllCollections
                .Where(c => CompareWithCollections.Contains(c.Id))
                .ToList();
        }

        // Get all features from all collections for cross-collection browsing
        AllAvailableFeatures = AllCollections
            .SelectMany(c => c.Features)
            .GroupBy(f => f.Name)
            .Select(g => g.First())
            .OrderBy(f => f.Name)
            .ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostPullFeatureAsync(Guid id, Guid sourceFeatureId, bool adaptToVersion)
    {
        Collection = _dataService.GetCollection(id);
        if (Collection == null)
        {
            return NotFound();
        }

        try
        {
            await _dataService.CopyFeatureToCollection(sourceFeatureId, Collection.Id, adaptToVersion);
            return RedirectToPage(new { id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error pulling feature: {ex.Message}");
            return Page();
        }
    }
}
