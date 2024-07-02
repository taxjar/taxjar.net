using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Linq;
using System.Net;

namespace Taxjar.Infrastructure;

internal static class TaxjarRequestBuilder
{
    private const string resourcePathFormat = "{0}/{1}";
    private const string resourcePathAndQueryFormat = "{0}?{1}";
    public static Func<string, string, string> ResourcePath = (basePath, path) => string.Format(resourcePathFormat, basePath, path);

    public static HttpContent SerializerHttpContent<TRequest>(this TRequest request, JsonSerializerOptions jsonSerializerOptions)
    {
        var json = JsonSerializer.Serialize(request, jsonSerializerOptions);
        return new StringContent(json, Encoding.UTF8, TaxjarConstants.ContentType);
    }


    public static string TaxJarObjectToQueryParams<T>(this T taxjarObject, JsonSerializerOptions jsonSerializerOptions)
    {

        var jsonData = JsonSerializer.Serialize(taxjarObject, jsonSerializerOptions);
        var kvpParameters = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonData, jsonSerializerOptions);

        if(kvpParameters is null || kvpParameters.Count == 0)
        {
            return string.Empty;
        }
        
        var nullOrEmptyValues = kvpParameters.Where(kvp => string.IsNullOrWhiteSpace(kvp.Key) || string.IsNullOrWhiteSpace(kvp.Value));

        return string.Join('\u0026', kvpParameters
                        .Except(nullOrEmptyValues)
                        .Select(kvp => string.Format("{0}={1}", kvp.Key, WebUtility.UrlEncode(kvp.Value))));
    }

    public static string GetRatesForLocationPath(Address address, JsonSerializerOptions jsonSerializerOptions)
    {
        var queryParameters = TaxJarObjectToQueryParams(address with { Zip = string.Empty }, jsonSerializerOptions);
        var queryString = string.IsNullOrWhiteSpace(queryParameters) ? string.Empty : '\u003F' + queryParameters;

        return string.Format("{0}/{1}{2}", TaxjarConstants.RatesEndpoint, address.Zip, queryString);
    }

    public static string GetShowCustomerPath(string customerId) => ResourcePath(TaxjarConstants.CustomersEndpoint, customerId);

    public static string GetFilterOrdersPath(OrderFilter orderFilter, JsonSerializerOptions jsonSerializerOptions)
    {
        var queryString = TaxJarObjectToQueryParams(orderFilter, jsonSerializerOptions);

      return  string.Format(resourcePathAndQueryFormat, TaxjarConstants.TransactionOrdersEndpoint, queryString);
    }

    public static string GetTransactionIdPathWithProvider(string endpoint, string transactionId, string? provider = null)
    {
        TaxjarRequestValidation.ValidateTransactionId(transactionId);

        var queryString = !string.IsNullOrWhiteSpace(provider) ? string.Format("?{0}={1}", TaxjarConstants.ParameterName.Provider, provider) : string.Empty;

        return string.Format("{0}/{1}{2}", endpoint, transactionId, queryString);
    }
    public static string GetShowRefund(string transactionId, string? provider = null) => GetTransactionIdPathWithProvider(TaxjarConstants.TransactionRefundsEndpoint, transactionId, provider);
    
    public static string GetFilterRefundPath(RefundFilter refundFilter, JsonSerializerOptions jsonSerializerOptions)
    {
        var queryString = TaxJarObjectToQueryParams(refundFilter, jsonSerializerOptions);

        return string.Format(resourcePathAndQueryFormat, TaxjarConstants.TransactionRefundsEndpoint, queryString);
    }

}