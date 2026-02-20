namespace WebApp.Models;

/// <summary>
/// Represents a recent user activity for tracking work context
/// </summary>
public class RecentActivity
{
    /// <summary>
    /// Type of activity
    /// </summary>
    public ActivityType Type { get; set; }

    /// <summary>
    /// When the activity occurred
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// ID of the related collection (if applicable)
    /// </summary>
    public int? CollectionId { get; set; }

    /// <summary>
    /// Name of the related collection
    /// </summary>
    public string? CollectionName { get; set; }

    /// <summary>
    /// ID of the related feature (if applicable)
    /// </summary>
    public int? FeatureId { get; set; }

    /// <summary>
    /// Name of the related feature
    /// </summary>
    public string? FeatureName { get; set; }

    /// <summary>
    /// Description of the activity
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Additional context data
    /// </summary>
    public Dictionary<string, string>? ContextData { get; set; }
}

/// <summary>
/// Types of user activities to track
/// </summary>
public enum ActivityType
{
    ViewedCollection,
    EditedCollection,
    CreatedCollection,
    DeletedCollection,
    ViewedFeature,
    EditedFeature,
    CreatedFeature,
    DeletedFeature,
    ComparedCollections,
    ViewedBytePacket,
    DesignedWorkflow,
    PulledFeature,
    AdaptedFeature,
    RunCompatibilityAnalysis
}
