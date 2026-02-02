namespace WebApp.Models;

public class FirmwareVersion
{
    public Guid Id { get; set; }
    public Guid NpiProgramId { get; set; }
    public NpiProgram? NpiProgram { get; set; }
    public string Version { get; set; } = string.Empty; // Semantic version (e.g., "3.2.1")
    public FirmwareReleaseStage Stage { get; set; }
    public string FilePath { get; set; } = string.Empty; // Path to XML file
    public DateTime ReleasedAt { get; set; }
    public string ReleasedBy { get; set; } = string.Empty;
    public List<string> ChangeLog { get; set; } = new();
    
    // Parsed metadata
    public List<ParameterDefinition> Parameters { get; set; } = new(); // For LPI
    public List<CommandDefinition> Commands { get; set; } = new();     // For PICCOLO
}

public enum FirmwareReleaseStage
{
    T0_Test,        // Experimental
    T1_Alpha,       // Early testing
    T2_Beta,        // Pre-release
    T3_Released     // Locked and production-ready
}
