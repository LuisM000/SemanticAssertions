namespace SemanticAssertions.Configuration;

public static class Embeddings
{
    internal static string DeploymentName { get; private set; } = string.Empty;
    internal static string Endpoint { get; private set; } = string.Empty;
    internal static string ApiKey { get; private set; } = string.Empty;
    
    public static void AddAzureTextEmbeddingGeneration(string deploymentName, string endpoint, string apiKey)
    {
        DeploymentName = deploymentName;
        Endpoint = endpoint;
        ApiKey = apiKey;
    }
}