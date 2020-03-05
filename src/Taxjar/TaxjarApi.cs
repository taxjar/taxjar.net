using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestRequest = RestSharp.RestRequest;

namespace Taxjar
{
    public static class TaxjarConstants
    {
        public const string DefaultApiUrl = "https://api.taxjar.com";
        public const string SandboxApiUrl = "https://api.sandbox.taxjar.com";
        public const string ApiVersion = "v2";
    }

    public class TaxjarApi
    {
        internal RestClient apiClient;
        public string apiToken { get; set; }
        public string apiUrl { get; set; }
        public IDictionary<string, string> headers { get; set; }
        public int timeout { get; set; }

        public TaxjarApi(string token, object parameters = null)
        {
            apiToken = token;
            apiUrl = TaxjarConstants.DefaultApiUrl + "/" + TaxjarConstants.ApiVersion + "/";
            headers = new Dictionary<string, string>();
            timeout = 0; // Milliseconds

            if (parameters != null)
            {
                if (parameters.GetType().GetProperty("apiUrl") != null)
                {
                    apiUrl = parameters.GetType().GetProperty("apiUrl").GetValue(parameters).ToString();
                    apiUrl += "/" + TaxjarConstants.ApiVersion + "/";
                }

                if (parameters.GetType().GetProperty("headers") != null)
                {
                    headers = (IDictionary<string, string>) parameters.GetType().GetProperty("headers").GetValue(parameters);
                }

                if (parameters.GetType().GetProperty("timeout") != null)
                {
                    timeout = (int) parameters.GetType().GetProperty("timeout").GetValue(parameters);
                }
            }

            if (string.IsNullOrWhiteSpace(apiToken))
            {
                throw new ArgumentException("Please provide a TaxJar API key.", nameof(apiToken));
            }

            apiClient = new RestClient(apiUrl);
        }

        public virtual void SetApiConfig(string key, object value)
        {
            if (key == "apiUrl")
            {
                value += "/" + TaxjarConstants.ApiVersion + "/";
                apiClient = new RestClient(value.ToString());
            }

            GetType().GetProperty(key).SetValue(this, value, null);
        }

        public virtual object GetApiConfig(string key)
        {
            return GetType().GetProperty(key).GetValue(this);
        }

        protected virtual RestRequest CreateRequest(string action, Method method = Method.POST, object body = null)
        {
            var request = new RestRequest(action, method)
            {
                RequestFormat = DataFormat.Json
            };
            var includeBody = new[] {Method.POST, Method.PUT, Method.PATCH}.Contains(method);

            request.AddHeader("Authorization", "Bearer " + apiToken);

            foreach (var header in headers)
            {
                request.AddHeader(header.Key, header.Value);
            }

            request.Timeout = timeout;

            if (body != null)
            {
                if (IsAnonymousType(body.GetType()))
                {
                    if (includeBody)
                    {
                        request.AddJsonBody(body);
                    }
                    else
                    {
                        foreach (var prop in body.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                        {
                            request.AddQueryParameter(prop.Name, prop.GetValue(body).ToString());
                        }
                    }
                }
                else
                {
                    if (includeBody)
                    {
                        request.AddParameter("application/json", JsonConvert.SerializeObject(body), ParameterType.RequestBody);
                    }
                    else
                    {
                        body = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(body));

                        foreach (var prop in JObject.FromObject(body).Properties())
                        {
                            request.AddQueryParameter(prop.Name, prop.Value.ToString());
                        }
                    }
                }
            }

            return request;
        }

