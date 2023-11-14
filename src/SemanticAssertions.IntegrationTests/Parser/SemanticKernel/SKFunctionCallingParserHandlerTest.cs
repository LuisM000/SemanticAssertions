using Microsoft.Extensions.Configuration;
using SemanticAssertions.Abstractions.Diagnostics;
using SemanticAssertions.IntegrationTests.Asserts.SemanticKernel.Async;
using SemanticAssertions.Internals.SemanticKernel;
using Xunit.Abstractions;

namespace SemanticAssertions.IntegrationTests.Parser.SemanticKernel;

// ReSharper disable InconsistentNaming
public class SKFunctionCallingParserHandlerShould
    // ReSharper restore InconsistentNaming
{
    private readonly SKFunctionCallingParserHandler parserHandler;

    public SKFunctionCallingParserHandlerShould(ITestOutputHelper output)
    {
        var logger = new XunitLogger(output);

        var configuration = new ConfigurationBuilder()
            .AddJsonFile(path: "testsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile(path: "testsettings.development.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddUserSecrets<AssertShould>()
            .Build();

        Configuration.Current
            .AddAzureTextCompletion(
                configuration.GetValue<string>("AzureOpenAI:ChatDeploymentName"),
                configuration.GetValue<string>("AzureOpenAI:Endpoint"),
                configuration.GetValue<string>("AzureOpenAI:ApiKey"))
            .WithLoggerFactory(logger);

        parserHandler = new SKFunctionCallingParserHandler();
    }

    [Theory]
    [InlineData("true")]
    [InlineData("TRUE")]
    [InlineData("The result is true")]
    [InlineData("It seems to be true")]
    public async Task parse_boolean_value_as_true(string value)
    {
        var result = await parserHandler.ParseBoolAsync(value);

        Assert.True(result);
    }

    [Theory]
    [InlineData("false")]
    [InlineData("FALSE")]
    [InlineData("The result is false")]
    [InlineData("It seems to be false")]
    public async Task parse_boolean_value_as_false(string value)
    {
        var result = await parserHandler.ParseBoolAsync(value);

        Assert.False(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("Blablablla")]
    [InlineData("It is not clear if it is true or false")]
    public async Task throw_exception_when_parse_non_boolean_value(string value)
    {
        var exception = await Record.ExceptionAsync(() => parserHandler.ParseBoolAsync(value));

        Assert.IsType<UnexpectedSemanticAssertionsException>(exception);
    }

    [Theory]
    [InlineData("65200", 65200)]
    [InlineData("0.2", 0.2)]
    [InlineData("0,2", 0.2)]
    [InlineData("1350", 1350)]
    [InlineData("12500,50", 12500.50)]
    [InlineData("12500.50", 12500.50)]
    [InlineData("12 500.50", 12500.50)]
    [InlineData("12,500.50", 12500.50)]
    [InlineData("12 500,50", 12500.50)]
    [InlineData("12.500,50", 12500.50)]
    [InlineData("1856967,0003", 1856967.0003)]
    public async Task parse_double_value(string value, double expected)
    {
        var result = await parserHandler.ParseDoubleAsync(value);

        Assert.Equal(result, expected);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("Blablablla")]
    [InlineData("This is not double!")]
    public async Task throw_exception_when_parse_non_double_value(string value)
    {
        var exception = await Record.ExceptionAsync(() => parserHandler.ParseDoubleAsync(value));

        Assert.IsType<UnexpectedSemanticAssertionsException>(exception);
    }
}