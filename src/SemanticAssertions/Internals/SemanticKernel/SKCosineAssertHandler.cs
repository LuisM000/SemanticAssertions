using System.Globalization;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.AI.Embeddings;
using Microsoft.SemanticKernel.AI.Embeddings.VectorOperations;
using SemanticAssertions.Abstractions.Diagnostics;

namespace SemanticAssertions.Internals.SemanticKernel;

// ReSharper disable InconsistentNaming
internal class SKCosineAssertHandler : SKAssertHandler
    // ReSharper restore InconsistentNaming
{
    private static ILogger Logger => Configuration.Current.LoggerFactory.CreateLogger<SKCosineAssertHandler>();
    
    public override async Task<string> CalculateSimilarityAsync(string expected, string actual)
    {
        var kernel = BuildKernel();
        
        var embeddingGenerator = kernel.GetService<ITextEmbeddingGeneration>();

        IList<ReadOnlyMemory<float>> embeddings;
        
        try
        {      
            embeddings = await embeddingGenerator.GenerateEmbeddingsAsync(new List<string>(){expected, actual});
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An error occurred while generating embeddings");
            throw new UnexpectedSemanticAssertionsException("An error occurred while generating embeddings", ex);
        }

        var result = embeddings[0].Span.CosineSimilarity(embeddings[1].Span);

        return result.ToString(CultureInfo.InvariantCulture);
    }
}