using SemanticAssertions.Abstractions;

namespace SemanticAssertions.Internals;

internal class ParserManagerHandler : IParserHandler
{
    private readonly IParserHandler[] parserHandlers;

    public ParserManagerHandler(params IParserHandler[] parserHandlers)
    {
        this.parserHandlers = parserHandlers;
    }

    public Task<bool> ParseBoolAsync(string? value)
    {
        return Parse(handler => handler.ParseBoolAsync(value));
    }

    public Task<double> ParseDoubleAsync(string? value)
    {
        return Parse(handler => handler.ParseDoubleAsync(value));
    }
    
    private async Task<T> Parse<T>(Func<IParserHandler, Task<T>> parser)
    {
        foreach (var handler in parserHandlers)
        {
            try
            {
                return await parser(handler).ConfigureAwait(false);
            }
            catch
            {
                // ignored. ToDo: maybe log
            }
        }

        throw new Exception(); //ToDo: review this exception. Should be an exception that indicates no test error but library error
    }
}