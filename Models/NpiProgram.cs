namespace WebApp.Models;

public class NpiProgram
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty; // e.g., "Eagan", "Elko", "Edina"
    public string Description { get; set; } = string.Empty;
    public int MajorVersion { get; set; } // Usually matches program (Eagan = v3, Elko = v4)
    public HardwareGeneration HardwareGen { get; set; }
    public List<FirmwareVersion> FirmwareVersions { get; set; } = new();
    public DateTime ProgramStartDate { get; set; }
    public ProgramStatus Status { get; set; }
    public string PmoOwner { get; set; } = string.Empty;
}

public enum HardwareGeneration
{
    Gen1,
    Gen2,
    Gen3,
    Gen4
}

public enum ProgramStatus
{
    Planning,
    Active,
    Released,
    Deprecated
}
