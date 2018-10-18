using System;
using System.Collections.Generic;
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
                    request.AddJsonBody(body);

                    if (method == Method.GET)
                    {
                        foreach (var prop in body.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                        {
                            request.AddQueryParameter(prop.Name, prop.GetValue(body).ToString());
                        }
                    }
                }
                else
                {
                    request.AddParameter("application/json", JsonConvert.SerializeObject(body), ParameterType.RequestBody);

                    if (method == Method.GET)
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

            return response.Data;
        }

        protected virtual async Task<T> SendRequestAsync<T>(string endpoint, object body = null, Method httpMethod = Method.POST) where T : new()
        {
            var request = CreateRequest(endpoint, httpMethod, body);
            var response = await apiClient.ExecuteTaskAsync<T>(request);

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

            return response.Data;
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

        public virtual OrderResponseAttributes ShowOrder(string transactionId)
        {
            var response = SendRequest<OrderResponse>("transactions/orders/" + transactionId, null, Method.GET);
            return response.Order;
        }

        public virtual OrderResponseAttributes CreateOrder(object parameters)
        {
            var response = SendRequest<OrderResponse>("transactions/orders", parameters, Method.POST);
            return response.Order;
        }

        public virtual OrderResponseAttributes UpdateOrder(object parameters)
        {
            var transactionId = parameters.GetType().GetProperty("transaction_id").GetValue(parameters).ToString();
            var response = SendRequest<OrderResponse>("transactions/orders/" + transactionId, parameters, Method.PUT);
            return response.Order;
        }

        public virtual OrderResponseAttributes DeleteOrder(string transactionId)
        {
            var response = SendRequest<OrderResponse>("transactions/orders/" + transactionId, null, Method.DELETE);
            return response.Order;
        }

        public virtual List<String> ListRefunds(object parameters)
        {
            var response = SendRequest<RefundsResponse>("transactions/refunds", parameters, Method.GET);
            return response.Refunds;
        }

        public virtual RefundResponseAttributes ShowRefund(string transactionId)
        {
            var response = SendRequest<RefundResponse>("transactions/refunds/" + transactionId, null, Method.GET);
            return response.Refund;
        }

        public virtual RefundResponseAttributes CreateRefund(object parameters)
        {
            var response = SendRequest<RefundResponse>("transactions/refunds", parameters, Method.POST);
            return response.Refund;
        }

        public virtual RefundResponseAttributes UpdateRefund(object parameters)
        {
            var transactionId = parameters.GetType().GetProperty("transaction_id").GetValue(parameters).ToString();
            var response = SendRequest<RefundResponse>("transactions/refunds/" + transactionId, parameters, Method.PUT);
            return response.Refund;
        }

        public virtual RefundResponseAttributes DeleteRefund(string transactionId)
        {
            var response = SendRequest<RefundResponse>("transactions/refunds/" + transactionId, null, Method.DELETE);
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
            var customerId = parameters.GetType().GetProperty("customer_id").GetValue(parameters).ToString();
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

        public virtual async Task<CategoriesResponse> CategoriesAsync()
        {
            return await SendRequestAsync<CategoriesResponse>("categories", null, Method.GET);
        }

        public virtual async Task<RateResponse> RatesForLocationAsync(string zip, object parameters = null)
        {
            return await SendRequestAsync<RateResponse>("rates/" + zip, parameters, Method.GET);
        }

        public virtual async Task<TaxResponse> TaxForOrderAsync(object parameters)
        {
            return await SendRequestAsync<TaxResponse>("taxes", parameters, Method.POST);
        }

        public virtual async Task<OrdersResponse> ListOrdersAsync(object parameters = null)
        {
            return await SendRequestAsync<OrdersResponse>("transactions/orders", parameters, Method.GET);
        }

        public virtual async Task<OrderResponse> ShowOrderAsync(string transactionId)
        {
            return await SendRequestAsync<OrderResponse>("transactions/orders/" + transactionId, null, Method.GET);
        }

        public virtual async Task<OrderResponse> CreateOrderAsync(object parameters)
        {
            return await SendRequestAsync<OrderResponse> ("transactions/orders", parameters, Method.POST);
        }

        public virtual async Task<OrderResponse> UpdateOrderAsync(object parameters)
        {
            var transactionId = parameters.GetType().GetProperty("transaction_id").GetValue(parameters).ToString();
            return await SendRequestAsync<OrderResponse>("transactions/orders/" + transactionId, parameters, Method.PUT);
        }

        public virtual async Task<OrderResponse> DeleteOrderAsync(string transactionId)
        {
            return await SendRequestAsync<OrderResponse>("transactions/orders/" + transactionId, null, Method.DELETE);
        }

        public virtual async Task<RefundsResponse> ListRefundsAsync(object parameters)
        {
            return await SendRequestAsync<RefundsResponse>("transactions/refunds", parameters, Method.GET);
        }

        public virtual async Task<RefundResponse> ShowRefundAsync(string transactionId)
        {
            return await SendRequestAsync<RefundResponse>("transactions/refunds/" + transactionId, null, Method.GET);
        }

        public virtual async Task<RefundResponse> CreateRefundAsync(object parameters)
        {
            return await SendRequestAsync<RefundResponse>("transactions/refunds", parameters, Method.POST);
        }

        public virtual async Task<RefundResponse> UpdateRefundAsync(object parameters)
        {
            var transactionId = parameters.GetType().GetProperty("transaction_id").GetValue(parameters).ToString();
            return await SendRequestAsync<RefundResponse>("transactions/refunds/" + transactionId, parameters, Method.PUT);
        }

        public virtual async Task<RefundResponse> DeleteRefundAsync(string transactionId)
        {
            return await SendRequestAsync<RefundResponse>("transactions/refunds/" + transactionId, null, Method.DELETE);
        }

        public virtual async Task<CustomersResponse> ListCustomersAsync(object parameters = null)
        {
            return await SendRequestAsync<CustomersResponse>("customers", parameters, Method.GET);
        }

        public virtual async Task<CustomerResponse> ShowCustomerAsync(string customerId)
        {
            return await SendRequestAsync<CustomerResponse>("customers/" + customerId, null, Method.GET);
        }

        public virtual async Task<CustomerResponse> CreateCustomerAsync(object parameters)
        {
            return await SendRequestAsync<CustomerResponse>("customers", parameters, Method.POST);
        }

        public virtual async Task<CustomerResponse> UpdateCustomerAsync(object parameters)
        {
            var customerId = parameters.GetType().GetProperty("customer_id").GetValue(parameters).ToString();
            return await SendRequestAsync<CustomerResponse>("customers/" + customerId, parameters, Method.PUT);
        }

        public virtual async Task<CustomerResponse> DeleteCustomerAsync(string customerId)
        {
            return await SendRequestAsync<CustomerResponse>("customers/" + customerId, null, Method.DELETE);
        }

        public virtual async Task<NexusRegionsResponse> NexusRegionsAsync()
        {
            return await SendRequestAsync<NexusRegionsResponse>("nexus/regions", null, Method.GET);
        }

        public virtual async Task<AddressValidationResponse> ValidateAddressAsync(object parameters)
        {
            return await SendRequestAsync<AddressValidationResponse>("addresses/validate", parameters, Method.POST);
        }

        public virtual async Task<ValidationResponse> ValidateVatAsync(object parameters)
        {
            return await SendRequestAsync<ValidationResponse>("validation", parameters, Method.GET);
        }

        public virtual async Task<SummaryRatesResponse> SummaryRatesAsync()
        {
            return await SendRequestAsync<SummaryRatesResponse>("summary_rates", null, Method.GET);
        }
    }
}
