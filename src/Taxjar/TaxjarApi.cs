using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

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

        protected virtual IRestRequest CreateRequest(string action, Method method = Method.POST)
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

            request.Timeout = this.timeout * 1000;

            return request;
        }

        protected virtual string SendRequest(string endpoint, object body = null, Method httpMethod = Method.POST)
        {
            var req = CreateRequest(endpoint, httpMethod);

            if (body != null)
            {
                if (IsAnonymousType(body.GetType()))
                {
                    req.AddJsonBody(body);

                    if (httpMethod == Method.GET)
                    {
                        foreach (var prop in body.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                        {
                            req.AddQueryParameter(prop.Name, prop.GetValue(body).ToString());
                        }
                    }
                }
                else
                {
                    req.AddParameter("application/json", JsonConvert.SerializeObject(body), ParameterType.RequestBody);

                    if (httpMethod == Method.GET)
                    {
                        body = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(body));

                        foreach (var prop in JObject.FromObject(body).Properties())
                        {
                            req.AddQueryParameter(prop.Name, prop.Value.ToString());
                        }    
                    }
                }   
            }

            var res = this.apiClient.Execute(req);

            if ((int) res.StatusCode >= 400)
            {
                var taxjarError = JsonConvert.DeserializeObject<TaxjarError>(res.Content);
                var errorMessage = taxjarError.Error + " - " + taxjarError.Detail;
                throw new TaxjarException(res.StatusCode, taxjarError, errorMessage);                
            }

            if (res.ErrorException != null)
            {
                throw new Exception(res.ErrorMessage, res.ErrorException);
            }

            return res.Content;
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
            var res = SendRequest("categories", null, Method.GET);
            var categoryRequest = JsonConvert.DeserializeObject<CategoriesResponse>(res);
            return categoryRequest.Categories;
        }

        public virtual RateResponseAttributes RatesForLocation(string zip, object parameters = null)
        {
            var res = SendRequest("rates/" + zip, parameters, Method.GET);
            var rateRequest = JsonConvert.DeserializeObject<RateResponse>(res);
            return rateRequest.Rate;
        }

        public virtual TaxResponseAttributes TaxForOrder(object parameters)
        {
            var res = SendRequest("taxes", parameters, Method.POST);
            var taxRequest = JsonConvert.DeserializeObject<TaxResponse>(res);
            return taxRequest.Tax;
        }

        public virtual List<String> ListOrders(object parameters = null)
        {
            var res = SendRequest("transactions/orders", parameters, Method.GET);
            var ordersRequest = JsonConvert.DeserializeObject<OrdersResponse>(res);
            return ordersRequest.Orders;
        }

        public virtual OrderResponseAttributes ShowOrder(string transactionId)
        {
            var res = SendRequest("transactions/orders/" + transactionId, null, Method.GET);
            var orderRequest = JsonConvert.DeserializeObject<OrderResponse>(res);
            return orderRequest.Order;
        }

        public virtual OrderResponseAttributes CreateOrder(object parameters)
        {
            var res = SendRequest("transactions/orders", parameters, Method.POST);
            var orderRequest = JsonConvert.DeserializeObject<OrderResponse>(res);
            return orderRequest.Order;
        }

        public virtual OrderResponseAttributes UpdateOrder(object parameters)
        {
            var transactionId = parameters.GetType().GetProperty("transaction_id").GetValue(parameters).ToString();
            var res = SendRequest("transactions/orders/" + transactionId, parameters, Method.PUT);
            var orderRequest = JsonConvert.DeserializeObject<OrderResponse>(res);
            return orderRequest.Order;
        }

        public virtual OrderResponseAttributes DeleteOrder(string transactionId)
        {
            var res = SendRequest("transactions/orders/" + transactionId, null, Method.DELETE);
            var orderRequest = JsonConvert.DeserializeObject<OrderResponse>(res);
            return orderRequest.Order;
        }

        public virtual List<String> ListRefunds(object parameters)
        {
            var res = SendRequest("transactions/refunds", parameters, Method.GET);
            var refundsRequest = JsonConvert.DeserializeObject<RefundsResponse>(res);
            return refundsRequest.Refunds;
        }

        public virtual RefundResponseAttributes ShowRefund(string transactionId)
        {
            var res = SendRequest("transactions/refunds/" + transactionId, null, Method.GET);
            var refundRequest = JsonConvert.DeserializeObject<RefundResponse>(res);
            return refundRequest.Refund;
        }

        public virtual RefundResponseAttributes CreateRefund(object parameters)
        {
            var res = SendRequest("transactions/refunds", parameters, Method.POST);
            var refundRequest = JsonConvert.DeserializeObject<RefundResponse>(res);
            return refundRequest.Refund;
        }

        public virtual RefundResponseAttributes UpdateRefund(object parameters)
        {
            var transactionId = parameters.GetType().GetProperty("transaction_id").GetValue(parameters).ToString();
            var res = SendRequest("transactions/refunds/" + transactionId, parameters, Method.PUT);
            var refundRequest = JsonConvert.DeserializeObject<RefundResponse>(res);
            return refundRequest.Refund;
        }

        public virtual RefundResponseAttributes DeleteRefund(string transactionId)
        {
            var res = SendRequest("transactions/refunds/" + transactionId, null, Method.DELETE);
            var refundRequest = JsonConvert.DeserializeObject<RefundResponse>(res);
            return refundRequest.Refund;
        }

        public virtual List<String> ListCustomers(object parameters = null)
        {
            var res = SendRequest("customers", parameters, Method.GET);
            var customersRequest = JsonConvert.DeserializeObject<CustomersResponse>(res);
            return customersRequest.Customers;
        }

        public virtual CustomerResponseAttributes ShowCustomer(string customerId)
        {
            var res = SendRequest("customers/" + customerId, null, Method.GET);
            var customerRequest = JsonConvert.DeserializeObject<CustomerResponse>(res);
            return customerRequest.Customer;
        }

        public virtual CustomerResponseAttributes CreateCustomer(object parameters)
        {
            var res = SendRequest("customers", parameters, Method.POST);
            var customerRequest = JsonConvert.DeserializeObject<CustomerResponse>(res);
            return customerRequest.Customer;
        }

        public virtual CustomerResponseAttributes UpdateCustomer(object parameters)
        {
            var customerId = parameters.GetType().GetProperty("customer_id").GetValue(parameters).ToString();
            var res = SendRequest("customers/" + customerId, parameters, Method.PUT);
            var customerRequest = JsonConvert.DeserializeObject<CustomerResponse>(res);
            return customerRequest.Customer;
        }

        public virtual CustomerResponseAttributes DeleteCustomer(string customerId)
        {
            var res = SendRequest("customers/" + customerId, null, Method.DELETE);
            var customerRequest = JsonConvert.DeserializeObject<CustomerResponse>(res);
            return customerRequest.Customer;
        }

        public virtual List<NexusRegion> NexusRegions()
        {
            var res = SendRequest("nexus/regions", null, Method.GET);
            var nexusRegionsRequest = JsonConvert.DeserializeObject<NexusRegionsResponse>(res);
            return nexusRegionsRequest.Regions;
        }

        public virtual ValidationResponseAttributes Validate(object parameters)
        {
            var res = SendRequest("validation", parameters, Method.GET);
            var validationRequest = JsonConvert.DeserializeObject<ValidationResponse>(res);
            return validationRequest.Validation;
        }

        public virtual List<SummaryRate> SummaryRates()
        {
            var res = SendRequest("summary_rates", null, Method.GET);
            var summaryRatesRequest = JsonConvert.DeserializeObject<SummaryRatesResponse>(res);
            return summaryRatesRequest.SummaryRates;
        }
    }
}
