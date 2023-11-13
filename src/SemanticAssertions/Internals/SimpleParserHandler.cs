using System.Globalization;
using SemanticAssertions.Abstractions;
using SemanticAssertions.Abstractions.Diagnostics;

namespace SemanticAssertions.Internals;

internal class SimpleParserHandler : IParserHandler
{
    public Task<bool> ParseBoolAsync(string value)
    {
        if (bool.TryParse(value, out var result))
        {
            return Task.FromResult(result);
        }

        throw new UnexpectedSemanticAssertionsException($"Failed to parse '{value}' as a boolean"); 
    }

    public Task<double> ParseDoubleAsync(string value)
    {
        if (double.TryParse(value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            return Task.FromResult(result);
        }

        throw new UnexpectedSemanticAssertionsException($"Failed to parse '{value}' as a double");
    }
}