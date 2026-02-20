namespace WebApp.Models;

/// <summary>
/// Represents contextual information passed to the chat service
/// to provide relevant, page-specific AI assistance
/// </summary>
public class ChatContext
{
    /// <summary>
    /// Current firmware version the user is working with (version string)
    /// </summary>
    public string? CurrentFirmwareVersion { get; set; }

    /// <summary>
    /// ID of the active collection (if on a collection-related page)
    /// </summary>
    public string? ActiveCollectionId { get; set; }

    /// <summary>
    /// Name of the active collection
    /// </summary>
    public string? ActiveCollectionName { get; set; }

    /// <summary>
    /// ID of the current feature being viewed/edited (if applicable)
    /// </summary>
    public string? CurrentFeatureId { get; set; }

    /// <summary>
    /// Name of the current feature
    /// </summary>
    public string? CurrentFeatureName { get; set; }

    /// <summary>
    /// List of available collection names (for pulling/comparing features)
    /// </summary>
    public List<string>? AvailableCollections { get; set; }

    /// <summary>
    /// Recent activity for the user (last accessed collections, features)
    /// </summary>
    public List<RecentActivity>? RecentActivity { get; set; }

    /// <summary>
    /// The page/section the chat is being used from
    /// </summary>
    public ChatPageContext PageContext { get; set; }

    /// <summary>
    /// Additional context-specific data (flexible JSON for page-specific needs)
    /// </summary>
    public Dictionary<string, object>? AdditionalData { get; set; }

    /// <summary>
    /// User's session ID for tracking conversation history
    /// </summary>
    public string? SessionId { get; set; }

    /// <summary>
    /// Current NPI Program filter (if active)
    /// </summary>
    public string? CurrentNpiProgram { get; set; }
}

/// <summary>
/// Enum representing different sections of the application
/// where chat assistance is available
/// </summary>
public enum ChatPageContext
{
    Dashboard,
    CollectionsList,
    CollectionDetails,
    CollectionCreate,
    CollectionCompare,
    FeaturesList,
    FeatureEdit,
    FeatureBytePacket,
    FeatureCompatibility,
    WorkflowDesigner,
    WorkflowVisualize,
    PullFeature
}
