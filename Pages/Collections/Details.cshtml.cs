using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages.Collections;

public class DetailsModel : PageModel
{
    private readonly FirmwareDataService _dataService;

    public DetailsModel(FirmwareDataService dataService)
    {
        _dataService = dataService;
    }

    public FeatureCollection? Collection { get; set; }

    public IActionResult OnGet(Guid id)
    {
        Collection = _dataService.GetCollection(id);
        if (Collection == null)
        {
            return NotFound();
        }
        return Page();
    }
}
