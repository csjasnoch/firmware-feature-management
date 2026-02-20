namespace WebApp.Models;

/// <summary>
/// Represents a single message in a chat conversation
/// </summary>
public class ChatMessage
{
    /// <summary>
    /// Unique identifier for the message
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// The role of the message sender (User or Assistant)
    /// </summary>
    public ChatRole Role { get; set; }

    /// <summary>
    /// The message content/text
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// When the message was created
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Optional metadata about the message (e.g., token count, processing time)
    /// </summary>
    public Dictionary<string, object>? Metadata { get; set; }
}

/// <summary>
/// Represents the role of a chat message sender
/// </summary>
public enum ChatRole
{
    /// <summary>
    /// Message from the user
    /// </summary>
    User,

    /// <summary>
    /// Message from the AI assistant
    /// </summary>
    Assistant,

    /// <summary>
    /// System message (for context/instructions, not displayed in UI)
    /// </summary>
    System
}
