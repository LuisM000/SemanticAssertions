using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI.AzureSdk;
using SemanticAssertions.Abstractions;
using SemanticAssertions.Abstractions.Diagnostics;

namespace SemanticAssertions.Internals.SemanticKernel;
    
// ReSharper disable InconsistentNaming
internal class SKFunctionCallingParserHandler : IParserHandler
    // ReSharper restore InconsistentNaming
{
    private static ILogger Logger => Configuration.Current.LoggerFactory.CreateLogger<SKCosineAssertHandler>();

    public async Task<bool> ParseBoolAsync(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new UnexpectedSemanticAssertionsException($"Failed to parse '{value}' as a boolean");
        }

        var kernel = BuildKernel();

        var chatCompletion = kernel.GetService<IChatCompletion>();
        var chatHistory = chatCompletion.CreateNewChat();

        var requestSettings = new OpenAIRequestSettings()
        {
            Functions = new List<OpenAIFunction>(1)
            {
                new()
                {
                    Description = "Parse the value to true, false or null",
                    FunctionName = "parse_bool",
                    Parameters = new List<OpenAIFunctionParameter>(1)
                    {
                        new()
                        {
                            Description = "Bool value. If it's not clear, don't include it.",
                            IsRequired = false,
                            Name = "ParsedValue",
                            Type = "boolean"
                        }
                    }
                }
            },
            FunctionCall = "parse_bool"
        };

        chatHistory.AddSystemMessage("You are an algorithm to parse the received text to true or false or null");
        chatHistory.AddUserMessage("Parses the following text, if not something like true or false, returns null");
        chatHistory.AddUserMessage(value);

        IReadOnlyList<IChatResult> chatResults;
        try
        {
            chatResults = await chatCompletion.GetChatCompletionsAsync(chatHistory, requestSettings);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while executing the chat completions");
            throw new UnexpectedSemanticAssertionsException($"An error occurred while executing the chat completions. Failed to parse '{value}' as a boolean", ex);
        }
           
        var functionResponse = chatResults.Select(c => c.GetFunctionResponse()).FirstOrDefault(f => f?.FunctionName == "parse_bool");
        if (functionResponse!= null && functionResponse.Parameters.TryGetValue("ParsedValue", out var rawValue) && 
            rawValue is JsonElement
            {
                ValueKind: JsonValueKind.False or JsonValueKind.True
            } jsonElementValue)
        {
            return jsonElementValue.GetBoolean();
        }


        throw new UnexpectedSemanticAssertionsException($"Failed to parse '{value}' as a boolean");
    }

    public async Task<double> ParseDoubleAsync(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new UnexpectedSemanticAssertionsException($"Failed to parse '{value}' as a double");
        }

        var kernel = BuildKernel();

        var chatCompletion = kernel.GetService<IChatCompletion>();
        var chatHistory = chatCompletion.CreateNewChat();

        var requestSettings = new OpenAIRequestSettings()
        {
            Functions = new List<OpenAIFunction>(1)
            {
                new()
                {
                    Description = "Parse the value to double or null",
                    FunctionName = "parse_double",
                    Parameters = new List<OpenAIFunctionParameter>(1)
                    {
                        new()
                        {
                            Description = "Double value. If it's not clear, don't include it.",
                            IsRequired = false,
                            Name = "ParsedDouble",
                            Type = "number"
                        }
                    }
                }
            },
            FunctionCall = "parse_double"
        };

        chatHistory.AddSystemMessage("You are an algorithm to parse the received text to double or null");
        chatHistory.AddUserMessage($@"Parses the following text, if not something like a number, returns null. 
The decimal separator can be point or comma. Ignore the thousands separator.

[EXAMPLES]
18365,90 => 18365.90
18365.90 => 18365.90
18 365.90 => 18365.90
18,365.90 => 18365.90
18 365,90 => 18365.90
18.365,90 => 18365.90
[END EXAMPLES]

The value is {value}");

        IReadOnlyList<IChatResult> chatResults;
        try
        {
            chatResults = await chatCompletion.GetChatCompletionsAsync(chatHistory, requestSettings);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while executing the chat completions");
            throw new UnexpectedSemanticAssertionsException($"An error occurred while executing the chat completions. Failed to parse '{value}' as a double", ex);
        }

        var functionResponse = chatResults.Select(c => c.GetFunctionResponse()).FirstOrDefault(f => f?.FunctionName == "parse_double");
        if (functionResponse != null && functionResponse.Parameters.TryGetValue("ParsedDouble", out var rawValue) &&
            rawValue is JsonElement
            {
                ValueKind: JsonValueKind.Number
            } jsonElementValue)
        {
            return jsonElementValue.GetDouble();
        }


        throw new UnexpectedSemanticAssertionsException($"Failed to parse '{value}' as a double");
    }

    private static IKernel BuildKernel()
    {
        var kernel = new KernelBuilder()
            .WithLoggerFactory(Configuration.Current.LoggerFactory)
            .WithAzureChatCompletionService(
                Configuration.Current.Completion.DeploymentName,
                Configuration.Current.Completion.Endpoint,
                Configuration.Current.Completion.ApiKey,
                alsoAsTextCompletion: true
            )
            .Build();

        return kernel;
    }
}