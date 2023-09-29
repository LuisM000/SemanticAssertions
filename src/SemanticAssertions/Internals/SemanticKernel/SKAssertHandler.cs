using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;

namespace SemanticAssertions.Internals.SemanticKernel;

// ReSharper disable InconsistentNaming
internal class SKAssertHandler : IAssertHandler
    // ReSharper restore InconsistentNaming
{
    public virtual async Task<string> CalculateSimilarityAsync(string expected, string actual)
    {
        var kernel = BuildKernel();
        
        var areSimilarFunction = kernel.Skills.GetFunction(
            Plugins.SimilarityPlugin.SimilarityPluginInfo.Name,
            Plugins.SimilarityPlugin.SimilarityPluginInfo.AreSimilar.Name);
        var variables = new ContextVariables
        {
            [Plugins.SimilarityPlugin.SimilarityPluginInfo.AreSimilar.Parameters.Expected] = expected,
            [Plugins.SimilarityPlugin.SimilarityPluginInfo.AreSimilar.Parameters.Actual] = actual
        };
        
        var context = await kernel.RunAsync(variables, areSimilarFunction);

        //ToDo: check context exceptions????
        // The exception raised should be indicated to 
        // the SemanticAssertions users that the exception is
        // on library, not in test...
        
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
            [Plugins.LanguagePlugin.LanguagePluginInfo.AreInSameLanguage.Parameters.Expected] = expected,
            [Plugins.LanguagePlugin.LanguagePluginInfo.AreInSameLanguage.Parameters.Actual] = actual
        };
        
        var context = await kernel.RunAsync(variables, areInSameLanguageFunction);

        //ToDo: check context exceptions????
        // The exception raised should be indicated to 
        // the SemanticAssertions users that the exception is
        // on library, not in test...
        
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