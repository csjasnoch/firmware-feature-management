using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages.Features;

public class IndexModel : PageModel
{
    private readonly FirmwareDataService _dataService;

    public IndexModel(FirmwareDataService dataService)
    {
        _dataService = dataService;
    }

    public List<Feature> Features { get; set; } = new();

    public void OnGet()
    {
        Features = _dataService.GetFeatures();
    }
}
