using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using SemanticAssertions.Abstractions;

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
        
        //ToDo: review this. Now, throws an exception is fails
        var result = await kernel.RunAsync(variables, areSimilarFunction);
       
        //ToDo: now we can  result.GetValue<>(). This replaces SimpleParser??
        /*if (result)
        {
            throw new SemanticAssertionsException("Unexpected Semantic Kernel exception", context.LastException);
        }*/
        
        return result.ToString();    
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
        
        var result = await kernel.RunAsync(variables, calculateSimilarityFunction);
        //ToDo: review this
        /*if (context.ErrorOccurred)
        {
            throw new SemanticAssertionsException("Unexpected Semantic Kernel exception", context.LastException);
        }*/
        
        return result.ToString();
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
        
        var result = await kernel.RunAsync(variables, areInSameLanguageFunction);
        //ToDo: review this
        /*if (context.ErrorOccurred)
        {
            throw new SemanticAssertionsException("Unexpected Semantic Kernel exception", context.LastException);
        }*/

        return result.ToString();
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
}