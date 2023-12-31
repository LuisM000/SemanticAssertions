using Microsoft.Extensions.Configuration;
using SemanticAssertions.Abstractions.Diagnostics;
using Xunit.Abstractions;

namespace SemanticAssertions.IntegrationTests.Asserts.Shared;

public abstract class AssertNotTestBase
{
    protected AssertNotTestBase(ITestOutputHelper output)
    {
        var logger = new XunitLogger(output);

        var configuration = new ConfigurationBuilder()
            .AddJsonFile(path: "testsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile(path: "testsettings.development.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddUserSecrets<AssertNotTestBase>()
            .Build();

        Configuration.Current
            .AddAzureTextCompletion(
            configuration.GetValue<string>("AzureOpenAI:ChatDeploymentName"),
            configuration.GetValue<string>("AzureOpenAI:Endpoint"),
            configuration.GetValue<string>("AzureOpenAI:ApiKey"))
            .AddAzureTextEmbeddingGeneration(
            configuration.GetValue<string>("AzureOpenAI:TextEmbeddingsDeploymentName"),
            configuration.GetValue<string>("AzureOpenAI:Endpoint"),
            configuration.GetValue<string>("AzureOpenAI:ApiKey"))
            .WithLoggerFactory(logger);
    }

    [Fact]
    public async Task throw_exception_when_texts_have_a_similarity_greater_than_08()
    {
        var exception = await Record.ExceptionAsync(() => Async.Assert.Not.AreSimilar(
            "El Teide tiene 3718 metros",
            "El Teide, que se encuentra en la isla de Tenerife en España, tiene una altura de aproximadamente 3718 metros sobre el nivel del mar. Es el pico más alto de España y uno de los volcanes más altos del mundo si se mide desde su base en el lecho oceánico.",
            0.8));

        Assert.IsType<SemanticAssertionsException>(exception);
    }

    [Fact]
    public async Task not_throw_exception_when_texts_have_a_similarity_less_than_08()
    {
        var exception = await Record.ExceptionAsync(() => Async.Assert.Not.AreSimilar(
            "Nueva York está en USA",
            "El Teide, que se encuentra en la isla de Tenerife en España, tiene una altura de aproximadamente 3718 metros sobre el nivel del mar. Es el pico más alto de España y uno de los volcanes más altos del mundo si se mide desde su base en el lecho oceánico.",
            0.8));

        Assert.Null(exception);
    }

    [Fact]
    public async Task throw_exception_when_texts_are_similar()
    {
        var exception = await Record.ExceptionAsync(() => Async.Assert.Not.AreSimilar(
            "El Teide tiene 3718 metros",
            "El Teide, que se encuentra en la isla de Tenerife en España, tiene una altura de aproximadamente 3718 metros sobre el nivel del mar. Es el pico más alto de España y uno de los volcanes más altos del mundo si se mide desde su base en el lecho oceánico."));

        Assert.IsType<SemanticAssertionsException>(exception);
    }

    [Fact]
    public async Task not_throw_exception_when_texts_are_not_similar()
    {
        var exception = await Record.ExceptionAsync(() => Async.Assert.Not.AreSimilar(
            "Nueva York está en USA",
            "El Teide, que se encuentra en la isla de Tenerife en España, tiene una altura de aproximadamente 3718 metros sobre el nivel del mar. Es el pico más alto de España y uno de los volcanes más altos del mundo si se mide desde su base en el lecho oceánico."));

        Assert.Null(exception);
    }

    [Fact]
    public async Task throw_exception_when_texts_are_in_same_language()
    {
        var exception = await Record.ExceptionAsync(() => Async.Assert.Not.AreInSameLanguage(
            "Esto es un texto en castellano",
            "Esto es otro texto que no debería lanzar excepción porque está en el mismo idioma"));

        Assert.IsType<SemanticAssertionsException>(exception);
    }

    [Fact]
    public async Task not_throw_exception_when_texts_are_in_different_languages()
    {
        var exception = await Record.ExceptionAsync(() => Async.Assert.Not.AreInSameLanguage(
            "Esto es un texto en castellano",
            "This text must be raise an exception because it is in a different language"));

        Assert.Null(exception);
    }
    
    [Theory]
    [InlineData("El Teide es el más alto de España")]
    [InlineData("El Teide, que se encuentra en la isla de Tenerife en España, tiene una altura de aproximadamente 3718 metros sobre el nivel del mar. Es el pico más alto de España y uno de los volcanes más altos del mundo si se mide desde su base en el lecho oceánico.")]
    public async Task throw_exception_when_text_contains_subset_of_information_included_in_another_text(string actual)
    {
        var exception = await Record.ExceptionAsync(() => Async.Assert.Not.ContainsInformationSubset(
            "El Teide, que se encuentra en la isla de Tenerife en España, tiene una altura de aproximadamente 3718 metros sobre el nivel del mar. Es el pico más alto de España y uno de los volcanes más altos del mundo si se mide desde su base en el lecho oceánico.",
            actual));

        Assert.IsType<SemanticAssertionsException>(exception);
    }
    
    [Theory]
    [InlineData("El Teide, que se encuentra en la isla de Tenerife en España, tiene una altura de aproximadamente 3718 metros sobre el nivel del mar. Es el pico más alto de España y uno de los volcanes más altos del mundo si se mide desde su base en el lecho oceánico.")]
    [InlineData("Nueva York está en USA")]
    public async Task not_throw_exception_when_text_not_contains_subset_of_information_included_in_another_text(string actual)
    {
        var exception = await Record.ExceptionAsync(() => Async.Assert.Not.ContainsInformationSubset(
            "El Teide es el más alto de España",
            actual));

        Assert.Null(exception);
    }
}