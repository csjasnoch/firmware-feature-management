using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages.Collections;

public class CompareModel : PageModel
{
    private readonly FirmwareDataService _dataService;
    private readonly ContextService _contextService;

    public CompareModel(FirmwareDataService dataService, ContextService contextService)
    {
        _dataService = dataService;
        _contextService = contextService;
    }

    [BindProperty(SupportsGet = true)]
    public List<Guid> CollectionIds { get; set; } = new();

    // Legacy 2-collection support
    [BindProperty(SupportsGet = true)]
    public Guid? LeftId { get; set; }

    [BindProperty(SupportsGet = true)]
    public Guid? RightId { get; set; }

    public FeatureCollection? LeftCollection { get; set; }
    public FeatureCollection? RightCollection { get; set; }
    public List<FeatureCollection> SelectedCollections { get; set; } = new();
    public List<FeatureCollection> AllCollections { get; set; } = new();

    public List<SelectListItem> CollectionOptions { get; set; } = new();

    public List<FeatureComparison> Comparisons { get; set; } = new();
    public List<Feature> CommonFeatures { get; set; } = new();
    public List<Feature> UniqueFeatures { get; set; } = new();
    
    [BindProperty]
    public List<Guid> SelectedFeatureIds { get; set; } = new();
    
    [BindProperty]
    public MergeStrategy MergeStrategy { get; set; } = MergeStrategy.Union;

    public void OnGet()
    {
        AllCollections = _dataService.GetCollections();
        CollectionOptions = AllCollections.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = $"{c.Name} ({c.TargetProgram?.Name})"
        }).ToList();

        // Support multi-collection comparison
        if (CollectionIds.Any())
        {
            SelectedCollections = AllCollections
                .Where(c => CollectionIds.Contains(c.Id))
                .ToList();
            
            if (SelectedCollections.Count >= 2)
            {
                CompareMultipleCollections();
            }
        }
        // Legacy 2-collection support
        else if (LeftId.HasValue && RightId.HasValue)
        {
            LeftCollection = _dataService.GetCollection(LeftId.Value);
            RightCollection = _dataService.GetCollection(RightId.Value);

            if (LeftCollection != null && RightCollection != null)
            {
                CompareCollections();
            }
        }
    }

    public async Task<IActionResult> OnPostMergeFeaturesAsync()
    {
        if (!SelectedFeatureIds.Any())
        {
            ModelState.AddModelError("", "Please select at least one feature to merge");
            return Page();
        }

        try
        {
            var mergedFeature = await _dataService.MergeFeatures(SelectedFeatureIds, MergeStrategy);
            TempData["SuccessMessage"] = $"Successfully merged {SelectedFeatureIds.Count} features into '{mergedFeature.Name}'";
            return RedirectToPage("/Features/Edit", new { id = mergedFeature.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error merging features: {ex.Message}");
            return Page();
        }
    }

    private void CompareMultipleCollections()
    {
        // Find common features (appear in all collections)
        var firstCollection = SelectedCollections.First();
        CommonFeatures = firstCollection.Features
            .Where(f => SelectedCollections.All(c => c.Features.Any(cf => cf.Name == f.Name)))
            .ToList();

        // Find unique features (appear in only one collection)
        foreach (var collection in SelectedCollections)
        {
            var uniqueInCollection = collection.Features
                .Where(f => SelectedCollections.Count(c => c.Features.Any(cf => cf.Name == f.Name)) == 1)
                .ToList();
            UniqueFeatures.AddRange(uniqueInCollection);
        }

        // Build comparison matrix for common features
        foreach (var commonFeature in CommonFeatures)
        {
            var comp = new FeatureComparison
            {
                FeatureId = commonFeature.Id,
                FeatureName = commonFeature.Name,
                Status = "Common"
            };
            Comparisons.Add(comp);
        }
    }

    private void CompareCollections()
    {
        var allFeatureIds = LeftCollection!.Features.Select(f => f.Id)
            .Union(RightCollection!.Features.Select(f => f.Id))
            .Distinct();

        foreach (var id in allFeatureIds)
        {
            var left = LeftCollection.Features.FirstOrDefault(f => f.Id == id);
            var right = RightCollection.Features.FirstOrDefault(f => f.Id == id);

            var comp = new FeatureComparison
            {
                FeatureId = id,
                FeatureName = left?.Name ?? right?.Name ?? "Unknown",
                LeftFeature = left,
                RightFeature = right
            };

            // Detect changes
            if (left == null) comp.Status = "Added";
            else if (right == null) comp.Status = "Removed";
            else
            {
                // Simple equality check (can be improved to check parameters)
                var leftParams = string.Join(",", left.Parameters.OrderBy(p => p.Id).Select(p => p.Value));
                var rightParams = string.Join(",", right.Parameters.OrderBy(p => p.Id).Select(p => p.Value));
                
                if (leftParams != rightParams) comp.Status = "Modified";
                else comp.Status = "Same";
            }
            
            Comparisons.Add(comp);
        }
    }
}

public class FeatureComparison
{
    public Guid FeatureId { get; set; }
    public string FeatureName { get; set; } = string.Empty;
    public Feature? LeftFeature { get; set; }
    public Feature? RightFeature { get; set; }
    public string Status { get; set; } = string.Empty; // Same, Modified, Added, Removed
}
