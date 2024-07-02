using System.Text.Json;
using FluentAssertions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Taxjar.Tests.Infrastructure;
using TaxJar.Tests;
using Taxjar.Tests.Fixtures;
using Microsoft.Extensions.Options;
using System.Net;

namespace Taxjar.Tests;

[TestFixture]
public class Rates
{
    protected IHttpClientFactory httpClientFactory;
    protected IOptions<TaxjarApiOptions> options = Substitute.For<IOptions<TaxjarApiOptions>>();
    protected string apiToken;
    protected string ratesEndpoint;
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

        ratesEndpoint = FormatRatesEndpoint(options.Value.ApiUrl);
        defaultHeaders = new Dictionary<string, string>{
            {"Authorization", $"Bearer {options.Value.ApiToken}" },
            {"Accept", "application/json"}
        };
    }

    private static string FormatRatesEndpoint(string apiUrl) => $"{apiUrl}/{TaxjarConstants.RatesEndpoint}";


    [TestCaseSource(typeof(TaxjarTestCaseData), nameof(TaxjarTestCaseData.RatesTestCasesAsync))]
    public async Task when_showing_tax_rates_for_a_location_async(string jsonFilePath)
    {
        //arrange
        var jsonData = TaxjarFixture.GetJSON(jsonFilePath);
        var expected = JsonSerializer.Deserialize<RateResponse>(jsonData, options.Value.JsonSerializerOptions);
        var responseBody = JsonSerializer.Serialize(expected!, options.Value.JsonSerializerOptions);
        var rateAddress = new Address{ Zip = expected!.Rate!.Zip };

        var ratesForLocationEndpoint = $"{ratesEndpoint}/{expected.Rate.Zip}";

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Get, ratesForLocationEndpoint)
            .Respond(TaxjarConstants.ContentType, responseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler)
        );

        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        var result = await sut.RatesForLocationAsync(rateAddress);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected!.Rate);
    }

    [Test]
    public async Task when_showing_tax_rates_zip_code_is_null_or_whitesspace()
    {
        //arrange
        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        Func<Task> act = async () => await sut.RatesForLocationAsync(new Address{ Zip = string.Empty });

        //assert
        await act.Should().ThrowAsync<ArgumentNullException>()
         .WithMessage("*Zip is null or empty!*");
    }

    [Test]
    public async Task when_showing_tax_rates_async()
    {
        //arrange
        var expected = TaxjarFakes.FakeRateResponse().Generate();
        var address = new Address{ Zip = expected.Rate!.Zip};
        var jsonData = JsonSerializer.Serialize(expected, options.Value.JsonSerializerOptions);

        var ratesForLocationEndpoint = $"{ratesEndpoint}/{address.Zip}";

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Get, ratesForLocationEndpoint)
            .WithHeaders(defaultHeaders)
            .Respond("application/json", jsonData);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler)
        );

        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        var result = await sut.RatesForLocationAsync(address);

        //assert
        result.Should().NotBeNull();
        result!.Should().BeEquivalentTo(expected.Rate);
        result!.Zip.Should().Be(address.Zip);
    }

    [Test]
    public async Task when_showing_tax_rates_for_a_address_async()
    {
        //arrange
        var expected = TaxjarFakes.FakeRateResponse().Generate();
        var address = TaxjarFakes.FakeAddress().Generate();
        var jsonData = JsonSerializer.Serialize(expected, options.Value.JsonSerializerOptions);

        var ratesForLocationEndpoint = $"{ratesEndpoint}/{address.Zip}";

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Get, ratesForLocationEndpoint)
            .WithHeaders(defaultHeaders)
            .WithQueryString($"city={WebUtility.UrlEncode(address.City)}&state={WebUtility.UrlEncode(address.State)}&country={WebUtility.UrlEncode(address.Country)}")
            .Respond("application/json", jsonData);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler)
        );

        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        var result = await sut.RatesForLocationAsync(address);

        //assert
        result.Should().NotBeNull();
        result!.Should().BeEquivalentTo(expected.Rate);
        result!.Zip.Should().Be(expected.Rate!.Zip);
    }
}
