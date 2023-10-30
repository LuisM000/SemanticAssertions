namespace SemanticAssertions.Abstractions;

public interface IParserHandler
{
    Task<bool> ParseBoolAsync(string? value);
    
    Task<double> ParseDoubleAsync(string? value);
}