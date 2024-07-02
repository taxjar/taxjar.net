using FluentAssertions;
using Taxjar.Tests.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taxjar.Tests.Fixtures;

namespace Taxjar.Tests;

[TestFixture]
[Category("integration")]
public class IntegrationTests
{

    private IConfiguration? configuration;
    private ServiceProvider? serviceProvider;
    private DefaultHttpClientFactory? integrationHttpClientFactory;
    private IEnumerable<KeyValuePair<string, string?>> integrationTestBaseConfiguration;

    [SetUp]
    public void Init()
    {

        var configSectionName = nameof(Taxjar);
        integrationHttpClientFactory = new DefaultHttpClientFactory();
        integrationTestBaseConfiguration = new List<KeyValuePair<string, string?>>{
            {new KeyValuePair<string, string?>( $"{configSectionName}:{nameof(TaxjarApiOptions.Timeout)}", "00:00:15")},
            {new KeyValuePair<string, string?>( $"{configSectionName}:{nameof(TaxjarApiOptions.UseSandbox)}", "true")}
        };

        configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(integrationTestBaseConfiguration)
                .AddUserSecrets(TaxjarFixture.GetCurrentAssembly(), false)
                .Build();

        var serviceCollection = new ServiceCollection().AddSingleton(_ => configuration);

        serviceCollection.AddSingleton<IHttpClientFactory>(_ => integrationHttpClientFactory!);
        serviceCollection.AddTaxJar();

        serviceProvider = serviceCollection.BuildServiceProvider();


    }

    [TearDown]
    public void Teardown()
    {
        integrationHttpClientFactory?.Dispose();
        serviceProvider?.Dispose();
    }

    [Test]
    public async Task when_listing_tax_categories_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();

        //act
        var result = await sut!.CategoriesAsync();

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_listing_customers_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();

        //act
        var result = await sut!.ListCustomersAsync();

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_showing_a_customers_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();

        //act
        var result = await sut!.ShowCustomerAsync(TaxjarFakes.Faker.Random.Number(1_000, 5_000).ToString());

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_creating_a_customers_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();
        var customerRequest = TaxjarFakes.FakeTaxjarCustomerRequest().Generate();

        //act
        var result = await sut!.CreateCustomerAsync(customerRequest);

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_updating_a_customers_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();
        var taxjarCustomerRequest = TaxjarFakes.FakeTaxjarCustomerRequest().Generate();

        //act
        var result = await sut!.UpdateCustomerAsync(taxjarCustomerRequest);

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_deleting_a_customers_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();

        //act
        var result = await sut!.DeleteCustomerAsync(TaxjarFakes.Faker.Random.Number(1_000, 5_000).ToString());

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_showing_tax_rates_for_a_location_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();
        var address = new Address{
            Street = "1313 Disneyland Dr",
            City = "Anaheim",
            State = "CA",
            Zip = "92802"
        };

        //act
        var result = await sut!.RatesForLocationAsync(address);

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_listing_nexus_regions_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();

        //act
        var result = await sut!.NexusRegionsAsync();

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_summarizing_tax_rates_for_all_regions_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();
        var taxjarTaxCalculationRequest = TaxjarFakes.FakeTaxjarTaxCalculationRequest().Generate();

        //act
        var result = await sut!.TaxForOrderAsync(taxjarTaxCalculationRequest);

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_listing_order_transactions_by_transaction_date_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();
        var orderFilter = new OrderFilter {
            TransactionDate = TaxjarFakes.Faker.Date.Past()
        };


        //act
        var result = await sut!.ListOrdersAsync(orderFilter);

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_listing_order_transactions_by_date_range_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();
        var orderFilter = new OrderFilter
        {
            FromTransactionDate = TaxjarFakes.Faker.Date.Past(),
            ToTransactionDate = DateTime.UtcNow,
        };

        //act
        var result = await sut!.ListOrdersAsync(orderFilter);

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_showing_an_order_transaction_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();


        //act
        var result = await sut!.ShowOrderAsync(TaxjarFakes.Faker.Random.Number(1_000, 5_000).ToString());

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_creating_an_order_transaction_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();
        var taxjarOrder = TaxjarFakes.FakeTaxjarCreateOrderRequest(generateLineItems: true).Generate() with {
            ExemptionType = "non_exempt"
        };

        //act
        var result = await sut!.CreateOrderAsync(taxjarOrder);

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_updating_an_order_transaction_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();
        var taxjarOrder = TaxjarFakes.FakeTaxjarCreateOrderRequest(generateLineItems: true).Generate() with
        {
            ExemptionType = "non_exempt"
        };

        //act
        var result = await sut!.UpdateOrderAsync(taxjarOrder);

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_deleting_an_order_transaction_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();

        //act
        var result = await sut!.DeleteOrderAsync(TaxjarFakes.Faker.Random.Number(1_000, 5_000).ToString());

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_listing_refund_transactions_by_transaction_date_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();
        var refundFilter = new RefundFilter
        {
            TransactionDate = TaxjarFakes.Faker.Date.Past()
        };

        //act
        var result = await sut!.ListRefundsAsync(refundFilter);

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_listing_refund_transactions_by_date_range_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();
        var refundFilter = new RefundFilter
        {
            FromTransactionDate = TaxjarFakes.Faker.Date.Past(),
            ToTransactionDate = DateTime.UtcNow
        };

        //act
        var result = await sut!.ListRefundsAsync(refundFilter);

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_showing_an_refund_transaction_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();

        //act
        var result = await sut!.ShowRefundAsync(TaxjarFakes.Faker.Random.Number(1_000, 5_000).ToString());

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_creating_an_refund_transaction_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();
        var taxjarCrateRefundRequest = TaxjarFakes.FakeTaxjarRefundRequest().Generate() with
        {
            ExemptionType = "non_exempt"
        };

        //act
        var result = await sut!.CreateRefundAsync(taxjarCrateRefundRequest);

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_updating_an_refund_transaction_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();
        var taxjarCrateRefundRequest = TaxjarFakes.FakeTaxjarRefundRequest().Generate() with
        {
            ExemptionType = "non_exempt"
        };

        //act
        var result = await sut!.UpdateRefundAsync(taxjarCrateRefundRequest);

        //assert
        result.Should().NotBeNull();
    }

    [Test]
    public async Task when_deleting_an_refund_transaction_async()
    {
        //arrange
        var sut = serviceProvider!.GetService<ITaxjarApi>();

        //act
        var result = await sut!.DeleteRefundAsync(TaxjarFakes.Faker.Random.Number(1_000, 5_000).ToString());

        //assert
        result.Should().NotBeNull();
    }
}