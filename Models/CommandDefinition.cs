namespace WebApp.Models;

public class CommandDefinition
{
    public byte CommandCode { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<ParameterDefinition> Parameters { get; set; } = new();
    public byte ExpectedResponseCode { get; set; }
    public bool IsDeprecated { get; set; }
    public string DeprecationMessage { get; set; } = string.Empty;
    
    // Additional properties for cross-collection operations
    public int CommandId { get; set; }
    public int Sequence { get; set; }
    public string ServiceName { get; set; } = string.Empty;
}
