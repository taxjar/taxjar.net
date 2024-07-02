using System.Net;
using System.Text.Json;
using FluentAssertions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Taxjar.Tests.Infrastructure;
using Taxjar.Tests.Fixtures;
using Microsoft.Extensions.Options;

namespace Taxjar.Tests;

[TestFixture]
public class Client
{
    protected IHttpClientFactory httpClientFactory;
    protected IOptions<TaxjarApiOptions> options = Substitute.For<IOptions<TaxjarApiOptions>>();
    protected string apiToken;
    protected string categoriesEndpoint;
    protected Dictionary<string, string> defaultHeaders;

    [SetUp]
    public void Init()
    {
        apiToken = TaxjarFakes.Faker.Internet.Password();
        httpClientFactory = Substitute.For<IHttpClientFactory>();
        options.Value.Returns(new TaxjarApiOptions
        {
            ApiToken = apiToken,
            ApiUrl = TaxjarFakes.Faker.Internet.UrlWithPath(protocol: "https", domain: "api.taxjartest.com"),
        });

        categoriesEndpoint = $"{options.Value.ApiUrl}/{TaxjarConstants.CategoriesEndpoint}";

        defaultHeaders = new Dictionary<string, string>{
            {"Authorization", $"Bearer {options.Value.ApiToken}" },
            {"Accept", "application/json"}
        };
    }

    [Test]
    public async Task when_config_uses_defaults()
    {
        //arrange
        var body = TaxjarFixture.GetJSON("categories.json");
        var expected = JsonSerializer.Deserialize<CategoriesResponse>(body);
        var testOptions = Substitute.For<IOptions<TaxjarApiOptions>>();

        testOptions.Value.Returns(new TaxjarApiOptions{
            ApiToken = TaxjarFakes.Faker.Internet.Password()
        });

        var sut = new TaxjarApi(httpClientFactory, testOptions);
        var baseUrl = $"{TaxjarConstants.DefaultApiUrl}/{TaxjarConstants.ApiVersion}/";

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Get, $"{baseUrl}{TaxjarConstants.CategoriesEndpoint}")
            .WithHeaders(new Dictionary<string, string>{
                {"Authorization", $"Bearer {testOptions.Value.ApiToken}" },
                {"Accept", "application/json" },
                //investigate!
                //{"User-Agent", sut.UserAgent}
                })
                .Respond("application/json", body);


        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        //act
        var categories = await sut.CategoriesAsync();

        //assert
        sut.ApiUrl.Should().Be(baseUrl);
        categories.Should().NotBeNullOrEmpty();
        categories.Should().BeEquivalentTo(expected!.Categories);
    }

    [Test]
    public async Task when_config_set_to_defaults()
    {
        //arrange
        var testOptions = Substitute.For<IOptions<TaxjarApiOptions>>();

        testOptions.Value.Returns(new TaxjarApiOptions
        {
            ApiToken = TaxjarFakes.Faker.Internet.Password(),
            ApiUrl = TaxjarConstants.DefaultApiUrl
        });

        var baseUrl = $"{TaxjarConstants.DefaultApiUrl}/{TaxjarConstants.ApiVersion}/";

        //act
        var sut = new TaxjarApi(httpClientFactory, testOptions);

        //assert
        sut.ApiUrl.Should().Be(baseUrl);
    }

    [Test]
    public void when_missing_api_token_throws()
    {
        //arrange
        TaxjarApi sut;
        options.Value.Returns(new TaxjarApiOptions());

        //act
        Action act = () => sut = new TaxjarApi(httpClientFactory, options);

        //assert
        act.Should().Throw<ArgumentException>()
        .WithMessage("Please provide a TaxJar API key.*");
    }


    [Test]
    public async Task includes_appropriate_headers()
    {
        //arrange
        var body = TaxjarFixture.GetJSON("categories.json");
        var expected = JsonSerializer.Deserialize<CategoriesResponse>(body);
        var sut = new TaxjarApi(httpClientFactory, options);


        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Get, categoriesEndpoint)
            .WithHeaders(defaultHeaders)
            .Respond("application/json", body);


        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        //act
        var categories = await sut.CategoriesAsync();

        //assert
        categories.Should().NotBeNullOrEmpty();
        categories.Should().BeEquivalentTo(expected!.Categories);
    }


    [Test]
    public async Task when_api_token_is_invalid_throws()
    {
        //arrange
        var expectedStatusCode = HttpStatusCode.Unauthorized;
        var expectedDetail = "Not authorized for route 'GET /v2/categories'";
        var expectedMessage = $"{expectedStatusCode} - {expectedDetail}";

        var sut = new TaxjarApi(httpClientFactory, options);
        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Get, categoriesEndpoint)
            .WithHeaders(defaultHeaders)
            .Respond(HttpStatusCode.Unauthorized, "application/json", $$"""
            {
                "error": "{{expectedStatusCode}}",
                "detail": "{{expectedDetail}}",
                "status": "{{(int)expectedStatusCode}}"
            }
            """);


        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler)
        {
            BaseAddress = new Uri($"{TaxjarConstants.DefaultApiUrl}/{TaxjarConstants.ApiVersion}")
        }
        );

        //act
        Func<Task> act = () => sut.CategoriesAsync();

        //assert
        await act.Should().ThrowAsync<TaxjarException>()
        .Where(ex => ex.HttpStatusCode == expectedStatusCode)
        .Where(ex => ex.TaxjarError.Error == expectedStatusCode.ToString())
        .Where(ex => ex.TaxjarError.StatusCode == ((int)expectedStatusCode).ToString())
        .Where(ex => ex.TaxjarError.Detail == expectedDetail)
        .WithMessage(expectedMessage);
    }

    [Test]
    public async Task returns_exception_with_timeout()
    {
        //arrange
        var timeout = TimeSpan.FromMilliseconds(100);

        options.Value.Returns(options.Value with
        {
            Timeout = timeout
        });
        
        var sut = new TaxjarApi(httpClientFactory, options);
        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Get, categoriesEndpoint)
            .WithHeaders(defaultHeaders)
            .Respond(async () => { 
                await Task.Delay(Convert.ToInt32(timeout.TotalMilliseconds) + TaxjarFakes.Faker.Random.Number(300, 400));
                return new HttpResponseMessage(HttpStatusCode.OK);
            });


        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler)
        {
            BaseAddress = new Uri($"{TaxjarConstants.DefaultApiUrl}/{TaxjarConstants.ApiVersion}")
        }
        );

        //act
        Func<Task> act = () => sut.CategoriesAsync();

        //assert
        await act.Should().ThrowAsync<Exception>()
        .WithMessage("The request was canceled*");
    }
}