using WebApp.Models;

namespace WebApp.Services;

/// <summary>
/// Interface for AI chat assistance service
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Send a message to the AI and get a response
    /// </summary>
    /// <param name="prompt">The user's message/question</param>
    /// <param name="context">Contextual information about the current page/state</param>
    /// <param name="conversationHistory">Previous messages in the conversation (optional)</param>
    /// <returns>The AI's response message</returns>
    Task<ChatMessage> GetChatResponseAsync(
        string prompt, 
        ChatContext context, 
        List<ChatMessage>? conversationHistory = null);

    /// <summary>
    /// Get suggested prompts based on the current context
    /// </summary>
    /// <param name="context">The current page context</param>
    /// <returns>List of suggested prompt strings</returns>
    Task<List<string>> GetSuggestedPromptsAsync(ChatContext context);

    /// <summary>
    /// Validate that the chat service is properly configured
    /// </summary>
    /// <returns>True if configured and ready to use</returns>
    Task<bool> IsConfiguredAsync();
}
