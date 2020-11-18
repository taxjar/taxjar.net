using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Taxjar
{
    public interface ITaxjarApi
    {
        string apiToken { get; set; }


        string apiUrl { get; set; }

        IDictionary<string, string> headers { get; set; }

        /// <summary>
        /// Milliseconds.
        /// </summary>
        int timeout { get; set; }

        List<Category> Categories();

        Task<List<Category>> CategoriesAsync(CancellationToken cancellationToken = default);

        CustomerResponseAttributes CreateCustomer(object parameters);

        Task<CustomerResponseAttributes> CreateCustomerAsync(object parameters, CancellationToken cancellationToken = default);

        OrderResponseAttributes CreateOrder(object parameters);

        Task<OrderResponseAttributes> CreateOrderAsync(object parameters, CancellationToken cancellationToken = default);

        RefundResponseAttributes CreateRefund(object parameters);

        Task<RefundResponseAttributes> CreateRefundAsync(object parameters, CancellationToken cancellationToken = default);

        CustomerResponseAttributes DeleteCustomer(string customerId);

        Task<CustomerResponseAttributes> DeleteCustomerAsync(string customerId, CancellationToken cancellationToken = default);

        OrderResponseAttributes DeleteOrder(string transactionId, object parameters = null);

        Task<OrderResponseAttributes> DeleteOrderAsync(string transactionId, object parameters = null, CancellationToken cancellationToken = default);

        RefundResponseAttributes DeleteRefund(string transactionId, object parameters = null);

        Task<RefundResponseAttributes> DeleteRefundAsync(string transactionId, object parameters = null, CancellationToken cancellationToken = default);

        object GetApiConfig(string key);

        List<string> ListCustomers(object parameters = null);

        Task<List<string>> ListCustomersAsync(object parameters = null, CancellationToken cancellationToken = default);

        List<string> ListOrders(object parameters = null);

        Task<List<string>> ListOrdersAsync(object parameters = null, CancellationToken cancellationToken = default);

        List<string> ListRefunds(object parameters);

        Task<List<string>> ListRefundsAsync(object parameters, CancellationToken cancellationToken = default);

        List<NexusRegion> NexusRegions();

        Task<List<NexusRegion>> NexusRegionsAsync(CancellationToken cancellationToken = default);

        RateResponseAttributes RatesForLocation(string zip, object parameters = null);

        Task<RateResponseAttributes> RatesForLocationAsync(string zip, object parameters = null, CancellationToken cancellationToken = default);

        void SetApiConfig(string key, object value);

        CustomerResponseAttributes ShowCustomer(string customerId);

        Task<CustomerResponseAttributes> ShowCustomerAsync(string customerId, CancellationToken cancellationToken = default);

        OrderResponseAttributes ShowOrder(string transactionId, object parameters = null);

        Task<OrderResponseAttributes> ShowOrderAsync(string transactionId, object parameters = null, CancellationToken cancellationToken = default);

        RefundResponseAttributes ShowRefund(string transactionId, object parameters = null);

        Task<RefundResponseAttributes> ShowRefundAsync(string transactionId, object parameters = null, CancellationToken cancellationToken = default);

        List<SummaryRate> SummaryRates();

        Task<List<SummaryRate>> SummaryRatesAsync(CancellationToken cancellationToken = default);

        TaxResponseAttributes TaxForOrder(object parameters);

        Task<TaxResponseAttributes> TaxForOrderAsync(object parameters, CancellationToken cancellationToken = default);

        CustomerResponseAttributes UpdateCustomer(object parameters);

        Task<CustomerResponseAttributes> UpdateCustomerAsync(object parameters, CancellationToken cancellationToken = default);

        OrderResponseAttributes UpdateOrder(object parameters);

        Task<OrderResponseAttributes> UpdateOrderAsync(object parameters, CancellationToken cancellationToken = default);

        RefundResponseAttributes UpdateRefund(object parameters);

        Task<RefundResponseAttributes> UpdateRefundAsync(object parameters, CancellationToken cancellationToken = default);

        List<Address> ValidateAddress(object parameters);

        Task<List<Address>> ValidateAddressAsync(object parameters, CancellationToken cancellationToken = default);

        ValidationResponseAttributes ValidateVat(object parameters);

        Task<ValidationResponseAttributes> ValidateVatAsync(object parameters, CancellationToken cancellationToken = default);
    }
}
