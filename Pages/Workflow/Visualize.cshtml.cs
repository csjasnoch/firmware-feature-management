using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages.Workflow
{
    public class VisualizeModel : PageModel
    {
        private readonly FirmwareDataService _service;

        public OperationalFlow Flow { get; set; } = new();
        public FeatureCollection? Collection { get; set; }
        public Feature? Feature { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }

        public VisualizeModel(FirmwareDataService service)
        {
            _service = service;
        }

        public IActionResult OnGet(Guid? id)
        {
            if (id.HasValue)
            {
                // Get the feature and generate a flow from it
                Feature = _service.GetFeature(id.Value);
                if (Feature == null)
                {
                    return NotFound();
                }
                
                Flow = GenerateFlowFromFeature(Feature);
                
                // Try to find the collection containing this feature
                Collection = _service.GetCollections().FirstOrDefault(c => c.Features.Any(f => f.Id == id.Value));
            }
            else
            {
                // Fallback to DFU example if no ID provided
                Flow = _service.GetDfuFlow();
                Collection = _service.GetCollection(Flow.CollectionId);
            }
            
            return Page();
        }
        
        private OperationalFlow GenerateFlowFromFeature(Feature feature)
        {
            var flow = new OperationalFlow
            {
                Id = Guid.NewGuid(),
                Name = $"{feature.Name} Workflow",
                Description = $"PICCOLO command sequence for {feature.Name}",
                FeatureId = feature.Id,
                CollectionId = Collection?.Id ?? Guid.Empty,
                Operations = new List<FlowOperation>()
            };
            
            // Create a single operation containing all commands
            var operation = new FlowOperation
            {
                Id = Guid.NewGuid(),
                Name = $"{feature.Name} Execution",
                Type = "Sequential",
                Steps = new List<FlowStep>()
            };
            
            // Generate steps from feature's PICCOLO commands
            int stepNumber = 1;
            foreach (var cmd in feature.Commands.OrderBy(c => c.ExecutionOrder))
            {
                operation.Steps.Add(new FlowStep
                {
                    Id = Guid.NewGuid(),
                    StepNumber = $"{stepNumber}",
                    Title = cmd.CommandName,
                    Description = cmd.Notes ?? "",
                    Type = "Command",
                    Attributes = new Dictionary<string, string>
                    {
                        { "CommandCode", $"0x{cmd.CommandCode:X2}" },
                        { "Payload", string.Join(" ", cmd.Payload.Select(b => $"0x{b:X2}")) },
                        { "ExpectedDurationMs", "50" }
                    }
                });
                stepNumber++;
            }
            
            flow.Operations.Add(operation);
            return flow;
        }
    }
}
