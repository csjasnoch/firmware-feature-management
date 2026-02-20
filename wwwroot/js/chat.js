/**
 * Chat Application - AI Assistant Integration
 * Handles chat UI interactions and API communication
 */

class ChatApp {
    constructor(contextData) {
        this.context = contextData;
        this.conversationHistory = [];
        this.isLoading = false;

        // DOM elements
        this.chatPanel = document.getElementById('chatPanel');
        this.chatToggleBtn = document.getElementById('chatToggleBtn');
        this.chatForm = document.getElementById('chatForm');
        this.chatInput = document.getElementById('chatInput');
        this.chatSendBtn = document.getElementById('chatSendBtn');
        this.chatMessages = document.getElementById('chatMessages');
        this.chatStatus = document.getElementById('chatStatus');
        this.suggestedPromptsList = document.getElementById('suggestedPromptsList');
    }

    init() {
        this.setupEventListeners();
        this.loadSuggestedPrompts();
        this.loadConversationHistory();
    }

    setupEventListeners() {
        // Toggle chat panel
        this.chatToggleBtn.addEventListener('click', () => this.togglePanel());

        // Form submission
        this.chatForm.addEventListener('submit', (e) => {
            e.preventDefault();
            this.sendMessage();
        });

        // Auto-resize textarea
        this.chatInput.addEventListener('input', () => {
            this.chatInput.style.height = 'auto';
            this.chatInput.style.height = this.chatInput.scrollHeight + 'px';
        });

        // Enter to send, Shift+Enter for new line
        this.chatInput.addEventListener('keydown', (e) => {
            if (e.key === 'Enter' && !e.shiftKey) {
                e.preventDefault();
                this.sendMessage();
            }
        });
    }

    togglePanel() {
        this.chatPanel.classList.toggle('collapsed');
        const icon = this.chatToggleBtn.querySelector('i');
        
        if (this.chatPanel.classList.contains('collapsed')) {
            icon.classList.remove('bi-chevron-right');
            icon.classList.add('bi-chevron-left');
        } else {
            icon.classList.remove('bi-chevron-left');
            icon.classList.add('bi-chevron-right');
        }
    }

    async loadSuggestedPrompts() {
        try {
            const response = await fetch('/api/chat/suggested-prompts', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(this.context)
            });

            if (response.ok) {
                const prompts = await response.json();
                this.renderSuggestedPrompts(prompts);
            } else {
                this.renderDefaultPrompts();
            }
        } catch (error) {
            console.error('Error loading suggested prompts:', error);
            this.renderDefaultPrompts();
        }
    }

    renderSuggestedPrompts(prompts) {
        this.suggestedPromptsList.innerHTML = '';

        prompts.forEach(prompt => {
            const button = document.createElement('button');
            button.type = 'button';
            button.className = 'suggested-prompt-btn';
            button.textContent = prompt;
            button.addEventListener('click', () => this.useSuggestedPrompt(prompt));
            this.suggestedPromptsList.appendChild(button);
        });
    }

    renderDefaultPrompts() {
        const defaultPrompts = [
            "How can I help you today?",
            "Tell me about your current task",
            "What features are you working on?"
        ];
        this.renderSuggestedPrompts(defaultPrompts);
    }

    useSuggestedPrompt(prompt) {
        this.chatInput.value = prompt;
        this.chatInput.focus();
        this.sendMessage();
    }

    async sendMessage() {
        const message = this.chatInput.value.trim();
        if (!message || this.isLoading) return;

        // Add user message to UI
        this.addMessageToUI('user', message);

        // Clear input
        this.chatInput.value = '';
        this.chatInput.style.height = 'auto';

        // Set loading state
        this.setLoading(true);

        try {
            const response = await fetch('/api/chat/send', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    prompt: message,
                    context: this.context,
                    conversationHistory: this.conversationHistory
                })
            });

            if (response.ok) {
                const data = await response.json();
                this.addMessageToUI('assistant', data.content);
                
                // Save to conversation history
                this.conversationHistory.push({ role: 'user', content: message });
                this.conversationHistory.push({ role: 'assistant', content: data.content });
                this.saveConversationHistory();
            } else {
                const errorText = await response.text();
                this.addMessageToUI('error', 'Sorry, I encountered an error processing your request. Please try again.');
                console.error('Chat API error:', errorText);
            }
        } catch (error) {
            this.addMessageToUI('error', 'Sorry, I\'m unable to connect to the chat service. Please check your configuration.');
            console.error('Chat error:', error);
        } finally {
            this.setLoading(false);
        }
    }

    addMessageToUI(role, content) {
        // Remove welcome message if present
        const welcome = this.chatMessages.querySelector('.chat-welcome');
        if (welcome) {
            welcome.remove();
        }

        const messageDiv = document.createElement('div');
        messageDiv.className = `chat-message chat-message-${role}`;

        const messageContent = document.createElement('div');
        messageContent.className = 'chat-message-content';
        
        if (role === 'user') {
            messageContent.innerHTML = `<strong>You:</strong> ${this.escapeHtml(content)}`;
        } else if (role === 'assistant') {
            messageContent.innerHTML = `<strong>Assistant:</strong> ${this.formatMarkdown(content)}`;
        } else if (role === 'error') {
            messageContent.innerHTML = `<i class="bi bi-exclamation-triangle-fill text-danger"></i> ${this.escapeHtml(content)}`;
        }

        const timestamp = document.createElement('div');
        timestamp.className = 'chat-message-timestamp';
        timestamp.textContent = new Date().toLocaleTimeString();

        messageDiv.appendChild(messageContent);
        messageDiv.appendChild(timestamp);
        this.chatMessages.appendChild(messageDiv);

        // Scroll to bottom
        this.chatMessages.scrollTop = this.chatMessages.scrollHeight;
    }

    setLoading(isLoading) {
        this.isLoading = isLoading;
        this.chatSendBtn.disabled = isLoading;
        this.chatInput.disabled = isLoading;

        if (isLoading) {
            this.chatStatus.innerHTML = '<i class="spinner-border spinner-border-sm" role="status"></i> Assistant is thinking...';
        } else {
            this.chatStatus.innerHTML = '';
        }
    }

    saveConversationHistory() {
        try {
            sessionStorage.setItem(
                `chat_history_${this.context.SessionId}`,
                JSON.stringify(this.conversationHistory)
            );
        } catch (error) {
            console.warn('Could not save conversation history:', error);
        }
    }

    loadConversationHistory() {
        try {
            const saved = sessionStorage.getItem(`chat_history_${this.context.SessionId}`);
            if (saved) {
                this.conversationHistory = JSON.parse(saved);
                
                // Restore messages to UI
                this.conversationHistory.forEach(msg => {
                    this.addMessageToUI(msg.role, msg.content);
                });
            }
        } catch (error) {
            console.warn('Could not load conversation history:', error);
        }
    }

    escapeHtml(text) {
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }

    formatMarkdown(text) {
        // Simple markdown formatting (bold, italic, code)
        let formatted = this.escapeHtml(text);
        
        // Bold: **text**
        formatted = formatted.replace(/\*\*(.+?)\*\*/g, '<strong>$1</strong>');
        
        // Italic: *text*
        formatted = formatted.replace(/\*(.+?)\*/g, '<em>$1</em>');
        
        // Inline code: `code`
        formatted = formatted.replace(/`(.+?)`/g, '<code>$1</code>');
        
        // Line breaks
        formatted = formatted.replace(/\n/g, '<br>');
        
        return formatted;
    }
}

// Export for global use
window.ChatApp = ChatApp;
