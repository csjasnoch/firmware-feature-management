using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages.Features;

public class IndexModel : PageModel
{
    private readonly FirmwareDataService _dataService;
    private readonly ContextService _contextService;

    public IndexModel(FirmwareDataService dataService, ContextService contextService)
    {
        _dataService = dataService;
        _contextService = contextService;
    }

    public List<Feature> Features { get; set; } = new();
    public ChatContext FeaturesChatContext { get; set; } = new();

    public void OnGet()
    {
        Features = _dataService.GetFeatures();
        
        var currentContext = _contextService.GetOrCreateDefaultContext();
        var currentVersion = _dataService.GetFirmwareVersionById(currentContext.LpiVersionId);
        
        // Setup chat context
        FeaturesChatContext = new ChatContext
        {
            PageContext = ChatPageContext.FeaturesList,
            CurrentFirmwareVersion = currentVersion?.Version ?? currentContext.LpiVersionNumber,
            CurrentNpiProgram = currentContext.NpiProgramName,
            SessionId = HttpContext.Session.Id,
            AdditionalData = new Dictionary<string, object>
            {
                ["TotalFeatures"] = Features.Count,
                ["FeatureCategories"] = Features.GroupBy(f => f.Category).Select(g => new { Category = g.Key, Count = g.Count() }).ToList()
            }
        };
    }
}
