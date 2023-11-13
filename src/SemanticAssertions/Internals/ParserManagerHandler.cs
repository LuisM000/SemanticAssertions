using Microsoft.Extensions.Logging;
using SemanticAssertions.Abstractions;
using SemanticAssertions.Abstractions.Diagnostics;

namespace SemanticAssertions.Internals;

internal class ParserManagerHandler : IParserHandler
{
    private readonly ILogger logger;
    private readonly IParserHandler[] parserHandlers;

    public ParserManagerHandler(params IParserHandler[] parserHandlers)
    {
        logger = Configuration.Current.LoggerFactory.CreateLogger<ParserManagerHandler>();
        this.parserHandlers = parserHandlers;
    }

    public Task<bool> ParseBoolAsync(string value)
    {
        return Parse(value, (handler, val) => handler.ParseBoolAsync(val));
    }

    public Task<double> ParseDoubleAsync(string value)
    {    
        return Parse(value, (handler, val) => handler.ParseDoubleAsync(val));
    }
    
    private async Task<T> Parse<T>(string value, Func<IParserHandler, string, Task<T>> parser)
    {
        foreach (var handler in parserHandlers)
        {
            try
            {
                return await parser(handler, value).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Unable to parse Value {value}", value);
            }
        }

        throw new UnexpectedSemanticAssertionsException($"Failed to parse '{value}' as a {typeof(T).Name}");
    }
}