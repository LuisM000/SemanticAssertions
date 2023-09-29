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
        
        var areSimilarFunction = kernel.Skills.GetFunction(
            Plugins.SimilarityPlugin.SimilarityPluginInfo.Name,
            Plugins.SimilarityPlugin.SimilarityPluginInfo.AreSimilar.Name);
        var variables = new ContextVariables
        {
            [Plugins.PluginsInfo.Parameters.Expected] = expected,
            [Plugins.PluginsInfo.Parameters.Actual] = actual
        };
        
        var context = await kernel.RunAsync(variables, areSimilarFunction);
        if (context.ErrorOccurred)
        {
            throw new SemanticAssertionsException("Unexpected Semantic Kernel exception", context.LastException);
        }
        
        return context.Result;    
    }
    
    public virtual async Task<string> CalculateSimilarityAsync(string expected, string actual)
    {
        var kernel = BuildKernel();
        
        var calculateSimilarityFunction = kernel.Skills.GetFunction(
            Plugins.SimilarityPlugin.SimilarityPluginInfo.Name,
            Plugins.SimilarityPlugin.SimilarityPluginInfo.CalculateSimilarity.Name);
        var variables = new ContextVariables
        {
            [Plugins.PluginsInfo.Parameters.Expected] = expected,
            [Plugins.PluginsInfo.Parameters.Actual] = actual
        };
        
        var context = await kernel.RunAsync(variables, calculateSimilarityFunction);
        if (context.ErrorOccurred)
        {
            throw new SemanticAssertionsException("Unexpected Semantic Kernel exception", context.LastException);
        }
        
        return context.Result;
    }

    public async Task<string> AreInSameLanguage(string expected, string actual)
    {
        var kernel = BuildKernel();
        
        var areInSameLanguageFunction = kernel.Skills.GetFunction(
            Plugins.LanguagePlugin.LanguagePluginInfo.Name,
            Plugins.LanguagePlugin.LanguagePluginInfo.AreInSameLanguage.Name);
        var variables = new ContextVariables
        {
            [Plugins.PluginsInfo.Parameters.Expected] = expected,
            [Plugins.PluginsInfo.Parameters.Actual] = actual
        };
        
        var context = await kernel.RunAsync(variables, areInSameLanguageFunction);
        if (context.ErrorOccurred)
        {
            throw new SemanticAssertionsException("Unexpected Semantic Kernel exception", context.LastException);
        }
        
        return context.Result;
    }

    protected static IKernel BuildKernel()
    {
        var kernel = Kernel.Builder
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

        kernel.ImportSemanticSkillFromDirectory(Plugins.PluginsInfo.Directory,
            Plugins.SimilarityPlugin.SimilarityPluginInfo.Name,
            Plugins.LanguagePlugin.LanguagePluginInfo.Name);

        return kernel;
    }
}