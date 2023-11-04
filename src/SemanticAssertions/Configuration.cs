using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SemanticAssertions.Abstractions;
using SemanticAssertions.Providers;

namespace SemanticAssertions;

public class Configuration
{
    public static Configuration Current { get; private set; } = new();
    
    internal Completion Completion { get; private set; } = Completion.Empty;
    internal Embeddings Embeddings { get; private set; } = Embeddings.Empty;
    internal IAssertProvider AssertProvider { get; private set; } = new DefaultAssertProvider();
    internal IParserProvider ParserProvider { get; private set; } = new DefaultParserProvider();
    internal ILoggerFactory LoggerFactory { get; private set; } = NullLoggerFactory.Instance;
    
    
    public Configuration AddAzureTextCompletion(string deploymentName, string endpoint, string apiKey)
    {
        Completion = new Completion(deploymentName, endpoint, apiKey);
        return this;
    }

    public Configuration AddAzureTextEmbeddingGeneration(string deploymentName, string endpoint, string apiKey)
    {
        Embeddings = new Embeddings(deploymentName, endpoint, apiKey);
        return this;
    }
    
    public Configuration AddAssertProvider(IAssertProvider provider)
    {
        AssertProvider = provider;
        return this;
    }
    
    public Configuration AddParserProvider(IParserProvider provider)
    {
        ParserProvider = provider;
        return this;
    }
    
    public Configuration WithLoggerFactory(ILoggerFactory loggerFactory)
    {
        LoggerFactory = loggerFactory;
        return this;
    }
}

internal class Completion
{
    public Completion(string deploymentName, string endpoint, string apiKey)
    {
        DeploymentName = deploymentName;
        Endpoint = endpoint;
        ApiKey = apiKey;
    }

    public static readonly Completion Empty = new(string.Empty, string.Empty, string.Empty);
    
    public string DeploymentName { get; }
    public string Endpoint { get; }
    public string ApiKey { get; }
}

internal class Embeddings
{
    public Embeddings(string deploymentName, string endpoint, string apiKey)
    {
        DeploymentName = deploymentName;
        Endpoint = endpoint;
        ApiKey = apiKey;
    }

    public static readonly Embeddings Empty = new(string.Empty, string.Empty, string.Empty);
    
    public string DeploymentName { get; }
    public string Endpoint { get; }
    public string ApiKey { get; }
}