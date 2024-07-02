using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Taxjar.Infrastructure;

namespace Taxjar;

public class TaxjarApi : ITaxjarApi
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IOptions<TaxjarApiOptions> options;

    public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    public string ApiUrl { get; set; }
    public string ApiToken { get; set; }
    public string UserAgent { get; set; }
    public int Timeout { get; set; }

    public TaxjarApi(IHttpClientFactory httpClientFactory, IOptions<TaxjarApiOptions> options)
    {
        this.httpClientFactory = httpClientFactory;
        this.options = options;
        if (string.IsNullOrWhiteSpace(options.Value.ApiToken))
        {
            throw new ArgumentException("Please provide a TaxJar API key.", nameof(options));
        }

        ApiToken = options.Value.ApiToken;
        ApiUrl = GetApiUrl(options.Value);
        Headers = options.Value.Headers;
        Timeout = Convert.ToInt32(options.Value.Timeout.TotalMilliseconds);
        UserAgent = GetUserAgent();
    }

    private static string GetApiUrl(TaxjarApiOptions taxJarApiOptions)
    {
        if (!string.IsNullOrWhiteSpace(taxJarApiOptions.ApiUrl)
        && taxJarApiOptions.ApiUrl.IndexOf(TaxjarConstants.DefaultApiUrl) == -1)
        {
            return taxJarApiOptions.ApiUrl.LastIndexOf('/') == taxJarApiOptions.ApiUrl.Length - 1 ? taxJarApiOptions.ApiUrl : taxJarApiOptions.ApiUrl + "/";
        }

        return taxJarApiOptions.UseSandbox ? GetApiBaseUrl(TaxjarConstants.SandboxApiUrl, taxJarApiOptions.ApiVersion) : GetApiBaseUrl(TaxjarConstants.DefaultApiUrl, taxJarApiOptions.ApiVersion);
    }

    private static string GetApiBaseUrl(string apiUrl, string apiVersion) => string.Format("{0}/{1}/", apiUrl, apiVersion);

    protected HttpClient CreateClient()
    { 

        var taxJarHttpClient = httpClientFactory.CreateClient(nameof(TaxjarApi));
        taxJarHttpClient.BaseAddress = new Uri(ApiUrl);
        taxJarHttpClient.Timeout = options.Value.Timeout;

        taxJarHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiToken);
        taxJarHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(TaxjarConstants.ContentType));
        taxJarHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);

        foreach (var key in Headers.Select(header => header.Key))
        {
            if (key.Equals(TaxjarConstants.ReservedHeaders.Authorization, StringComparison.OrdinalIgnoreCase)
                || key.Equals(TaxjarConstants.ReservedHeaders.Accept, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (key.Equals(TaxjarConstants.ReservedHeaders.UserAgent, StringComparison.OrdinalIgnoreCase))
            {
                taxJarHttpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(Headers[key]);
            }

            taxJarHttpClient.DefaultRequestHeaders.Add(key, Headers[key]);
        }

        return taxJarHttpClient;
    }

    private async Task<TResponse?> SendRequestAsync<TResponse>(Func<CancellationToken, Task<HttpResponseMessage>> callback, CancellationToken cancellationToken = default) where TResponse : new()
    {
        using (HttpResponseMessage response = await callback(cancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                var taxjarError = JsonSerializer.Deserialize<TaxjarError>(errorResponse, options.Value.JsonSerializerOptions) ?? throw new JsonException(string.Format("An error thrown, but it failed to deserialize type {0}. The original message was: {1}", typeof(TaxjarError).Name, errorResponse));
                var errorMessage = taxjarError.Error + " - " + taxjarError.Detail;
                throw new TaxjarException(response.StatusCode, taxjarError, errorMessage);
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(jsonResponse, options.Value.JsonSerializerOptions);
        }
    }
    
    private async Task<TResponse?> GetRequestAsync<TResponse>(string endpoint, CancellationToken cancellationToken = default) where TResponse : new()
    {
        return await SendRequestAsync<TResponse>(async (cancellationToken) =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await CreateClient().GetAsync(endpoint, cancellationToken).ConfigureAwait(false);
        }, cancellationToken);
    }

    private async Task<KResponse?> PostRequestAsync<TRequest, KResponse>(string endpoint, TRequest request, CancellationToken cancellationToken = default) where TRequest : new() where KResponse : new()
    {
        return await SendRequestAsync<KResponse>(async (cancellationToken) =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            var httpContent = TaxjarRequestBuilder.SerializerHttpContent(request, options.Value.JsonSerializerOptions);

            return await CreateClient().PostAsync(endpoint, httpContent, cancellationToken).ConfigureAwait(false);
        }, cancellationToken);
    }

    private async Task<KResponse?> PutRequestAsync<TRequest, KResponse>(string endpoint, TRequest request, CancellationToken cancellationToken = default) where TRequest : new() where KResponse : new()
    {
        return await SendRequestAsync<KResponse>(async (cancellationToken) =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            var httpContent = TaxjarRequestBuilder.SerializerHttpContent(request, options.Value.JsonSerializerOptions);

            return await CreateClient().PutAsync(endpoint, httpContent, cancellationToken).ConfigureAwait(false);
        }, cancellationToken);
    }

    private async Task<TResponse?> DeleteRequestAsync<TResponse>(string endpoint, CancellationToken cancellationToken = default) where TResponse : new()
    {
        return await SendRequestAsync<TResponse>(async (cancellationToken) =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await CreateClient().DeleteAsync(endpoint, cancellationToken).ConfigureAwait(false);
        }, cancellationToken);
    }

    /// <summary>
    /// Lists all tax categories.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>List of product categories and corresponding tax code</returns>
    public async Task<List<Category>?> CategoriesAsync(CancellationToken cancellationToken = default)
    {
        var response = await GetRequestAsync<CategoriesResponse>(TaxjarConstants.CategoriesEndpoint, cancellationToken);
        return response?.Categories;
    }

    /// <summary>
    /// Shows the sales tax rates for a given location.
    /// </summary>
    /// <param name="address"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Rates for a given location broken down by state, county, city, and district. </returns>
    public async Task<RateResponseAttributes?> RatesForLocationAsync(Address address, CancellationToken cancellationToken = default)
    {
        TaxjarRequestValidation.ValidateZip(address.Zip);
        
        var ratesForLocationPath = TaxjarRequestBuilder.GetRatesForLocationPath(address, options.Value.JsonSerializerOptions);
        var response = await GetRequestAsync<RateResponse>(ratesForLocationPath, cancellationToken).ConfigureAwait(false);
        
        return response?.Rate;
    }

    /// <summary>
    /// Lists existing customers
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>List of customer IDs</returns>
    public async Task<List<string>?> ListCustomersAsync(CancellationToken cancellationToken = default)
    {
        var response = await GetRequestAsync<CustomersResponse>(TaxjarConstants.CustomersEndpoint, cancellationToken);
        return response?.Customers;
    }

    /// <summary>
    /// Shows an existing customer 
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Details of a customer</returns>
    public async Task<CustomerResponseAttributes?> ShowCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        TaxjarRequestValidation.ValidateCustomerId(customerId);
        var showCustomersPath = TaxjarRequestBuilder.GetShowCustomerPath(customerId);

        var response = await GetRequestAsync<CustomerResponse>(showCustomersPath, cancellationToken).ConfigureAwait(false);
        return response?.Customer;
    }

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="customer"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Details of the new customer.</returns>
    public async Task<CustomerResponseAttributes?> CreateCustomerAsync(TaxjarCustomerRequest taxjarCustomerRequest, CancellationToken cancellationToken = default)
    {
        TaxjarRequestValidation.ValidateCustomer(taxjarCustomerRequest);
        return await PostRequestAsync<TaxjarCustomerRequest, CustomerResponseAttributes>(TaxjarConstants.CustomersEndpoint, taxjarCustomerRequest, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Updates an existing customer 
    /// </summary>
    /// <param name="customer"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Details of the updated customer.</returns>
    public async Task<CustomerResponseAttributes?> UpdateCustomerAsync(TaxjarCustomerRequest taxjarCustomerRequest, CancellationToken cancellationToken = default)
    {
        TaxjarRequestValidation.ValidateCustomer(taxjarCustomerRequest);
        var updateCustomersPath = TaxjarRequestBuilder.GetShowCustomerPath(taxjarCustomerRequest.CustomerId);

        return await PutRequestAsync<TaxjarCustomerRequest, CustomerResponseAttributes>(updateCustomersPath, taxjarCustomerRequest, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    ///Deletes an existing customer created 
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Deleted customer identifiers.</returns>
    public async Task<CustomerResponseAttributes?> DeleteCustomerAsync(string customerId, CancellationToken cancellationToken = default)
    {
        TaxjarRequestValidation.ValidateCustomerId(customerId);
        var deleteCustomersPath = TaxjarRequestBuilder.GetShowCustomerPath(customerId);

        var response = await DeleteRequestAsync<CustomerResponse>(deleteCustomersPath, cancellationToken).ConfigureAwait(false);
        return response?.Customer;
    }

    /// <summary>
    /// Lists existing nexus locations for an existing TaxJar account.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>List of nexus regions sorted alphabetically.</returns>
    public async Task<List<NexusRegion>?> NexusRegionsAsync(CancellationToken cancellationToken = default)
    {
        var response = await GetRequestAsync<NexusRegionsResponse>(TaxjarConstants.NexusRegionsEndpoint, cancellationToken).ConfigureAwait(false);
        return response?.Regions;
    }

    /// <summary>
    /// Retrieve minimum and average sales tax rates by region as a backup.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>List of summarized rates for each region/state.</returns>
    public async Task<List<SummaryRate>?> SummaryRatesAsync(CancellationToken cancellationToken = default)
    {
        var response = await GetRequestAsync<SummaryRatesResponse>(TaxjarConstants.SummaryRatesEndpoint, cancellationToken).ConfigureAwait(false);
        return response?.SummaryRates;
    }

    /// <summary>
    ///Calculate sales tax for an order 
    /// </summary>
    ///<remarks>
    ///Shows the sales tax that should be collected for a given order.
    ///</remarks>
    /// <param name="taxjarTaxCalculationRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Returns calculated sales tax for a given order. If available, returns a breakdown of rates by jurisdiction at the order, shipping, and line item level.</returns>
    public async Task<TaxResponseAttributes?> TaxForOrderAsync(TaxjarTaxCalculationRequest taxjarTaxCalculationRequest, CancellationToken cancellationToken = default)
    {
        TaxjarRequestValidation.ValidateTaxjarTaxCalculationRequest(taxjarTaxCalculationRequest);
        var response = await PostRequestAsync<TaxjarTaxCalculationRequest, TaxResponse>(TaxjarConstants.TaxesEndpoint, taxjarTaxCalculationRequest, cancellationToken).ConfigureAwait(false);
        return response?.Tax;
    }

    /// <summary>
    /// Lists existing order transactions.
    /// </summary>
    /// <param name="orderFilter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of order IDs created through the API.</returns>
    public async Task<List<string>?> ListOrdersAsync(OrderFilter orderFilter, CancellationToken cancellationToken = default)
    {
        var listOrdersPath = TaxjarRequestBuilder.GetFilterOrdersPath(orderFilter, options.Value.JsonSerializerOptions);

        var response = await GetRequestAsync<OrdersResponse>(listOrdersPath, cancellationToken).ConfigureAwait(false);

        return response?.Orders;
    }

    /// <summary>
    /// Shows an existing order transaction created through the API.
    /// </summary>
    /// <param name="transactionId">Unique identifier of the given order transaction.</param>
    /// <param name="provider">Source of where the transaction was originally recorded. Defaults to “api”.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<OrderResponseAttributes?> ShowOrderAsync(string transactionId, string? provider = null, CancellationToken cancellationToken = default)
    {
        TaxjarRequestValidation.ValidateTransactionId(transactionId);

        var showOrderPath = TaxjarRequestBuilder.GetTransactionIdPathWithProvider(TaxjarConstants.TransactionOrdersEndpoint, transactionId, provider);

        var response = await GetRequestAsync<OrderResponse>(showOrderPath, cancellationToken).ConfigureAwait(false);

        return response?.Order;
    }

    /// <summary>
    /// Creates a new order transaction.
    /// </summary>
    /// <param name="taxjarCreateOrderRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Details of a given order transaction.</returns>
    public async Task<OrderResponseAttributes?> CreateOrderAsync(TaxjarOrderRequest taxjarOrderRequest, CancellationToken cancellationToken = default)
    {
        TaxjarRequestValidation.ValidateTaxjarCreateOrderRequest(taxjarOrderRequest);

        var response = await PostRequestAsync<TaxjarOrderRequest, OrderResponse>(TaxjarConstants.TransactionOrdersEndpoint, taxjarOrderRequest, cancellationToken).ConfigureAwait(false);

        return response?.Order;
    }

    /// <summary>
    /// Updates an existing order transaction created through the API.
    /// </summary>
    /// <param name="taxjarCreateOrderRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Details of the updated order transaction.</returns>
    public async Task<OrderResponseAttributes?> UpdateOrderAsync(TaxjarOrderRequest taxjarOrderRequest, CancellationToken cancellationToken = default)
    {
        TaxjarRequestValidation.ValidateTaxjarUpdateOrderRequest(taxjarOrderRequest);
        
        var updateOrderPath = TaxjarRequestBuilder.ResourcePath(TaxjarConstants.TransactionOrdersEndpoint, taxjarOrderRequest.TransactionId);

        var response = await PutRequestAsync<TaxjarOrderRequest, OrderResponse>(updateOrderPath, taxjarOrderRequest, cancellationToken).ConfigureAwait(false);

        return response?.Order;
    }

    /// <summary>
    /// Deletes an existing order transaction created through the API.
    /// </summary>
    /// <param name="transactionId">Unique identifier of the given order transaction.</param>
    /// <param name="provider">Source of where the transaction was originally recorded. Defaults to “api”.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Deleted order transaction identifiers.</returns>
    public async Task<OrderResponseAttributes?> DeleteOrderAsync(string transactionId, string? provider = null, CancellationToken cancellationToken = default)
    {
        TaxjarRequestValidation.ValidateTransactionId(transactionId);

        var deleteOrderPath = TaxjarRequestBuilder.GetTransactionIdPathWithProvider(TaxjarConstants.TransactionOrdersEndpoint, transactionId, provider);

        var response = await DeleteRequestAsync<OrderResponse>(deleteOrderPath, cancellationToken).ConfigureAwait(false);

        return response?.Order;
    }

    /// <summary>
    /// Lists existing refund transactions
    /// </summary>
    /// <param name="refundFilter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of of refund IDs.</returns>
    public async Task<List<string>?> ListRefundsAsync(RefundFilter refundFilter, CancellationToken cancellationToken = default)
    {
        var listRefundPath = TaxjarRequestBuilder.GetFilterRefundPath(refundFilter, options.Value.JsonSerializerOptions);

        var response = await GetRequestAsync<RefundsResponse?>(listRefundPath, cancellationToken).ConfigureAwait(false);

        return response?.Refunds;
    }

    /// <summary>
    /// Shows an existing refund transaction
    /// </summary>
    /// <param name="refundTransactionId">Unique identifier of the given refund transaction.</param>
    /// <param name="provider">Source of where the transaction was originally recorded. Defaults to “api”.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Details of a given refund transaction</returns>
    public async Task<RefundResponseAttributes?> ShowRefundAsync(string refundTransactionId, string? provider = null, CancellationToken cancellationToken = default)
    {
        TaxjarRequestValidation.ValidateTransactionId(refundTransactionId);
        var showRefundPath = TaxjarRequestBuilder.GetShowRefund(refundTransactionId, provider);

        var response = await GetRequestAsync<RefundResponse>(showRefundPath, cancellationToken).ConfigureAwait(false);
        return response?.Refund;
    }

    /// <summary>
    /// Creates a new refund transaction.
    /// </summary>
    /// <param name="taxjarRefundRequest"></param>
    /// <param name="provider"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Details of the new refund transaction.</returns>
    public async Task<RefundResponseAttributes?> CreateRefundAsync(TaxjarRefundRequest taxjarRefundRequest, CancellationToken cancellationToken = default)
    {
        TaxjarRequestValidation.ValidateTaxjarRefundRequest(taxjarRefundRequest);

        var response = await PostRequestAsync<TaxjarRefundRequest, RefundResponse>(TaxjarConstants.TransactionRefundsEndpoint, taxjarRefundRequest, cancellationToken).ConfigureAwait(false);

        return response?.Refund;
    }

    /// <summary>
    /// Updates an existing refund transaction created through the API.
    /// </summary>
    /// <param name="taxjarRefundRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Details of the updated refund transaction</returns>
    public async Task<RefundResponseAttributes?> UpdateRefundAsync(TaxjarRefundRequest taxjarRefundRequest, CancellationToken cancellationToken = default)
    {
        TaxjarRequestValidation.ValidateTaxjarRefundRequestTransactionId(taxjarRefundRequest);

        var showRefundPath = TaxjarRequestBuilder.GetShowRefund(taxjarRefundRequest.TransactionId);

        var response = await PutRequestAsync<TaxjarRefundRequest, RefundResponse>(showRefundPath, taxjarRefundRequest, cancellationToken).ConfigureAwait(false);

        return response?.Refund;
    }

    /// <summary>
    ///Deletes an existing refund transaction created through the API. 
    /// </summary>
    /// <param name="transactionId"></param>
    /// <param name="provider"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Deleted refund transaction identifiers.</returns>
    public async Task<RefundResponseAttributes?> DeleteRefundAsync(string transactionId, string? provider = null, CancellationToken cancellationToken = default)
    {
        TaxjarRequestValidation.ValidateTransactionId(transactionId);

        var deleteRefundPath = TaxjarRequestBuilder.GetShowRefund(transactionId, provider);

        var response = await DeleteRequestAsync<RefundResponse>(deleteRefundPath, cancellationToken).ConfigureAwait(false);

        return response?.Refund;
    }

    /// <summary>
    /// Validates a customer address and returns back a collection of address matches.
    /// </summary>
    /// <param name="address"></param>
    /// <returns>List of address matches. If no addresses are found, a 404 response is returned.</returns>
    public async Task<List<Address>?> ValidateAddressAsync(Address address, CancellationToken cancellationToken = default)
    {
        var response = await PostRequestAsync<Address, AddressValidationResponse>(TaxjarConstants.AddressesValidateEndpoint, address, cancellationToken).ConfigureAwait(false);

        return response?.Addresses;
    }

    /// <summary>
    /// Validates a customer address and returns back a collection of address matches
    /// </summary>
    /// <param name="validation"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ValidationResponseAttributes?> ValidateVatAsync(string vatNumber, CancellationToken cancellationToken = default)
    {
        TaxjarRequestValidation.ValidateVat(vatNumber);

        var response = await GetRequestAsync<ValidationResponse>(TaxjarRequestBuilder.ResourcePath(TaxjarConstants.ValidationEndpoint, vatNumber), cancellationToken).ConfigureAwait(false);
        return response?.Validation;
    }

    private string GetUserAgent()
    {
        string platform = RuntimeInformation.OSDescription;
        string arch = RuntimeInformation.OSArchitecture.ToString();
        string framework = RuntimeInformation.FrameworkDescription;

        string version = GetType()?.Assembly?.GetName()?.Version?.ToString(3) ?? options.Value.ApiVersion;

        return $"TaxJar/.NET ({platform}; {arch}; {framework}) taxjar.net/{version}";
    }
}