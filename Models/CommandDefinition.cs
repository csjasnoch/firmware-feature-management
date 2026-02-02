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
}
