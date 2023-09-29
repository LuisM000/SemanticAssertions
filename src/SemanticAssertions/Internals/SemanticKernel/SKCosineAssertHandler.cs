using System.Globalization;
using Microsoft.SemanticKernel.AI.Embeddings;
using Microsoft.SemanticKernel.AI.Embeddings.VectorOperations;

namespace SemanticAssertions.Internals.SemanticKernel;

// ReSharper disable InconsistentNaming
internal class SKCosineAssertHandler : SKAssertHandler
    // ReSharper restore InconsistentNaming
{
    public override async Task<string> CalculateSimilarityAsync(string expected, string actual)
    {
        var kernel = BuildKernel();
        
        var embeddingGenerator = kernel.GetService<ITextEmbeddingGeneration>();
        var embeddings = await embeddingGenerator.GenerateEmbeddingsAsync(new List<string>(){expected, actual});

        var result = embeddings[0].Span.CosineSimilarity(embeddings[1].Span);

        return result.ToString(CultureInfo.InvariantCulture);
    }
}