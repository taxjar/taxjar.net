using FluentAssertions;
using NSubstitute;
using Taxjar.Tests.Infrastructure;
using TaxJar.Tests;
using Taxjar.Tests.Fixtures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Taxjar.Tests;

[TestFixture]
public class ServiceCollectionExtensionsTests
{

    private IConfiguration? configuration;

    [SetUp]
    public void Init()
    {

        configuration = new ConfigurationBuilder().Build();
    }

    [Test]
    public void when_service_collection_null_throws()
    {
        //arrange
        IServiceCollection? sut = null;

        //act
        Action act = () => sut!.AddTaxJar();

        //assert
         act.Should().Throw<ArgumentNullException>()
         .WithMessage("*A service collection is required.*");
    }

    [Test]
    public void when_add_taxjar_services_are_registered()
    {
        //arrange
        var configSectionName = nameof(Taxjar);
        IEnumerable<KeyValuePair<string, string?>>? inMemoryConfig = new List<KeyValuePair<string, string?>>{
            {new KeyValuePair<string, string?>( $"{configSectionName}:{nameof(TaxjarApiOptions.ApiToken)}", TaxjarFakes.Faker.Internet.Password())}
        };

        configuration = new ConfigurationBuilder()
        .AddInMemoryCollection(inMemoryConfig)
        .Build();
        
        var httpClientFactory = Substitute.For<IHttpClientFactory>();

        var serviceCollection = new ServiceCollection().AddSingleton(_ => configuration);

        serviceCollection.AddSingleton<IHttpClientFactory>(_ => httpClientFactory);
        
        //act
        serviceCollection.AddTaxJar();
        var provider = serviceCollection.BuildServiceProvider();

        //assert
        provider.GetService<ITaxjarApi>().Should().NotBeNull().And.BeOfType<TaxjarApi>();
    }
}