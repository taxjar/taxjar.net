using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Taxjar;

public interface ITaxjarApi
{
     IDictionary<string, string> Headers { get; set; }
     string ApiUrl { get; set; }
     string ApiToken { get; set; }
     string UserAgent { get; set; }
     int Timeout { get; set; }

    /// <summary>
    /// Lists all tax categories.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>List of product categories and corresponding tax code</returns>
    Task<List<Category>?> CategoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Shows the sales tax rates for a given location.
    /// </summary>
    /// <param name="address"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Rates for a given location broken down by state, county, city, and district. </returns>
    Task<RateResponseAttributes?> RatesForLocationAsync(Address address, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists existing customers
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>List of customer IDs</returns>
    Task<List<string>?> ListCustomersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Shows an existing customer 
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Details of a customer.</returns>
    Task<CustomerResponseAttributes?> ShowCustomerAsync(string customerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="customer"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Details of the new customer.</returns>
    Task<CustomerResponseAttributes?> CreateCustomerAsync(TaxjarCustomerRequest taxjarCustomerRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing customer 
    /// </summary>
    /// <param name="customer"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Details of the updated customer.</returns>
    Task<CustomerResponseAttributes?> UpdateCustomerAsync(TaxjarCustomerRequest taxjarCustomerRequest, CancellationToken cancellationToken = default);

    /// <summary>
    ///Deletes an existing customer created 
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Deleted customer identifiers.</returns>
    Task<CustomerResponseAttributes?> DeleteCustomerAsync(string customerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists existing nexus locations for an existing TaxJar account.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>List of nexus regions sorted alphabetically.</returns>
    Task<List<NexusRegion>?> NexusRegionsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve minimum and average sales tax rates by region as a backup.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>List of summarized rates for each region/state.</returns>
    Task<List<SummaryRate>?> SummaryRatesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///Calculate sales tax for an order 
    /// </summary>
    ///<remarks>
    ///Shows the sales tax that should be collected for a given order.
    ///</remarks>
    /// <param name="taxjarTaxCalculationRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Returns calculated sales tax for a given order. If available, returns a breakdown of rates by jurisdiction at the order, shipping, and line item level.</returns>
    Task<TaxResponseAttributes?> TaxForOrderAsync(TaxjarTaxCalculationRequest taxjarTaxCalculationRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists existing order transactions.
    /// </summary>
    /// <param name="orderFilter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of order IDs created through the API.</returns>
    Task<List<string>?> ListOrdersAsync(OrderFilter orderFilter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Shows an existing order transaction created through the API.
    /// </summary>
    /// <param name="transactionId">Unique identifier of the given order transaction.</param>
    /// <param name="provider">Source of where the transaction was originally recorded. Defaults to “api”.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OrderResponseAttributes?> ShowOrderAsync(string transactionId, string? provider = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new order transaction.
    /// </summary>
    /// <param name="taxjarCreateOrderRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Details of a given order transaction.</returns>
    Task<OrderResponseAttributes?> CreateOrderAsync(TaxjarOrderRequest taxjarOrderRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing order transaction created through the API.
    /// </summary>
    /// <param name="taxjarCreateOrderRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Details of the updated order transaction.</returns>
    Task<OrderResponseAttributes?> UpdateOrderAsync(TaxjarOrderRequest taxjarOrderRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing order transaction created through the API.
    /// </summary>
    /// <param name="transactionId">Unique identifier of the given order transaction.</param>
    /// <param name="provider">Source of where the transaction was originally recorded. Defaults to “api”.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Deleted order transaction identifiers.</returns>
    Task<OrderResponseAttributes?> DeleteOrderAsync(string transactionId, string? provider = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists existing refund transactions
    /// </summary>
    /// <param name="refundFilter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of of refund IDs.</returns>
    Task<List<string>?> ListRefundsAsync(RefundFilter refundFilter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Shows an existing refund transaction
    /// </summary>
    /// <param name="refundTransactionId">Unique identifier of the given refund transaction.</param>
    /// <param name="provider">Source of where the transaction was originally recorded. Defaults to “api”.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Details of a given refund transaction</returns>
    public Task<RefundResponseAttributes?> ShowRefundAsync(string refundTransactionId, string? provider = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new refund transaction.
    /// </summary>
    /// <param name="taxjarRefundRequest"></param>
    /// <param name="provider"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Details of the new refund transaction.</returns>
    public Task<RefundResponseAttributes?> CreateRefundAsync(TaxjarRefundRequest taxjarRefundRequest, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing refund transaction created through the API.
    /// </summary>
    /// <param name="taxjarRefundRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Details of the updated refund transaction</returns>
    public Task<RefundResponseAttributes?> UpdateRefundAsync(TaxjarRefundRequest taxjarRefundRequest, CancellationToken cancellationToken = default);

    /// <summary>
    ///Deletes an existing refund transaction created through the API. 
    /// </summary>
    /// <param name="transactionId"></param>
    /// <param name="provider"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Deleted refund transaction identifiers.</returns>
    public Task<RefundResponseAttributes?> DeleteRefundAsync(string transactionId, string? provider = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates a customer address and returns back a collection of address matches.
    /// </summary>
    /// <param name="address"></param>
    /// <returns>List of address matches. If no addresses are found, a 404 response is returned.</returns>
    public Task<List<Address>?> ValidateAddressAsync(Address address, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates a customer address and returns back a collection of address matches
    /// </summary>
    /// <param name="validation"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ValidationResponseAttributes?> ValidateVatAsync(string vatNumber, CancellationToken cancellationToken = default);

}
