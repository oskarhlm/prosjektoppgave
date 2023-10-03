using Azure.AI.OpenAI;
using Client.GeonorgeFunctions;

namespace Client.GPT4;

public class ChatGptClient
{
    private readonly IConfiguration _configuration;

    public ChatGptClient(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<ChatChoice?> MakePrompt(string prompt)
    {
        OpenAIClient client = new(_configuration["OpenAI:ApiKey"]);
        // string model = "gpt-4";
        string model = "gpt-3.5-turbo";
    
        ChatCompletionsOptions chatCompletionsOptions = new()
        {
            MaxTokens = 1024,
            FunctionCall = FunctionDefinition.Auto,
            Functions =
            {
                GetGeonorgeDatasetFunction.GetFunctionDefinition()
            }
        };
        
        chatCompletionsOptions.Messages
            .Add(new(ChatRole.User, prompt));

        ChatCompletions response = await client.GetChatCompletionsAsync(model, chatCompletionsOptions);
        var responseChoice = response.Choices.First();

        if (responseChoice.FinishReason != CompletionsFinishReason.FunctionCall)
        {
            return null;
        } 
    
        return responseChoice;
    }
}