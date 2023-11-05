using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using SemanticAssertions.Abstractions;
using SemanticAssertions.Abstractions.Diagnostics;

namespace SemanticAssertions.Internals.SemanticKernel;

// ReSharper disable InconsistentNaming
internal class SKAssertHandler : IAssertHandler
    // ReSharper restore InconsistentNaming
{
    private static ILogger Logger => Configuration.Current.LoggerFactory.CreateLogger<SKAssertHandler>();

    public async Task<string> AreSimilar(string expected, string actual)
    {
        var kernel = BuildKernel();
        
        var areSimilarFunction = kernel.Functions.GetFunction(
            Plugins.SimilarityPlugin.SimilarityPluginInfo.Name,
            Plugins.SimilarityPlugin.SimilarityPluginInfo.AreSimilar.Name);
        var variables = new ContextVariables
        {
            [Plugins.PluginsInfo.Parameters.Expected] = expected,
            [Plugins.PluginsInfo.Parameters.Actual] = actual
        };
       
        var result = await RunAsync(kernel, variables, areSimilarFunction).ConfigureAwait(false);

        return result;   
    }
    
    public virtual async Task<string> CalculateSimilarityAsync(string expected, string actual)
    {
        var kernel = BuildKernel();
        
        var calculateSimilarityFunction = kernel.Functions.GetFunction(
            Plugins.SimilarityPlugin.SimilarityPluginInfo.Name,
            Plugins.SimilarityPlugin.SimilarityPluginInfo.CalculateSimilarity.Name);
        var variables = new ContextVariables
        {
            [Plugins.PluginsInfo.Parameters.Expected] = expected,
            [Plugins.PluginsInfo.Parameters.Actual] = actual
        };
        
        var result = await RunAsync(kernel, variables, calculateSimilarityFunction).ConfigureAwait(false);

        return result;
    }

    public async Task<string> AreInSameLanguage(string expected, string actual)
    {
        var kernel = BuildKernel();
        
        var areInSameLanguageFunction = kernel.Functions.GetFunction(
            Plugins.LanguagePlugin.LanguagePluginInfo.Name,
            Plugins.LanguagePlugin.LanguagePluginInfo.AreInSameLanguage.Name);
        var variables = new ContextVariables
        {
            [Plugins.PluginsInfo.Parameters.Expected] = expected,
            [Plugins.PluginsInfo.Parameters.Actual] = actual
        };

        var result = await RunAsync(kernel, variables, areInSameLanguageFunction).ConfigureAwait(false);

        return result;
    }

    protected static IKernel BuildKernel()
    {
        var kernel = new KernelBuilder()
            .WithLoggerFactory(Configuration.Current.LoggerFactory)
            .WithAzureChatCompletionService(
                Configuration.Current.Completion.DeploymentName,
                Configuration.Current.Completion.Endpoint,
                Configuration.Current.Completion.ApiKey,
                alsoAsTextCompletion: true
            )
            .WithAzureTextEmbeddingGenerationService(
                Configuration.Current.Embeddings.DeploymentName,
                Configuration.Current.Embeddings.Endpoint,
                Configuration.Current.Embeddings.ApiKey,
                serviceId: null
            )
            .Build();

        kernel.ImportSemanticFunctionsFromDirectory(Plugins.PluginsInfo.Directory,
            Plugins.SimilarityPlugin.SimilarityPluginInfo.Name,
            Plugins.LanguagePlugin.LanguagePluginInfo.Name);

        return kernel;
    }
    
    private static async Task<string> RunAsync(IKernel kernel, ContextVariables variables, ISKFunction function)
    {
        try
        {
            var kernelResult = await kernel.RunAsync(variables, function).ConfigureAwait(false);
            return kernelResult.ToString();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while executing the Kernel");
            throw new UnexpectedSemanticAssertionsException("An error occurred while executing the Kernel", ex);
        }
    }
}