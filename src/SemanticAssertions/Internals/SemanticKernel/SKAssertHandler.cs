using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using SemanticAssertions.Abstractions;
using SemanticAssertions.Abstractions.Diagnostics;

namespace SemanticAssertions.Internals.SemanticKernel;

// ReSharper disable InconsistentNaming
internal class SKAssertHandler : IAssertHandler
    // ReSharper restore InconsistentNaming
{
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
       
        var result = await RunAsync(kernel, variables, areSimilarFunction);

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
        
        var result = await RunAsync(kernel, variables, calculateSimilarityFunction);

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

        var result = await RunAsync(kernel, variables, areInSameLanguageFunction);

        return result;
    }

    protected static IKernel BuildKernel()
    {
        var kernel = new KernelBuilder()
            .WithAzureChatCompletionService(Configuration.Completion.DeploymentName,
                Configuration.Completion.Endpoint,
                Configuration.Completion.ApiKey,
                alsoAsTextCompletion: true
            )
            .WithAzureTextEmbeddingGenerationService(
                Configuration.Embeddings.DeploymentName,
                Configuration.Embeddings.Endpoint,
                Configuration.Embeddings.ApiKey,
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
        string result;
        try
        {
            var kernelResult = await kernel.RunAsync(variables, function);
            result = kernelResult.ToString();
        }
        catch (Exception ex)
        {
            throw new UnexpectedSemanticAssertionsException("Unexpected Semantic Kernel exception", ex);
        }

        return result;
    }
}