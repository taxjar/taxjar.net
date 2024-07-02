using System.Text.Json;
using FluentAssertions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Taxjar.Tests.Infrastructure;
using TaxJar.Tests;
using Taxjar.Tests.Fixtures;
using Microsoft.Extensions.Options;

namespace Taxjar.Tests;

[TestFixture]
public class Customers
{
    protected IHttpClientFactory httpClientFactory;
    protected IOptions<TaxjarApiOptions> options = Substitute.For<IOptions<TaxjarApiOptions>>();
    protected string apiToken;
    protected string customersEndpoint;
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

        customersEndpoint = FormatCustomersEndpoint(options.Value.ApiUrl);
        defaultHeaders = new Dictionary<string, string>{
            {"Authorization", $"Bearer {options.Value.ApiToken}" },
            {"Accept", "application/json"}
        };
    }

    private static string FormatCustomersEndpoint(string apiUrl) => $"{apiUrl}/{TaxjarConstants.CustomersEndpoint}";

    [Test]
    public async Task when_listing_customers_async()
    {
        //arrange
        var jsonData = TaxjarFixture.GetJSON("customers/list.json");
        var expected = JsonSerializer.Deserialize<CustomersResponse>(jsonData, options.Value.JsonSerializerOptions);
        var responseBody = JsonSerializer.Serialize(expected!, options.Value.JsonSerializerOptions);

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Get, customersEndpoint)
            .Respond(TaxjarConstants.ContentType, responseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler)
        );

        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        var customers = await sut.ListCustomersAsync();

        //assert
        customers.Should().NotBeNullOrEmpty();
        customers.Should().BeEquivalentTo(expected!.Customers);
    }


    [Test]
    public async Task when_showing_a_customer_async()
    {
        //arrange
        var jsonData = TaxjarFixture.GetJSON("customers/show.json");
        var expected = JsonSerializer.Deserialize<CustomerResponse>(jsonData, options.Value.JsonSerializerOptions);
        var customerId = expected!.Customer!.CustomerId;
        var showCustomerEndpoint = $"{customersEndpoint}/{customerId}";

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Get, showCustomerEndpoint)
            .WithHeaders(defaultHeaders)
            .Respond("application/json", jsonData);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler)
        );

        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        var result = await sut.ShowCustomerAsync(customerId);

        //assert
        result.Should().NotBeNull();
        result!.Should().BeEquivalentTo(expected!.Customer);
    }

    [Test]
    public async Task when_creating_a_customer_async()
    {
        //arrange
        var jsonData = TaxjarFixture.GetJSON("customers/show.json");
        var expected = JsonSerializer.Deserialize<CustomerResponse>(jsonData, options.Value.JsonSerializerOptions);
        var jsonPayload = JsonSerializer.Serialize(expected!.Customer, options.Value.JsonSerializerOptions);
        var responseBody = JsonSerializer.Serialize(expected!.Customer, options.Value.JsonSerializerOptions);

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Post, customersEndpoint)
            .WithHeaders(defaultHeaders)
            .WithContent(jsonPayload)
            .Respond("application/json", responseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler)
        {
            BaseAddress = new Uri($"{TaxjarConstants.DefaultApiUrl}/{TaxjarConstants.ApiVersion}")
        }
        );

        var sut = new TaxjarApi(httpClientFactory, options);
        var request = new TaxjarCustomerRequest {
            CustomerId = expected.Customer!.CustomerId,
            ExemptionType = expected.Customer!.ExemptionType,
            Name = expected.Customer!.Name,
            ExemptRegions = expected.Customer!.ExemptRegions,
            Country = expected.Customer!.Country,
            State = expected.Customer!.State,
            Zip = expected.Customer!.Zip,
            City = expected.Customer!.City,
            Street = expected.Customer!.Street
        };

        //act
        var result = await sut.CreateCustomerAsync(request);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected!.Customer);
    }


    [Test]
    public async Task when_updating_a_customer_async()
    {
        //arrange
        var jsonData = TaxjarFixture.GetJSON("customers/show.json");
        var expected = JsonSerializer.Deserialize<CustomerResponse>(jsonData);
        var customerId = expected!.Customer!.CustomerId;
        var jsonPayload = JsonSerializer.Serialize(expected!.Customer, options.Value.JsonSerializerOptions);
        var updateCustomerEndpoint = $"{customersEndpoint}/{customerId}";

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Put, updateCustomerEndpoint)
            .WithHeaders(defaultHeaders)
            .WithContent(jsonPayload)
            .Respond("application/json", jsonPayload);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        var sut = new TaxjarApi(httpClientFactory, options);
        var request = new TaxjarCustomerRequest
        {
            CustomerId = expected.Customer!.CustomerId,
            ExemptionType = expected.Customer!.ExemptionType,
            Name = expected.Customer!.Name,
            ExemptRegions = expected.Customer!.ExemptRegions,
            Country = expected.Customer!.Country,
            State = expected.Customer!.State,
            Zip = expected.Customer!.Zip,
            City = expected.Customer!.City,
            Street = expected.Customer!.Street
        };

        //act
        var result = await sut.UpdateCustomerAsync(request);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected!.Customer);
    }

    [TestCaseSource(typeof(TaxjarTestCaseData), nameof(TaxjarTestCaseData.CustomerTestCases))]
    public async Task when_updating_a_customer_with_missing_customer_data((TaxjarCustomerRequest customer, string expectedMessage) testCase)
    {   
        //arrange
        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        Func<Task> act = async () => await sut.UpdateCustomerAsync(testCase.customer);

        //assert
        await act.Should().ThrowAsync<ArgumentException>()
         .WithMessage(testCase.expectedMessage);
    }

    [Test]
    public async Task when_deleting_a_customer_async()
    {
        //arrange
        var expected = TaxjarFakes.FakeCustomerResponse().Generate();
        var customerId = expected.Customer!.CustomerId;
        var jsonData = JsonSerializer.Serialize(expected, options.Value.JsonSerializerOptions);

        var deleteCustomerEndpoint = $"{customersEndpoint}/{customerId}";

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Delete, deleteCustomerEndpoint)
            .WithHeaders(defaultHeaders)
            .Respond("application/json", jsonData);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler)
        );

        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        var result = await sut.DeleteCustomerAsync(customerId);

        //assert
        result.Should().NotBeNull();
        result!.Should().BeEquivalentTo(expected.Customer!);
        result!.CustomerId.Should().Be(customerId);
    }
}
