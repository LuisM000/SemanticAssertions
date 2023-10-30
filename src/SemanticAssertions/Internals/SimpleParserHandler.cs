using System.Globalization;
using SemanticAssertions.Abstractions;

namespace SemanticAssertions.Internals;

public class SimpleParserHandler : IParserHandler
{
    public Task<bool> ParseBoolAsync(string? value)
    {
        if (bool.TryParse(value, out var result))
        {
            return Task.FromResult(result);
        }

        throw new Exception(); // ToDo: review this exception
    }

    public Task<double> ParseDoubleAsync(string? value)
    {
        if (double.TryParse(value?.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            return Task.FromResult(result);
        }

        throw new Exception(); // ToDo: review this exception
    }
}