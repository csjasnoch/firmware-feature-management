using System.Text;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Services;

/// <summary>
/// Implementation of AI chat service using OpenAI/Azure OpenAI
/// </summary>
public class ChatService : IChatService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ChatService> _logger;
    private readonly HttpClient _httpClient;

    public ChatService(
        IConfiguration configuration, 
        ILogger<ChatService> logger,
        IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("OpenAI");
    }

    public async Task<ChatMessage> GetChatResponseAsync(
        string prompt, 
        ChatContext context, 
        List<ChatMessage>? conversationHistory = null)
    {
        try
        {
            // Build system message with context
            var systemMessage = BuildSystemMessage(context);
            
            // Build message array for API
            var messages = new List<object> { systemMessage };
            
            // Add conversation history if provided
            if (conversationHistory != null)
            {
                foreach (var msg in conversationHistory.Where(m => m.Role != ChatRole.System))
                {
                    messages.Add(new
                    {
                        role = msg.Role.ToString().ToLower(),
                        content = msg.Content
                    });
                }
            }
            
            // Add current user message
            messages.Add(new
            {
                role = "user",
                content = prompt
            });

            // Get configuration
            var apiKey = _configuration["ChatService:ApiKey"];
            var endpoint = _configuration["ChatService:Endpoint"];
            var model = _configuration["ChatService:Model"] ?? "gpt-4";
            var maxTokens = int.Parse(_configuration["ChatService:MaxTokens"] ?? "1000");

            if (string.IsNullOrEmpty(apiKey))
            {
                _logger.LogWarning("Chat service API key not configured");
                return CreateFallbackResponse(prompt);
            }

            // Call OpenAI API
            var requestBody = new
            {
                model = model,
                messages = messages,
                max_tokens = maxTokens,
                temperature = 0.7
            };

            var request = new HttpRequestMessage(HttpMethod.Post, endpoint ?? "https://api.openai.com/v1/chat/completions");
            request.Headers.Add("Authorization", $"Bearer {apiKey}");
            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody), 
                Encoding.UTF8, 
                "application/json");

            var response = await _httpClient.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError("OpenAI API error: {StatusCode} - {Error}", response.StatusCode, error);
                return CreateFallbackResponse(prompt);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<OpenAIResponse>(responseContent);

            if (result?.Choices == null || result.Choices.Count == 0)
            {
                _logger.LogWarning("OpenAI returned no choices");
                return CreateFallbackResponse(prompt);
            }

            return new ChatMessage
            {
                Role = ChatRole.Assistant,
                Content = result.Choices[0].Message.Content,
                Timestamp = DateTime.UtcNow,
                Metadata = new Dictionary<string, object>
                {
                    ["model"] = model,
                    ["tokens"] = result.Usage?.TotalTokens ?? 0
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling chat service");
            return CreateFallbackResponse(prompt);
        }
    }

    public async Task<List<string>> GetSuggestedPromptsAsync(ChatContext context)
    {
        return context.PageContext switch
        {
            ChatPageContext.Dashboard => new List<string>
            {
                "Show me what I was working on recently",
                $"Which collections use firmware version {context.CurrentFirmwareVersion}?",
                "What features are most commonly reused across collections?",
                "Help me start a new collection for my current project"
            },
            
            ChatPageContext.CollectionsList => new List<string>
            {
                "Which collections were updated this week?",
                $"Show collections for {context.CurrentNpiProgram ?? "my NPI program"}",
                "How do I compare multiple collections?",
                "What's the difference between T0 and T3 collections?"
            },
            
            ChatPageContext.CollectionDetails => new List<string>
            {
                $"Which features from other collections are compatible with {context.ActiveCollectionName}?",
                "How do I pull a feature from another collection?",
                $"What firmware version is {context.ActiveCollectionName} using?",
                "Show me features I can add to this collection"
            },
            
            ChatPageContext.FeaturesList => new List<string>
            {
                "Show me all flow-type features",
                $"Which features are incompatible with {context.CurrentFirmwareVersion}?",
                "Explain the difference between data and flow features",
                "How do I create a new feature?"
            },
            
            ChatPageContext.FeatureEdit => new List<string>
            {
                $"Explain what {context.CurrentFeatureName} does",
                "Which parameters are required for this feature?",
                "How do I add a loop to this feature?",
                "What's the byte packet structure for this feature?"
            },
            
            ChatPageContext.FeatureBytePacket => new List<string>
            {
                "Why do bytes 4-5 differ from expected values?",
                "Which parameter controls byte offset 10?",
                "Explain the packet header structure",
                "How do I debug packet transmission errors?"
            },
            
            ChatPageContext.FeatureCompatibility => new List<string>
            {
                $"How do I adapt {context.CurrentFeatureName} to work with the target version?",
                "What parameter IDs changed between versions?",
                "Which commands were deprecated in the new version?",
                "Can this feature be automatically adapted?"
            },
            
            ChatPageContext.CollectionCompare => new List<string>
            {
                "What are the main differences between these collections?",
                "Which collection has the most complete feature set?",
                "Show me parameter conflicts across collections",
                "How do I merge features from multiple collections?"
            },
            
            ChatPageContext.PullFeature => new List<string>
            {
                "Which features from the source collection are compatible?",
                $"Show me {context.CurrentFeatureName ?? "features"} similar to what I need",
                "What adaptations are needed for incompatible features?",
                "Can I safely pull this feature without changes?"
            },
            
            ChatPageContext.WorkflowDesigner => new List<string>
            {
                "How do I create a loop in the workflow?",
                "Explain the difference between serial and parallel flows",
                "What's the best practice for error handling in workflows?",
                "Show me example workflows for common tasks"
            },
            
            _ => new List<string>
            {
                "How can I help you today?",
                "What would you like to know about firmware features?",
                "Tell me about your current task"
            }
        };
    }

    public async Task<bool> IsConfiguredAsync()
    {
        var apiKey = _configuration["ChatService:ApiKey"];
        return !string.IsNullOrEmpty(apiKey);
    }

    private object BuildSystemMessage(ChatContext context)
    {
        var systemPrompt = new StringBuilder();
        systemPrompt.AppendLine("You are an AI assistant helping users manage firmware feature collections.");
        systemPrompt.AppendLine("Current context:");
        
        if (context.CurrentFirmwareVersion != null)
        {
            systemPrompt.AppendLine($"- Firmware Version: {context.CurrentFirmwareVersion}");
        }
        
        if (!string.IsNullOrEmpty(context.ActiveCollectionName))
        {
            systemPrompt.AppendLine($"- Active Collection: {context.ActiveCollectionName}");
        }
        
        if (!string.IsNullOrEmpty(context.CurrentFeatureName))
        {
            systemPrompt.AppendLine($"- Current Feature: {context.CurrentFeatureName}");
        }
        
        if (!string.IsNullOrEmpty(context.CurrentNpiProgram))
        {
            systemPrompt.AppendLine($"- NPI Program: {context.CurrentNpiProgram}");
        }
        
        systemPrompt.AppendLine($"- Page: {context.PageContext}");
        systemPrompt.AppendLine();
        systemPrompt.AppendLine("Provide helpful, concise answers focused on firmware feature management tasks.");
        systemPrompt.AppendLine("Be specific about parameter IDs, command names, and version compatibility when relevant.");

        return new
        {
            role = "system",
            content = systemPrompt.ToString()
        };
    }

    private ChatMessage CreateFallbackResponse(string prompt)
    {
        return new ChatMessage
        {
            Role = ChatRole.Assistant,
            Content = "I apologize, but I'm unable to process your request at the moment. The chat service may not be configured or is temporarily unavailable. Please check the application configuration or try again later.",
            Timestamp = DateTime.UtcNow,
            Metadata = new Dictionary<string, object>
            {
                ["isFallback"] = true
            }
        };
    }

    // DTO classes for OpenAI API
    private class OpenAIResponse
    {
        public List<Choice>? Choices { get; set; }
        public Usage? Usage { get; set; }
    }

    private class Choice
    {
        public Message Message { get; set; } = new();
    }

    private class Message
    {
        public string Content { get; set; } = string.Empty;
    }

    private class Usage
    {
        public int TotalTokens { get; set; }
    }
}