        protected virtual T SendRequest<T>(string endpoint, object body = null, Method httpMethod = Method.POST) where T : new()
        {
            var request = CreateRequest(endpoint, httpMethod, body);
            var response = apiClient.Execute<T>(request);

            if ((int)response.StatusCode >= 400)
            {
                var taxjarError = JsonConvert.DeserializeObject<TaxjarError>(response.Content);
                var errorMessage = taxjarError.Error + " - " + taxjarError.Detail;
                throw new TaxjarException(response.StatusCode, taxjarError, errorMessage);
            }

            if (response.ErrorException != null)
            {
                throw new Exception(response.ErrorMessage, response.ErrorException);
            }

            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        protected virtual async Task<T> SendRequestAsync<T>(string endpoint, object body = null, Method httpMethod = Method.POST) where T : new()
        {
            var request = CreateRequest(endpoint, httpMethod, body);
            var response = await apiClient.ExecuteAsync<T>(request).ConfigureAwait(false);

            if ((int)response.StatusCode >= 400)
            {
                var taxjarError = JsonConvert.DeserializeObject<TaxjarError>(response.Content);
                var errorMessage = taxjarError.Error + " - " + taxjarError.Detail;
                throw new TaxjarException(response.StatusCode, taxjarError, errorMessage);
            }

            if (response.ErrorException != null)
            {
                throw new Exception(response.ErrorMessage, response.ErrorException);
            }

            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        protected virtual bool IsAnonymousType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                && type.IsGenericType && type.Name.Contains("AnonymousType")
                && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }

        public virtual List<Category> Categories()
        {
            var response = SendRequest<CategoriesResponse>("categories", null, Method.GET);
            return response.Categories;
        }

        public virtual RateResponseAttributes RatesForLocation(string zip, object parameters = null)
        {
            var response = SendRequest<RateResponse>("rates/" + zip, parameters, Method.GET);
            return response.Rate;
        }

        public virtual TaxResponseAttributes TaxForOrder(object parameters)
        {
            var response = SendRequest<TaxResponse>("taxes", parameters, Method.POST);
            return response.Tax;
        }

        public virtual List<String> ListOrders(object parameters = null)
        {
            var response = SendRequest<OrdersResponse>("transactions/orders", parameters, Method.GET);
            return response.Orders;
        }

        public virtual OrderResponseAttributes ShowOrder(string transactionId, object parameters = null)
        {
            var response = SendRequest<OrderResponse>("transactions/orders/" + transactionId, parameters, Method.GET);
            return response.Order;
        }

        public virtual OrderResponseAttributes CreateOrder(object parameters)
        {
            var response = SendRequest<OrderResponse>("transactions/orders", parameters, Method.POST);
            return response.Order;
        }

        public virtual OrderResponseAttributes UpdateOrder(object parameters)
        {
            var transactionIdProp = parameters.GetType().GetProperty("transaction_id") ?? parameters.GetType().GetProperty("TransactionId");

            if (transactionIdProp == null)
            {
                throw new Exception("Missing transaction ID for `UpdateOrder`");
            }

            var transactionId = transactionIdProp.GetValue(parameters).ToString();
            var response = SendRequest<OrderResponse>("transactions/orders/" + transactionId, parameters, Method.PUT);
            return response.Order;
        }

        public virtual OrderResponseAttributes DeleteOrder(string transactionId, object parameters = null)
        {
            var response = SendRequest<OrderResponse>("transactions/orders/" + transactionId, parameters, Method.DELETE);
            return response.Order;
        }

        public virtual List<String> ListRefunds(object parameters)
        {
            var response = SendRequest<RefundsResponse>("transactions/refunds", parameters, Method.GET);
            return response.Refunds;
        }

        public virtual RefundResponseAttributes ShowRefund(string transactionId, object parameters = null)
        {
            var response = SendRequest<RefundResponse>("transactions/refunds/" + transactionId, parameters, Method.GET);
            return response.Refund;
        }

        public virtual RefundResponseAttributes CreateRefund(object parameters)
        {
            var response = SendRequest<RefundResponse>("transactions/refunds", parameters, Method.POST);
            return response.Refund;
        }

        public virtual RefundResponseAttributes UpdateRefund(object parameters)
        {
            var transactionIdProp = parameters.GetType().GetProperty("transaction_id") ?? parameters.GetType().GetProperty("TransactionId");

            if (transactionIdProp == null) {
                throw new Exception("Missing transaction ID for `UpdateRefund`");
            }

            var transactionId = transactionIdProp.GetValue(parameters).ToString();
            var response = SendRequest<RefundResponse>("transactions/refunds/" + transactionId, parameters, Method.PUT);
            return response.Refund;
        }

        public virtual RefundResponseAttributes DeleteRefund(string transactionId, object parameters = null)
        {
            var response = SendRequest<RefundResponse>("transactions/refunds/" + transactionId, parameters, Method.DELETE);
            return response.Refund;
        }

        public virtual List<String> ListCustomers(object parameters = null)
        {
            var response = SendRequest<CustomersResponse>("customers", parameters, Method.GET);
            return response.Customers;
        }

        public virtual CustomerResponseAttributes ShowCustomer(string customerId)
        {
            var response = SendRequest<CustomerResponse>("customers/" + customerId, null, Method.GET);
            return response.Customer;
        }

        public virtual CustomerResponseAttributes CreateCustomer(object parameters)
        {
            var response = SendRequest<CustomerResponse>("customers", parameters, Method.POST);
            return response.Customer;
        }

        public virtual CustomerResponseAttributes UpdateCustomer(object parameters)
        {
            var customerIdProp = parameters.GetType().GetProperty("customer_id") ?? parameters.GetType().GetProperty("CustomerId");

            if (customerIdProp == null)
            {
                throw new Exception("Missing customer ID for `UpdateCustomer`");
            }

            var customerId = customerIdProp.GetValue(parameters).ToString();
            var response = SendRequest<CustomerResponse>("customers/" + customerId, parameters, Method.PUT);
            return response.Customer;
        }

        public virtual CustomerResponseAttributes DeleteCustomer(string customerId)
        {
            var response = SendRequest<CustomerResponse>("customers/" + customerId, null, Method.DELETE);
            return response.Customer;
        }

        public virtual List<NexusRegion> NexusRegions()
        {
            var response = SendRequest<NexusRegionsResponse>("nexus/regions", null, Method.GET);
            return response.Regions;
        }

        public virtual List<Address> ValidateAddress(object parameters)
        {
            var response = SendRequest<AddressValidationResponse>("addresses/validate", parameters, Method.POST);
            return response.Addresses;
        }

        public virtual ValidationResponseAttributes ValidateVat(object parameters)
        {
            var response = SendRequest<ValidationResponse>("validation", parameters, Method.GET);
            return response.Validation;
        }

        public virtual List<SummaryRate> SummaryRates()
        {
            var response = SendRequest<SummaryRatesResponse>("summary_rates", null, Method.GET);
            return response.SummaryRates;
        }

        public virtual async Task<List<Category>> CategoriesAsync()
        {
            var response = await SendRequestAsync<CategoriesResponse>("categories", null, Method.GET).ConfigureAwait(false);
            return response.Categories;
        }

        public virtual async Task<RateResponseAttributes> RatesForLocationAsync(string zip, object parameters = null)
        {
            var response = await SendRequestAsync<RateResponse>("rates/" + zip, parameters, Method.GET).ConfigureAwait(false);
            return response.Rate;
        }

        public virtual async Task<TaxResponseAttributes> TaxForOrderAsync(object parameters)
        {
            var response = await SendRequestAsync<TaxResponse>("taxes", parameters, Method.POST).ConfigureAwait(false);
            return response.Tax;
        }

        public virtual async Task<List<string>> ListOrdersAsync(object parameters = null)
        {
            var response = await SendRequestAsync<OrdersResponse>("transactions/orders", parameters, Method.GET).ConfigureAwait(false);
            return response.Orders;
        }

        public virtual async Task<OrderResponseAttributes> ShowOrderAsync(string transactionId, object parameters = null)
        {
            var response = await SendRequestAsync<OrderResponse>("transactions/orders/" + transactionId, parameters, Method.GET).ConfigureAwait(false);
            return response.Order;
        }

        public virtual async Task<OrderResponseAttributes> CreateOrderAsync(object parameters)
        {
            var response = await SendRequestAsync<OrderResponse> ("transactions/orders", parameters, Method.POST).ConfigureAwait(false);
            return response.Order;
        }

        public virtual async Task<OrderResponseAttributes> UpdateOrderAsync(object parameters)
        {
            var transactionId = parameters.GetType().GetProperty("transaction_id").GetValue(parameters).ToString();
            var response = await SendRequestAsync<OrderResponse>("transactions/orders/" + transactionId, parameters, Method.PUT).ConfigureAwait(false);
            return response.Order;
        }

        public virtual async Task<OrderResponseAttributes> DeleteOrderAsync(string transactionId, object parameters = null)
        {
            var response = await SendRequestAsync<OrderResponse>("transactions/orders/" + transactionId, parameters, Method.DELETE).ConfigureAwait(false);
            return response.Order;
        }

        public virtual async Task<List<string>> ListRefundsAsync(object parameters)
        {
            var response = await SendRequestAsync<RefundsResponse>("transactions/refunds", parameters, Method.GET).ConfigureAwait(false);
            return response.Refunds;
        }

        public virtual async Task<RefundResponseAttributes> ShowRefundAsync(string transactionId, object parameters = null)
        {
            var response = await SendRequestAsync<RefundResponse>("transactions/refunds/" + transactionId, parameters, Method.GET).ConfigureAwait(false);
            return response.Refund;
        }

        public virtual async Task<RefundResponseAttributes> CreateRefundAsync(object parameters)
        {
            var response = await SendRequestAsync<RefundResponse>("transactions/refunds", parameters, Method.POST).ConfigureAwait(false);
            return response.Refund;
        }

        public virtual async Task<RefundResponseAttributes> UpdateRefundAsync(object parameters)
        {
            var transactionId = parameters.GetType().GetProperty("transaction_id").GetValue(parameters).ToString();
            var response = await SendRequestAsync<RefundResponse>("transactions/refunds/" + transactionId, parameters, Method.PUT).ConfigureAwait(false);
            return response.Refund;
        }

        public virtual async Task<RefundResponseAttributes> DeleteRefundAsync(string transactionId, object parameters = null)
        {
            var response = await SendRequestAsync<RefundResponse>("transactions/refunds/" + transactionId, parameters, Method.DELETE).ConfigureAwait(false);
            return response.Refund;
        }

        public virtual async Task<List<string>> ListCustomersAsync(object parameters = null)
        {
            var response = await SendRequestAsync<CustomersResponse>("customers", parameters, Method.GET).ConfigureAwait(false);
            return response.Customers;
        }

        public virtual async Task<CustomerResponseAttributes> ShowCustomerAsync(string customerId)
        {
            var response = await SendRequestAsync<CustomerResponse>("customers/" + customerId, null, Method.GET).ConfigureAwait(false);
            return response.Customer;
        }

        public virtual async Task<CustomerResponseAttributes> CreateCustomerAsync(object parameters)
        {
            var response = await SendRequestAsync<CustomerResponse>("customers", parameters, Method.POST).ConfigureAwait(false);
            return response.Customer;
        }

        public virtual async Task<CustomerResponseAttributes> UpdateCustomerAsync(object parameters)
        {
            var customerId = parameters.GetType().GetProperty("customer_id").GetValue(parameters).ToString();
            var response = await SendRequestAsync<CustomerResponse>("customers/" + customerId, parameters, Method.PUT).ConfigureAwait(false);
            return response.Customer;
        }

        public virtual async Task<CustomerResponseAttributes> DeleteCustomerAsync(string customerId)
        {
            var response = await SendRequestAsync<CustomerResponse>("customers/" + customerId, null, Method.DELETE).ConfigureAwait(false);
            return response.Customer;
        }

        public virtual async Task<List<NexusRegion>> NexusRegionsAsync()
        {
            var response = await SendRequestAsync<NexusRegionsResponse>("nexus/regions", null, Method.GET).ConfigureAwait(false);
            return response.Regions;
        }

        public virtual async Task<List<Address>> ValidateAddressAsync(object parameters)
        {
            var response = await SendRequestAsync<AddressValidationResponse>("addresses/validate", parameters, Method.POST).ConfigureAwait(false);
            return response.Addresses;
        }

        public virtual async Task<ValidationResponseAttributes> ValidateVatAsync(object parameters)
        {
            var response = await SendRequestAsync<ValidationResponse>("validation", parameters, Method.GET).ConfigureAwait(false);
            return response.Validation;
        }

        public virtual async Task<List<SummaryRate>> SummaryRatesAsync()
        {
            var response = await SendRequestAsync<SummaryRatesResponse>("summary_rates", null, Method.GET).ConfigureAwait(false);
            return response.SummaryRates;
        }
    }
}
