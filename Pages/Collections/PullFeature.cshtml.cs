using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages.Collections;

/// <summary>
/// Pull features between collections page
/// NOTE: Simplified implementation - full compatibility requires model updates
/// </summary>
public class PullFeatureModel : PageModel
{
    private readonly FirmwareDataService _firmwareService;
    private readonly ContextService _contextService;

    public PullFeatureModel(FirmwareDataService firmwareService, ContextService contextService)
    {
        _firmwareService = firmwareService;
        _contextService = contextService;
    }

    public List<FeatureCollection> AvailableCollections { get; set; } = new();
    public FeatureCollection? SourceCollection { get; set; }
    public FeatureCollection? TargetCollection { get; set; }
    public List<Feature> SourceFeatures { get; set; } = new();
    public ChatContext PullFeatureChatContext { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public Guid? SourceCollectionId { get; set; }

    [BindProperty(SupportsGet = true)]
    public Guid? TargetCollectionId { get; set; }

    public async Task OnGetAsync()
    {
        await LoadBaseDataAsync();
        SetupChatContext();
    }

    public async Task<IActionResult> OnPostPullAsync(
        Guid featureId, 
        Guid sourceCollectionId, 
        Guid targetCollectionId, 
        bool adaptIfNeeded)
    {
        await LoadBaseDataAsync();
        
        var feature = SourceFeatures.FirstOrDefault(f => f.Id == featureId);
        var targetCollection = AvailableCollections.FirstOrDefault(c => c.Id == targetCollectionId);

        if (feature == null || targetCollection == null)
        {
            TempData["ErrorMessage"] = "Invalid feature or target collection";
            return RedirectToPage(new { sourceCollectionId, targetCollectionId });
        }

        try
        {
            // Create a copy of the feature for the target collection
            var pulledFeature = new Feature
            {
                Id = Guid.NewGuid(),
                Name = feature.Name,
                Description = feature.Description,
                Category = feature.Category,
                Owner = feature.Owner,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
                Parameters = feature.Parameters.Select(p => new ParameterValue
                {
                    Id = Guid.NewGuid(),
                    ParameterId = p.ParameterId,
                    ParameterName = p.ParameterName,
                    Value = p.Value,
                    Notes = p.Notes
                }).ToList(),
                Commands = feature.Commands.Select(c => new PiccoloCommand
                {
                    Id = Guid.NewGuid(),
                    CommandCode = c.CommandCode,
                    CommandName = c.CommandName,
                    Payload = c.Payload.ToList(),
                    ExecutionOrder = c.ExecutionOrder
                }).ToList()
            };

            // Add to target collection
            targetCollection.Features.Add(pulledFeature);
            targetCollection.ModifiedAt = DateTime.UtcNow;
            
            // In a real implementation, would save via service
            // await _firmwareService.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Feature '{feature.Name}' has been successfully pulled into '{targetCollection.Name}'!";
            return RedirectToPage("/Collections/Index");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error pulling feature: {ex.Message}";
            return RedirectToPage(new { sourceCollectionId, targetCollectionId });
        }
    }

    public string GetCompatibilityStatus(Feature feature)
    {
        if (TargetCollection == null)
        {
            return "unknown";
        }

        // Simplified compatibility check
        // In a full implementation, would compare version requirements
        return "compatible";
    }

    public string GetCompatibilityDetails(Feature feature)
    {
        if (TargetCollection?.RequiredLpiVersion == null || SourceCollection?.RequiredLpiVersion == null)
        {
            return "Version information not available";
        }

        var sourceVersion = SourceCollection.RequiredLpiVersion.Version;
        var targetVersion = TargetCollection.RequiredLpiVersion.Version;

        if (sourceVersion == targetVersion)
        {
            return "Same version - fully compatible";
        }

        return $"May need adaptation (source: {sourceVersion}, target: {targetVersion})";
    }

    private async Task LoadBaseDataAsync()
    {
        AvailableCollections = await _firmwareService.GetAllCollectionsAsync();

        if (SourceCollectionId.HasValue)
        {
            SourceCollection = AvailableCollections.FirstOrDefault(c => c.Id == SourceCollectionId.Value);
            SourceFeatures = SourceCollection?.Features ?? new List<Feature>();
        }

        if (TargetCollectionId.HasValue)
        {
            TargetCollection = AvailableCollections.FirstOrDefault(c => c.Id == TargetCollectionId.Value);
        }
    }

    private void SetupChatContext()
    {
        var currentContext = _contextService.GetOrCreateDefaultContext();
        
        PullFeatureChatContext = new ChatContext
        {
            PageContext = ChatPageContext.PullFeature,
            ActiveCollectionId = TargetCollectionId?.ToString(),
            ActiveCollectionName = TargetCollection?.Name,
            CurrentFirmwareVersion = TargetCollection?.RequiredLpiVersion?.Version ?? currentContext.LpiVersionNumber ?? "Unknown",
            AvailableCollections = AvailableCollections.Select(c => c.Name).ToList(),
            CurrentNpiProgram = currentContext.NpiProgramName,
            SessionId = HttpContext.Session.Id,
            AdditionalData = new Dictionary<string, object>
            {
                ["SourceCollectionId"] = SourceCollectionId?.ToString() ?? "None",
                ["SourceCollectionName"] = SourceCollection?.Name ?? "None",
                ["AvailableFeatures"] = SourceFeatures.Count
            }
        };
    }
}

