namespace WebApp.Models;

public class Feature
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<ParameterValue> Parameters { get; set; } = new();
    public List<PiccoloCommand> Commands { get; set; } = new();
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
}

public class PiccoloCommand
{
    public Guid Id { get; set; }
    public byte CommandCode { get; set; }
    public string CommandName { get; set; } = string.Empty;
    public List<byte> Payload { get; set; } = new();
    public int ExecutionOrder { get; set; }
    public string Notes { get; set; } = string.Empty;
}
