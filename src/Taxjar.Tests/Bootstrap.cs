using System.Net;
using System.Text.Json;
using NSubstitute;
using Taxjar.Tests.Fixtures;

namespace Taxjar.Tests;

[SetUpFixture]
static class Bootstrap
{

    [OneTimeSetUp]
    public static void Init()
    {
        TaxJarOptions = new()
        {
            JsonSerializerOptions = TaxjarConstants.TaxJarDefaultSerializationOptions,
            ApiToken = TaxjarFakes.Faker.Internet.Password(),
            ApiUrl = TaxjarFakes.Faker.Internet.UrlWithPath(protocol: "https", domain: "api.taxjartest.com"),
            ApiVersion = "v2",
            UseSandbox = false
        };

        HttpClientFactory = Substitute.For<IHttpClientFactory>();
    }

    public static IHttpClientFactory HttpClientFactory { get; private set; }
    public static TaxjarApiOptions TaxJarOptions { get; private set; }
    public static readonly Uri DefaultApiUri = new Uri($"{TaxjarConstants.DefaultApiUrl}/{TaxjarConstants.ApiVersion}");

    public static string FormatDateQueryParameter(this DateTime dateTime, JsonSerializerOptions jsonSerializerOptions) =>
            WebUtility.UrlEncode(JsonSerializer.Serialize(dateTime, jsonSerializerOptions).Replace("\"", string.Empty));

    public static string FormatDateQueryParameter(this DateTime? dateTime, JsonSerializerOptions jsonSerializerOptions) =>
         WebUtility.UrlEncode(JsonSerializer.Serialize(dateTime, jsonSerializerOptions).Replace("\"", string.Empty));
}