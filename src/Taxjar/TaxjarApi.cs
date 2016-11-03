using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Reflection;
using Newtonsoft.Json;
using RestSharp;

namespace Taxjar
{
	public static class TaxjarConstants
	{
		public const string ApiUrl = "https://api.taxjar.com/v2/";
	}

	public class TaxjarApi
	{
		internal RestClient apiClient;
		internal readonly string apiKey;
		internal readonly string apiUrl;

		public TaxjarApi(string apiKey = "", object parameters = null)
		{
			this.apiKey = !string.IsNullOrWhiteSpace(apiKey) ? apiKey : ConfigurationManager.AppSettings["TaxjarApiKey"];
			this.apiUrl = TaxjarConstants.ApiUrl;

			if (parameters != null)
			{
				if (parameters.GetType().GetProperty("apiUrl") != null)
				{
					this.apiUrl = parameters.GetType().GetProperty("apiUrl").GetValue(parameters, null).ToString();
				}
			}

			if (string.IsNullOrWhiteSpace(this.apiKey))
			{
				throw new ArgumentException("Please provide a TaxJar API key.", "apiKey");
			}

			this.apiClient = new RestClient(this.apiUrl);
		}

		protected virtual IRestRequest CreateRequest(string action, Method method = Method.POST)
		{
			var request = new RestRequest(action, method) {
				RequestFormat = DataFormat.Json
			};
			request.AddHeader("Authorization", "Bearer " + this.apiKey);
			return request;
		}

		public virtual string SendRequest(string endpoint, object body = null, Method httpMethod = Method.POST)
		{
			var req = CreateRequest(endpoint, httpMethod).AddJsonBody(body);

			if (httpMethod == Method.GET)
			{
				if (body != null)
				{
					foreach (var prop in body.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
					{
						req.AddQueryParameter(prop.Name, prop.GetValue(body, null).ToString());
					}
				}
			}

			var res = this.apiClient.Execute(req);

			if ((int)res.StatusCode >= 400)
			{
				var taxjarError = JsonConvert.DeserializeObject<TaxjarError>(res.Content);
				var errorMessage = taxjarError.Error + " - " + taxjarError.Detail;
				throw new TaxjarException(res.StatusCode, taxjarError, errorMessage);
			}

			return res.Content;
		}

		public virtual List<Category> Categories()
		{
			var res = SendRequest("categories", null, Method.GET);
			var categoryRequest = JsonConvert.DeserializeObject<CategoriesRequest>(res);
			return categoryRequest.Categories;
		}

		public virtual Rate RatesForLocation(string zip, object parameters = null)
		{
			var res = SendRequest("rates/" + zip, parameters, Method.GET);
			var rateRequest = JsonConvert.DeserializeObject<RateRequest>(res);
			return rateRequest.Rate;
		}

		public virtual Tax TaxForOrder(object parameters)
		{
			var res = SendRequest("taxes", parameters, Method.POST);
			var taxRequest = JsonConvert.DeserializeObject<TaxRequest>(res);
			return taxRequest.Tax;
		}

		public virtual List<String> ListOrders(object parameters = null)
		{
			var res = SendRequest("transactions/orders", parameters, Method.GET);
			var ordersRequest = JsonConvert.DeserializeObject<OrdersRequest>(res);
			return ordersRequest.Orders;
		}

		public virtual Order ShowOrder(int transactionId)
		{
			var res = SendRequest("transactions/orders/" + transactionId, null, Method.GET);
			var orderRequest = JsonConvert.DeserializeObject<OrderRequest>(res);
			return orderRequest.Order;
		}

		public virtual Order CreateOrder(object parameters)
		{
			var res = SendRequest("transactions/orders", parameters, Method.POST);
			var orderRequest = JsonConvert.DeserializeObject<OrderRequest>(res);
			return orderRequest.Order;
		}

		public virtual Order UpdateOrder(object parameters)
		{
			var transactionId = parameters.GetType().GetProperty("transaction_id").GetValue(parameters, null).ToString();
			var res = SendRequest("transactions/orders/" + transactionId, parameters, Method.PUT);
			var orderRequest = JsonConvert.DeserializeObject<OrderRequest>(res);
			return orderRequest.Order;
		}

		public virtual Order DeleteOrder(int transactionId)
		{
			var res = SendRequest("transactions/orders/" + transactionId, null, Method.DELETE);
			var orderRequest = JsonConvert.DeserializeObject<OrderRequest>(res);
			return orderRequest.Order;
		}

		public virtual List<String> ListRefunds(object parameters)
		{
			var res = SendRequest("transactions/refunds", parameters, Method.GET);
			var refundsRequest = JsonConvert.DeserializeObject<RefundsRequest>(res);
			return refundsRequest.Refunds;
		}

		public virtual Refund ShowRefund(int transactionId)
		{
			var res = SendRequest("transactions/refunds/" + transactionId, null, Method.GET);
			var refundRequest = JsonConvert.DeserializeObject<RefundRequest>(res);
			return refundRequest.Refund;
		}

		public virtual Refund CreateRefund(object parameters)
		{
			var res = SendRequest("transactions/refunds", parameters, Method.POST);
			var refundRequest = JsonConvert.DeserializeObject<RefundRequest>(res);
			return refundRequest.Refund;
		}

		public virtual Refund UpdateRefund(object parameters)
		{
			var transactionId = parameters.GetType().GetProperty("transaction_id").GetValue(parameters, null).ToString();
			var res = SendRequest("transactions/refunds/" + transactionId, parameters, Method.PUT);
			var refundRequest = JsonConvert.DeserializeObject<RefundRequest>(res);
			return refundRequest.Refund;
		}

		public virtual Refund DeleteRefund(int transactionId)
		{
			var res = SendRequest("transactions/refunds/" + transactionId, null, Method.DELETE);
			var refundRequest = JsonConvert.DeserializeObject<RefundRequest>(res);
			return refundRequest.Refund;
		}

		public virtual List<NexusRegion> NexusRegions()
		{
			var res = SendRequest("nexus/regions", null, Method.GET);
			var nexusRegionsRequest = JsonConvert.DeserializeObject<NexusRegionsRequest>(res);
			return nexusRegionsRequest.Regions;
		}

		public virtual Validation Validate(object parameters)
		{
			var res = SendRequest("validation", parameters, Method.GET);
			var validationRequest = JsonConvert.DeserializeObject<ValidationRequest>(res);
			return validationRequest.Validation;
		}

		public virtual List<SummaryRate> SummaryRates()
		{
			var res = SendRequest("summary_rates", null, Method.GET);
			var summaryRatesRequest = JsonConvert.DeserializeObject<SummaryRatesRequest>(res);
			return summaryRatesRequest.SummaryRates;
		}
	}
}
