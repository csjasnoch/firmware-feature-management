namespace WebApp.Models;

public class ParameterDefinition
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DataType { get; set; } = string.Empty;
    public int MinValue { get; set; }
    public int MaxValue { get; set; }
    public int DefaultValue { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsDeprecated { get; set; }
    public string DeprecationMessage { get; set; } = string.Empty;
    
    // Additional properties for cross-collection operations
    public string Context { get; set; } = string.Empty;
    public int SettingNumber { get; set; }
    public decimal? ValueDecimal { get; set; }
    public string ValueHex { get; set; } = string.Empty;
}
