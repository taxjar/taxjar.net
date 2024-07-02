using System.Text.Json;
using FluentAssertions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Taxjar.Tests.Infrastructure;
using Taxjar.Tests.Fixtures;
using Microsoft.Extensions.Options;

namespace Taxjar.Tests;

[TestFixture]
public class Categories
{
    protected IHttpClientFactory httpClientFactory;
    protected IOptions<TaxjarApiOptions> options = Substitute.For<IOptions<TaxjarApiOptions>>();

    [SetUp]
    public void Init()
    {
        httpClientFactory = Substitute.For<IHttpClientFactory>();
        options.Value.Returns(new TaxjarApiOptions
        {
            ApiToken = TaxjarFakes.Faker.Internet.Password(),
            ApiUrl = TaxjarConstants.DefaultApiUrl
        });
    }


    [Test]
    public async Task when_listing_tax_categories_async()
    {
        //arrange
        var body = TaxjarFixture.GetJSON("categories.json");
        var expected = JsonSerializer.Deserialize<CategoriesResponse>(body);
        
        var handler = new MockHttpMessageHandler();
        handler
            .When(HttpMethod.Get, $"{TaxjarConstants.DefaultApiUrl}/{options.Value.ApiVersion}/{TaxjarConstants.CategoriesEndpoint}")
             .Respond("application/json", body);

      
        httpClientFactory.CreateClient(nameof(TaxjarApi))
        .Returns(new HttpClient(handler)
        {
            BaseAddress = new Uri($"{TaxjarConstants.DefaultApiUrl}/{TaxjarConstants.ApiVersion}")
        }
        );
        
        var sut = new TaxjarApi(httpClientFactory, options);

        //act
        var categories = await sut.CategoriesAsync();
        
        //assert
        categories.Should().NotBeNullOrEmpty();
        categories.Should().BeEquivalentTo(expected!.Categories);

    }
}