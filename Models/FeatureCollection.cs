namespace WebApp.Models;

public class FeatureCollection
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // NPI Program link
    public Guid? NpiProgramId { get; set; }
    public NpiProgram? TargetProgram { get; set; }
    
    // Firmware version requirements
    public Guid RequiredLpiVersionId { get; set; }
    public FirmwareVersion? RequiredLpiVersion { get; set; }
    
    public Guid RequiredPiccoloVersionId { get; set; }
    public FirmwareVersion? RequiredPiccoloVersion { get; set; }
    
    // Features in this collection
    public List<Feature> Features { get; set; } = new();
    
    // Metadata
    public CollectionVisibility Visibility { get; set; }
    public string Owner { get; set; } = string.Empty;
    public List<string> SharedWith { get; set; } = new(); // User IDs or team names
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    
    // Version locking
    public bool IsLocked { get; set; }
    public string LockedReason { get; set; } = string.Empty;
    
    // Tags for organization
    public List<string> Tags { get; set; } = new();
}

public enum CollectionVisibility
{
    Private,      // Only owner can see
    Team,         // Shared with specific users/teams
    Organization, // All users in organization
    Public        // Everyone (for released collections)
}
