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
public class ValidationTests
{
    protected IHttpClientFactory httpClientFactory;
    protected IOptions<TaxjarApiOptions> options = Substitute.For<IOptions<TaxjarApiOptions>>();
    protected string apiToken;
    protected string addressValidationEndpoint;
    protected string validationEndpoint;
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

        addressValidationEndpoint = $"{options.Value.ApiUrl}/{TaxjarConstants.AddressesValidateEndpoint}";
        validationEndpoint = $"{options.Value.ApiUrl}/{TaxjarConstants.ValidationEndpoint}";
        defaultHeaders = new Dictionary<string, string>{
            {"Authorization", $"Bearer {options.Value.ApiToken}" },
            {"Accept", "application/json"}
        };
    }

    [Test]
    public async Task when_validating_an_address_async()
    {
        //arrange
        var jsonData = TaxjarFixture.GetJSON("addresses.json");
        var expected = JsonSerializer.Deserialize<AddressValidationResponse>(jsonData, options.Value.JsonSerializerOptions);
        
        var request = expected!.Addresses![0];
        var jsonRequestBody = JsonSerializer.Serialize(request, options.Value.JsonSerializerOptions);

        var jsonResponseBody = JsonSerializer.Serialize(expected!, options.Value.JsonSerializerOptions);

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Post, addressValidationEndpoint)
            .WithContent(jsonRequestBody)
            .WithHeaders(defaultHeaders)
            .Respond(TaxjarConstants.ContentType, jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler)
        );

        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        var result = await sut.ValidateAddressAsync(request);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected!.Addresses!);
    }

    [Test]
    public async Task when_validating_an_address_with_multiple_matches()
    {
        //arrange
        var jsonData = TaxjarFixture.GetJSON("addresses_multiple.json");
        var expected = JsonSerializer.Deserialize<AddressValidationResponse>(jsonData, options.Value.JsonSerializerOptions);
        var request = expected!.Addresses![0];
        var jsonRequestBody = JsonSerializer.Serialize(request, options.Value.JsonSerializerOptions);

        var jsonResponseBody = JsonSerializer.Serialize(expected!, options.Value.JsonSerializerOptions);

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Post, addressValidationEndpoint)
            .WithContent(jsonRequestBody)
            .WithHeaders(defaultHeaders)
            .Respond(TaxjarConstants.ContentType, jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler)
        );

        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        var result = await sut.ValidateAddressAsync(request);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected!.Addresses!);
    }

    [Test]
    public async Task when_validating_a_vat_number_async()
    {
        //arrange
        var jsonData = TaxjarFixture.GetJSON("validation.json");
        var expected = JsonSerializer.Deserialize<ValidationResponse>(jsonData, options.Value.JsonSerializerOptions);
        var vatNumber = expected!.Validation!.ViesResponse!.VatNumber;

        var jsonResponseBody = JsonSerializer.Serialize(expected!, options.Value.JsonSerializerOptions);
        var endpoint = $"{validationEndpoint}/{vatNumber}";
        
        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Get, endpoint)
            .WithHeaders(defaultHeaders)
            .Respond(TaxjarConstants.ContentType, jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler)
        );

        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        var result = await sut.ValidateVatAsync(vatNumber);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected!.Validation);
    }

    [Test]
    public async Task when_validating_a_vat_number_without_vat_throws_async()
    {
        //arrange
        var sut = new TaxjarApi(httpClientFactory, options);

        //act 
        Func<Task> act = async () => await sut.ValidateVatAsync(string.Empty);

        //assert
        await act.Should().ThrowAsync<ArgumentException>()
        .WithMessage("*VAT cannot be null or an empty string.*");
    }

}