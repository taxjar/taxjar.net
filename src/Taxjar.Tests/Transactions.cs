using System.Text.Json;
using FluentAssertions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Taxjar;
using Taxjar.Tests.Infrastructure;
using Taxjar.Tests.Fixtures;
using Microsoft.Extensions.Options;

namespace TaxJar.Tests;

public class Transactions
{
    protected IHttpClientFactory httpClientFactory;
    protected IOptions<TaxjarApiOptions> options = Substitute.For<IOptions<TaxjarApiOptions>>();
    protected string apiToken;
    protected string transactionOrdersEndpoint;
    protected string refundOrdersEndpoint;
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

        transactionOrdersEndpoint = $"{options.Value.ApiUrl}/{TaxjarConstants.TransactionOrdersEndpoint}";
        refundOrdersEndpoint = $"{options.Value.ApiUrl}/{TaxjarConstants.TransactionRefundsEndpoint}";

        defaultHeaders = new Dictionary<string, string>{
            {"Authorization", $"Bearer {options.Value.ApiToken}" },
            {"Accept", "application/json"}
        };
    }
        
    [Test]
    public async Task when_listing_order_transactions_by_transaction_date_async()
    {
        //arrange
        var taxJarOrderFilter = new OrderFilter{
            TransactionDate = TaxjarFakes.Faker.Date.Past(1),           
        };

        var expected = TaxjarFakes.FakeOrdersResponse().Generate();        
        var jsonResponseBody = JsonSerializer.Serialize(expected, options.Value.JsonSerializerOptions);

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Get, transactionOrdersEndpoint)
            .WithQueryString($"transaction_date={taxJarOrderFilter.TransactionDate:yyyy/MM/dd}")
            .WithHeaders(defaultHeaders)
            .Respond("application/json", jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        var result = await sut.ListOrdersAsync(taxJarOrderFilter);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected.Orders);
    }

    [Test]
    public async Task when_listing_order_transactions_by_date_range_async()
    {
        //arrange
        var taxJarOrderFilter = new OrderFilter
        {
            FromTransactionDate = TaxjarFakes.Faker.Date.Past(1),
            ToTransactionDate = DateTime.UtcNow
        };

        var expected = TaxjarFakes.FakeOrdersResponse().Generate();
        var jsonResponseBody = JsonSerializer.Serialize(expected, options.Value.JsonSerializerOptions);

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Get, transactionOrdersEndpoint)
            .WithQueryString($"from_transaction_date={taxJarOrderFilter.FromTransactionDate:yyyy/MM/dd}&to_transaction_date={taxJarOrderFilter.ToTransactionDate:yyyy/MM/dd}")
            .WithHeaders(defaultHeaders)
            .Respond("application/json", jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        var result = await sut.ListOrdersAsync(taxJarOrderFilter);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected.Orders);
    }

    [Test]
    public async Task when_listing_order_transactions_by_provider_async()
    {
        //arrange
        var taxJarOrderFilter = new OrderFilter
        {
            Provider = "api"
        };

        var expected = TaxjarFakes.FakeOrdersResponse().Generate();
        var jsonResponseBody = JsonSerializer.Serialize(expected, options.Value.JsonSerializerOptions);

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Get, transactionOrdersEndpoint)
            .WithQueryString($"provider={taxJarOrderFilter.Provider}")
            .WithHeaders(defaultHeaders)
            .Respond("application/json", jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        var result = await sut.ListOrdersAsync(taxJarOrderFilter);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected.Orders);
    }

    [Test]
    public async Task when_showing_an_order_transaction_async()
    {
        //arrange
        var jsonData = TaxjarFixture.GetJSON("orders/show.json");

        var expected = JsonSerializer.Deserialize<OrderResponse>(jsonData, options.Value.JsonSerializerOptions);
        var expectedTransactionId = expected!.Order!.TransactionId;
        var jsonResponseBody = JsonSerializer.Serialize(expected, options.Value.JsonSerializerOptions);
        var endpoint = $"{transactionOrdersEndpoint}/{expectedTransactionId}";
        var handler = new MockHttpMessageHandler();

        handler
            .When(HttpMethod.Get, endpoint)
            .WithHeaders(defaultHeaders)
            .Respond("application/json", jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        var sut = new TaxjarApi(httpClientFactory, options);

        //act 
        var result = await sut.ShowOrderAsync(expectedTransactionId);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected.Order);
    }

    [Test]
    public async Task when_showing_an_order_transaction_with_provider_async()
    {
        //arrange
        var expected = TaxjarFakes.FakeOrderResponse().Generate();
       
        var expectedTransactionId = expected!.Order!.TransactionId;
        var expectedProvider = expected!.Order!.Provider;

        var jsonResponseBody = JsonSerializer.Serialize(expected, options.Value.JsonSerializerOptions);
        var endpoint = $"{transactionOrdersEndpoint}/{expectedTransactionId}?provider={expectedProvider}";
        var handler = new MockHttpMessageHandler();

        handler
            .When(HttpMethod.Get, endpoint)
            .WithHeaders(defaultHeaders)
            .Respond("application/json", jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        var sut = new TaxjarApi(httpClientFactory, options);

        //act 
        var result = await sut.ShowOrderAsync(expectedTransactionId, expectedProvider);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected.Order);
    }

    [Test]
    public async Task when_showing_an_order_without_transaction_throws_async()
    {
        //arrange
        var sut = new TaxjarApi(httpClientFactory, options);

        //act 
        Func<Task> act = async () => await sut.ShowOrderAsync(string.Empty);

        //assert
        await act.Should().ThrowAsync<ArgumentException>()
        .WithMessage("*Transaction ID cannot be null or an empty string.*");
    }



    [Test]
    public async Task when_creating_an_order_transaction_with_line_items_async()
    {
        //arrange
        var request = TaxjarFakes.FakeTaxjarCreateOrderRequest(generateLineItems: true, generateCustomerId: true).Generate();
        var jsonRequestBody = JsonSerializer.Serialize(request, options.Value.JsonSerializerOptions);

        var expected = TaxjarFakes.FakeOrderResponse(generateLineItems: true).Generate();
        var jsonResponseBody = JsonSerializer.Serialize(expected, options.Value.JsonSerializerOptions);

        var handler = new MockHttpMessageHandler();

        handler
            .When(HttpMethod.Post, transactionOrdersEndpoint)
            .WithHeaders(defaultHeaders)
            .WithContent(jsonRequestBody)
            .Respond("application/json", jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        var sut = new TaxjarApi(httpClientFactory, options);

        //act 
        var result = await sut.CreateOrderAsync(request);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected!.Order);
    }

    [Test]
    public async Task when_creating_an_order_transaction_async()
    {
        //arrange
        var request = TaxjarFakes.FakeTaxjarCreateOrderRequest().Generate();
        var jsonRequestBody = JsonSerializer.Serialize(request, options.Value.JsonSerializerOptions);

        var jsonData = TaxjarFixture.GetJSON("orders/show.json");
        var expected = JsonSerializer.Deserialize<OrderResponse>(jsonData, options.Value.JsonSerializerOptions);
        var jsonResponseBody = JsonSerializer.Serialize(expected, options.Value.JsonSerializerOptions);

        var handler = new MockHttpMessageHandler();

        handler
            .When(HttpMethod.Post, transactionOrdersEndpoint)
            .WithHeaders(defaultHeaders)
            .WithContent(jsonRequestBody)
            .Respond("application/json", jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        var sut = new TaxjarApi(httpClientFactory, options);

        //act 
        var result = await sut.CreateOrderAsync(request);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected!.Order);
    }

    [TestCaseSource(typeof(TaxjarTestCaseData), nameof(TaxjarTestCaseData.TaxjarCreateOrderRequestTestCases))]
    public async Task when_creating_an_order_transaction_with_missing_required_throws_async((TaxjarOrderRequest taxjarCreateOrderRequest, string expectedMessage) testCase)
    {
        //arrange
        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        Func<Task> act = async () => await sut.CreateOrderAsync(testCase.taxjarCreateOrderRequest);

        //assert
        await act.Should().ThrowAsync<ArgumentException>()
         .WithMessage(testCase.expectedMessage);
    }

    [Test]
    public async Task when_updating_an_order_transaction_async()
    {
        //arrange
        var request = TaxjarFakes.FakeTaxjarCreateOrderRequest(generateLineItems: true, generateCustomerId: true).Generate();
        var jsonRequestBody = JsonSerializer.Serialize(request, options.Value.JsonSerializerOptions);

        var expected = TaxjarFakes.FakeOrderResponse(generateLineItems: true).Generate();
        var jsonResponseBody = JsonSerializer.Serialize(expected, options.Value.JsonSerializerOptions);

        var endpoint = $"{transactionOrdersEndpoint}/{request.TransactionId}";
        var handler = new MockHttpMessageHandler();

        handler
            .When(HttpMethod.Put, endpoint)
            .WithHeaders(defaultHeaders)
            .WithContent(jsonRequestBody)
            .Respond("application/json", jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        var sut = new TaxjarApi(httpClientFactory, options);

        //act 
        var result = await sut.UpdateOrderAsync(request);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected!.Order);
    }

    [Test]
    public async Task when_updating_an_order_transaction_throws_async()
    {
        //arrange
        var request = TaxjarFakes.FakeTaxjarCreateOrderRequest().Generate() with { TransactionId = string.Empty};
        var sut = new TaxjarApi(httpClientFactory, options);
        var expectedMessage = $"Invalid TaxjarOrderRequest.*TransactionId*";

        //act
        Func<Task> act = async () => await sut.CreateOrderAsync(request);

        //assert
        await act.Should().ThrowAsync<ArgumentException>()
         .WithMessage(expectedMessage);
    }

    [Test]
    public async Task when_deleting_an_order_transaction_async()
    {
        //arrange
        var deleteResponse = TaxjarFakes.FakeDeleteOrderResponse().Generate();
        var jsonResponseBody = JsonSerializer.Serialize(deleteResponse, options.Value.JsonSerializerOptions);
        var transactionId = deleteResponse!.Order!.TransactionId;

        var expected = JsonSerializer.Deserialize<OrderResponse>(jsonResponseBody, options.Value.JsonSerializerOptions);

        var endpoint = $"{transactionOrdersEndpoint}/{transactionId}";
        var handler = new MockHttpMessageHandler();

        handler
            .When(HttpMethod.Delete, endpoint)
            .WithHeaders(defaultHeaders)
            .Respond("application/json", jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        var sut = new TaxjarApi(httpClientFactory, options);

        //act 
        var result = await sut.DeleteOrderAsync(transactionId);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected!.Order);
    }

    [Test]
    public async Task when_deleting_an_order_transaction_by_provider_async()
    {
        //arrange
        var deleteResponse = TaxjarFakes.FakeDeleteOrderResponse().Generate();
        var jsonResponseBody = JsonSerializer.Serialize(deleteResponse, options.Value.JsonSerializerOptions);
        var transactionId = deleteResponse!.Order!.TransactionId;
        var provider = TaxjarFakes.Faker.Random.AlphaNumeric(7);
        
        var expected = JsonSerializer.Deserialize<OrderResponse>(jsonResponseBody, options.Value.JsonSerializerOptions);

        var endpoint = $"{transactionOrdersEndpoint}/{transactionId}";

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Delete, endpoint)
            .WithQueryString($"provider={provider}")
            .WithHeaders(defaultHeaders)
            .Respond("application/json", jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        var result = await sut.DeleteOrderAsync(transactionId, provider);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected!.Order);
    }

    [Test]
    public async Task when_deleting_an_order_transaction_with_missing_transaction_id_throws_async()
    {
        //arrange
        var transactionId = string.Empty;
        var sut = new TaxjarApi(httpClientFactory, options);
        var expectedMessage = "*Transaction ID cannot be null or an empty string*";

        //act
        Func<Task> act = async () => await sut.DeleteOrderAsync(transactionId);

        //assert
        await act.Should().ThrowAsync<ArgumentException>()
         .WithMessage(expectedMessage);
    }

    [Test]
    public async Task when_listing_refund_transactions_by_transaction_date_async()
    {
        //arrange
        var taxJarOrderFilter = new RefundFilter
        {
            TransactionDate = TaxjarFakes.Faker.Date.Past(1),
        };

        var expected = TaxjarFakes.FakeRefundsResponse().Generate();
        var jsonResponseBody = JsonSerializer.Serialize(expected, options.Value.JsonSerializerOptions);

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Get, refundOrdersEndpoint)
            .WithQueryString($"transaction_date={taxJarOrderFilter.TransactionDate:yyyy/MM/dd}")
            .WithHeaders(defaultHeaders)
            .Respond("application/json", jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        var result = await sut.ListRefundsAsync(taxJarOrderFilter);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected.Refunds);
    }

    [Test]
    public async Task when_listing_refund_transactions_by_date_range_async()
    {
        //arrange
        var taxJarRefundFilter = new RefundFilter
        {
            FromTransactionDate = TaxjarFakes.Faker.Date.Past(1),
            ToTransactionDate = DateTime.UtcNow
        };

        var expected = TaxjarFakes.FakeRefundsResponse().Generate();
        var jsonResponseBody = JsonSerializer.Serialize(expected, options.Value.JsonSerializerOptions);

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Get, refundOrdersEndpoint)
            .WithQueryString($"from_transaction_date={taxJarRefundFilter.FromTransactionDate:yyyy/MM/dd}&to_transaction_date={taxJarRefundFilter.ToTransactionDate:yyyy/MM/dd}")
            .WithHeaders(defaultHeaders)
            .Respond("application/json", jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        var result = await sut.ListRefundsAsync(taxJarRefundFilter);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected.Refunds);
    }

    [Test]
    public async Task when_showing_a_refund_transaction_async()
    {
        //arrange
        var jsonData = TaxjarFixture.GetJSON("refunds/show.json");

        var expected = JsonSerializer.Deserialize<RefundResponse>(jsonData, options.Value.JsonSerializerOptions);
        var expectedTransactionId = expected!.Refund!.TransactionId;
        var jsonResponseBody = JsonSerializer.Serialize(expected, options.Value.JsonSerializerOptions);
        var endpoint = $"{refundOrdersEndpoint}/{expectedTransactionId}";
        var handler = new MockHttpMessageHandler();

        handler
            .When(HttpMethod.Get, endpoint)
            .WithHeaders(defaultHeaders)
            .Respond("application/json", jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        var sut = new TaxjarApi(httpClientFactory, options);

        //act 
        var result = await sut.ShowRefundAsync(expectedTransactionId);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected.Refund);
    }

    [Test]
    public async Task when_creating_a_refund_transaction_async()
    {
        //arrange
        var request = TaxjarFakes.FakeTaxjarRefundRequest().Generate();
        var jsonRequestBody = JsonSerializer.Serialize(request, options.Value.JsonSerializerOptions);

        var jsonData = TaxjarFixture.GetJSON("refunds/show.json");
        var expected = JsonSerializer.Deserialize<RefundResponse>(jsonData, options.Value.JsonSerializerOptions);
        var jsonResponseBody = JsonSerializer.Serialize(expected, options.Value.JsonSerializerOptions);

        var handler = new MockHttpMessageHandler();

        handler
            .When(HttpMethod.Post, refundOrdersEndpoint)
            .WithHeaders(defaultHeaders)
            .WithContent(jsonRequestBody)
            .Respond("application/json", jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        var sut = new TaxjarApi(httpClientFactory, options);

        //act 
        var result = await sut.CreateRefundAsync(request);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected!.Refund);
    }

    [TestCaseSource(typeof(TaxjarTestCaseData), nameof(TaxjarTestCaseData.TaxjarRefundRequestTestCases))]
    public async Task when_creating_a_refund_transaction_missing_required_throws_async((TaxjarRefundRequest taxjarRefundRequest, string expectedMessage) testCase)
    {
        //arrange
        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        Func<Task> act = async () => await sut.CreateRefundAsync(testCase.taxjarRefundRequest);

        //assert
        await act.Should().ThrowAsync<ArgumentException>()
         .WithMessage(testCase.expectedMessage);
    }

    [Test]
    public async Task when_updating_a_refund_transaction_async()
    {
        //arrange
        var request = TaxjarFakes.FakeTaxjarRefundRequest(generateLineItems: true, generateCustomerId: true).Generate();
        var jsonRequestBody = JsonSerializer.Serialize(request, options.Value.JsonSerializerOptions);

        var expected = TaxjarFakes.FakeRefundResponse(generateLineItems: true).Generate();
        var jsonResponseBody = JsonSerializer.Serialize(expected, options.Value.JsonSerializerOptions);

        var endpoint = $"{refundOrdersEndpoint}/{request.TransactionId}";
        var handler = new MockHttpMessageHandler();

        handler
            .When(HttpMethod.Put, endpoint)
            .WithHeaders(defaultHeaders)
            .WithContent(jsonRequestBody)
            .Respond("application/json", jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        var sut = new TaxjarApi(httpClientFactory, options);

        //act 
        var result = await sut.UpdateRefundAsync(request);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected!.Refund);
    }

    [Test]
    public async Task when_deleting_a_refund_transaction_async()
    {
        //arrange
        var deleteResponse = TaxjarFakes.FakeDeleteRefundResponse().Generate();
        var jsonResponseBody = JsonSerializer.Serialize(deleteResponse, options.Value.JsonSerializerOptions);
        var transactionId = deleteResponse!.Refund!.TransactionId;

        var expected = JsonSerializer.Deserialize<RefundResponse>(jsonResponseBody, options.Value.JsonSerializerOptions);

        var endpoint = $"{refundOrdersEndpoint}/{transactionId}";
        var handler = new MockHttpMessageHandler();

        handler
            .When(HttpMethod.Delete, endpoint)
            .WithHeaders(defaultHeaders)
            .Respond("application/json", jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        var sut = new TaxjarApi(httpClientFactory, options);

        //act 
        var result = await sut.DeleteRefundAsync(transactionId);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected!.Refund);
    }
}