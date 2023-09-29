// ReSharper disable MemberHidesStaticFromOuterClass
namespace SemanticAssertions.Internals.SemanticKernel.Plugins.SimilarityPlugin;

internal static class SimilarityPluginInfo
{
    public const string Name = "SimilarityPlugin";
    
    public static class AreSimilar
    {
        public const string Name = nameof(AreSimilar);
    }
    
    public static class CalculateSimilarity
    {
        public const string Name = nameof(CalculateSimilarity);
    }
}