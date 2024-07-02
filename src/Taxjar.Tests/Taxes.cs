using System.Text.Json;
using FluentAssertions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Taxjar;
using Taxjar.Tests.Infrastructure;
using Taxjar.Tests.Fixtures;
using Microsoft.Extensions.Options;

namespace TaxJar.Tests;

public class Taxes
{
    protected IHttpClientFactory httpClientFactory;
    protected IOptions<TaxjarApiOptions> options = Substitute.For<IOptions<TaxjarApiOptions>>();
    protected string apiToken;
    protected string taxesEndpoint;
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

        taxesEndpoint = $"{options.Value.ApiUrl}/{TaxjarConstants.TaxesEndpoint}";
        defaultHeaders = new Dictionary<string, string>{
            {"Authorization", $"Bearer {options.Value.ApiToken}" },
            {"Accept", "application/json"}
        };
    }


    [TestCaseSource(typeof(TaxjarTestCaseData), nameof(TaxjarTestCaseData.TaxjarTaxCalculationRequestTestCases))]
    public async Task when_calculating_sales_tax_for_an_order_throws_async((TaxjarTaxCalculationRequest taxjarTaxCalculationRequest, string expectedMessage) testCase)
    {
        //arrange
        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        Func<Task> act = async () => await sut.TaxForOrderAsync(testCase.taxjarTaxCalculationRequest);

        //assert
        await act.Should().ThrowAsync<ArgumentException>()
         .WithMessage(testCase.expectedMessage);
    }

    [TestCase(true, TestName = "when_calculating_sales_tax_for_an_order_with_line_items_async")]
    [TestCase(false, TestName = "when_calculating_sales_tax_for_an_order_no_line_items_async")]
    public async Task when_calculating_sales_tax_for_an_generated_order_async(bool useLineItems)
    {
        //arrange
        var taxJarOrder = TaxjarFakes.FakeTaxjarTaxCalculationRequest(useLineItems).Generate();
        var jsonRequestPayload = JsonSerializer.Serialize(taxJarOrder, options.Value.JsonSerializerOptions);

        var expected = TaxjarFakes.FakeTaxResponse(useLineItems).Generate();        
        var jsonResponseBody = JsonSerializer.Serialize(expected, options.Value.JsonSerializerOptions);

        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Post, taxesEndpoint)
            .WithHeaders(defaultHeaders)
            .WithContent(jsonRequestPayload)
            .Respond("application/json", jsonResponseBody);

        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler));

        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        var result = await sut.TaxForOrderAsync(taxJarOrder);

        //assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expected!.Tax);
    }
}
