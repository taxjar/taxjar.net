using System.Text.Json;
using System.Text.Json.Serialization;

namespace Taxjar;

public static class TaxjarConstants
{
    public const string DefaultApiUrl = "https://api.taxjar.com";
    public const string SandboxApiUrl = "https://api.sandbox.taxjar.com";
    public const string ApiVersion = "v2";
    public const string CategoriesEndpoint = "categories";
    public const string RatesEndpoint = "rates";
    public const string TaxesEndpoint = "taxes";
    public const string TransactionOrdersEndpoint = "transactions/orders";
    public const string TransactionRefundsEndpoint = "transactions/refunds";
    public const string CustomersEndpoint = "customers";
    public const string NexusRegionsEndpoint = "nexus/regions";
    public const string AddressesValidateEndpoint = "addresses/validate";
    public const string ValidationEndpoint = "validation";
    public const string SummaryRatesEndpoint = "summary_rates";
    public const int TimeoutInMilliseconds = 10000;
    public const string ContentType = "application/json";
    public const string DefaultProvider = "api";
    public static class ParameterName
    {
        public const string Provider = "provider";
    }

    public static class ReservedHeaders
    {
        public const string Authorization = "Authorization";
        public const string Accept = "Accept";
        public const string UserAgent = "User-Agent";
    }

    public static JsonSerializerOptions TaxJarDefaultSerializationOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        IgnoreReadOnlyFields = true,
        AllowTrailingCommas = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never
    };
}
