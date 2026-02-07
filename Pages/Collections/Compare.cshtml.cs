using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages.Collections;

public class CompareModel : PageModel
{
    private readonly FirmwareDataService _dataService;

    public CompareModel(FirmwareDataService dataService)
    {
        _dataService = dataService;
    }

    [BindProperty(SupportsGet = true)]
    public Guid? LeftId { get; set; }

    [BindProperty(SupportsGet = true)]
    public Guid? RightId { get; set; }

    public FeatureCollection? LeftCollection { get; set; }
    public FeatureCollection? RightCollection { get; set; }

    public List<SelectListItem> CollectionOptions { get; set; } = new();

    public List<FeatureComparison> Comparisons { get; set; } = new();

    public void OnGet()
    {
        var collections = _dataService.GetCollections();
        CollectionOptions = collections.Select(c => new SelectListItem
        {
            Value = c.Id.ToString(),
            Text = c.Name
        }).ToList();

        if (LeftId.HasValue)
            LeftCollection = _dataService.GetCollection(LeftId.Value);

        if (RightId.HasValue)
            RightCollection = _dataService.GetCollection(RightId.Value);

        if (LeftCollection != null && RightCollection != null)
        {
            CompareCollections();
        }
    }

    private void CompareCollections()
    {
        var allFeatureIds = LeftCollection.Features.Select(f => f.Id)
            .Union(RightCollection.Features.Select(f => f.Id))
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
    public string FeatureName { get; set; }
    public Feature? LeftFeature { get; set; }
    public Feature? RightFeature { get; set; }
    public string Status { get; set; } // Same, Modified, Added, Removed
}
