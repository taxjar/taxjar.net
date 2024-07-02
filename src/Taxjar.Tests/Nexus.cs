using System.Text.Json;
using FluentAssertions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Taxjar;
using Taxjar.Tests.Infrastructure;
using Taxjar.Tests.Fixtures;
using Microsoft.Extensions.Options;

namespace TaxJar.Tests;

[TestFixture]
public class Nexus
{
    protected IHttpClientFactory httpClientFactory;
    protected IOptions<TaxjarApiOptions> options = Substitute.For<IOptions<TaxjarApiOptions>>();
    protected string apiToken;
    protected string nexusRegionsEndpoint;
    protected Dictionary<string, string> defaultHeaders;

    [SetUp]
    public void Init()
    {
        apiToken = TaxjarFakes.Faker.Internet.Password();
        httpClientFactory = Substitute.For<IHttpClientFactory>();
        options.Value.Returns(new TaxjarApiOptions
        {
            JsonSerializerOptions = TaxjarConstants.TaxJarDefaultSerializationOptions,
            ApiToken = TaxjarFakes.Faker.Internet.Password(),
            ApiUrl = TaxjarFakes.Faker.Internet.UrlWithPath(protocol: "https", domain: "api.taxjartest.com"),
            ApiVersion = "v2",
            UseSandbox = false
        });

        nexusRegionsEndpoint = FormatNexusEndpoint(options.Value.ApiUrl);
        defaultHeaders = new Dictionary<string, string>{
            {"Authorization", $"Bearer {options.Value.ApiToken}" },
            {"Accept", "application/json"}
        };
    }

    private static string FormatNexusEndpoint(string apiUrl) => $"{apiUrl}/{TaxjarConstants.NexusRegionsEndpoint}";

    [Test]
    public async Task when_listing_nexus_regions_async()
    {
        //arrange
        var jsonData = TaxjarFixture.GetJSON("nexus_regions.json");
        var expected = JsonSerializer.Deserialize<NexusRegionsResponse>(jsonData, options.Value.JsonSerializerOptions);
        var responseBody = JsonSerializer.Serialize(expected!, options.Value.JsonSerializerOptions);

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Get, nexusRegionsEndpoint)
            .WithHeaders(defaultHeaders)
            .Respond(TaxjarConstants.ContentType, responseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler)
        );

        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        var result = await sut.NexusRegionsAsync();

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected!.Regions);
    }

    [Test]
    public async Task when_listing_nexus_generated_regions_async()
    {
        //arrange
        var expected = TaxjarFakes.FakeNexusRegionsResponse().Generate();
        var jsonData = JsonSerializer.Serialize(expected, options.Value.JsonSerializerOptions);

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Get, nexusRegionsEndpoint)
            .WithHeaders(defaultHeaders)
            .Respond(TaxjarConstants.ContentType, jsonData);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler)
        );

        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        var result = await sut.NexusRegionsAsync();

        //assert
        result.Should().NotBeNullOrEmpty();
        result.Should().BeEquivalentTo(expected!.Regions);
    }
}