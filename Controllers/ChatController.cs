using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly ILogger<ChatController> _logger;

    public ChatController(IChatService chatService, ILogger<ChatController> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }

    /// <summary>
    /// Send a message to the AI chat service
    /// </summary>
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
            {
                return BadRequest(new { error = "Prompt cannot be empty" });
            }

            var response = await _chatService.GetChatResponseAsync(
                request.Prompt,
                request.Context,
                request.ConversationHistory);

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing chat request");
            return StatusCode(500, new { error = "An error occurred processing your request" });
        }
    }

    /// <summary>
    /// Get suggested prompts based on current context
    /// </summary>
    [HttpPost("suggested-prompts")]
    public async Task<IActionResult> GetSuggestedPrompts([FromBody] ChatContext context)
    {
        try
        {
            var prompts = await _chatService.GetSuggestedPromptsAsync(context);
            return Ok(prompts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting suggested prompts");
            return StatusCode(500, new { error = "An error occurred getting suggestions" });
        }
    }

    /// <summary>
    /// Check if chat service is configured
    /// </summary>
    [HttpGet("status")]
    public async Task<IActionResult> GetStatus()
    {
        try
        {
            var isConfigured = await _chatService.IsConfiguredAsync();
            return Ok(new
            {
                isConfigured = isConfigured,
                message = isConfigured
                    ? "Chat service is configured and ready"
                    : "Chat service is not configured. Please add API key to appsettings.json"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking chat service status");
            return StatusCode(500, new { error = "An error occurred checking service status" });
        }
    }
}

/// <summary>
/// Request model for chat messages
/// </summary>
public class ChatRequest
{
    public string Prompt { get; set; } = string.Empty;
    public ChatContext Context { get; set; } = new();
    public List<ChatMessage>? ConversationHistory { get; set; }
}
