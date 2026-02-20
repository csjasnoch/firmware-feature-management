namespace WebApp.Models;

public class Feature
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsComposite { get; set; } = false;
    public List<ParameterValue> Parameters { get; set; } = new();
    public List<PiccoloCommand> Commands { get; set; } = new();
    public List<string> RequiredLpiParams { get; set; } = new();
    public List<string> RequiredPiccoloCommands { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public string Owner { get; set; } = string.Empty;
}

public class ParameterValue
{
    public Guid Id { get; set; }
    public int ParameterId { get; set; } // References ParameterDefinition
    public string ParameterName { get; set; } = string.Empty;
    public int Value { get; set; }
    public string Notes { get; set; } = string.Empty;
    
    // Additional properties for cross-collection operations
    public string Context { get; set; } = string.Empty;
    public int SettingNumber { get; set; }
    public decimal? ValueDecimal { get; set; }
    public string ValueHex { get; set; } = string.Empty;
}

public class PiccoloCommand
{
    public Guid Id { get; set; }
    public byte CommandCode { get; set; }
    public string CommandName { get; set; } = string.Empty;
    public List<byte> Payload { get; set; } = new();
    public int ExecutionOrder { get; set; }
    public string Notes { get; set; } = string.Empty;
    
    // Additional properties for cross-collection operations
    public int CommandId { get; set; }
    public int Sequence { get; set; }
    public string ServiceName { get; set; } = string.Empty;
}
