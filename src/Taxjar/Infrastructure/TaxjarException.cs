using System;
using System.Net;
using Newtonsoft.Json;

namespace Taxjar
{
	[Serializable]
	public class TaxjarException : ApplicationException
	{
		public HttpStatusCode HttpStatusCode { get; set; }
		public string HttpStatusDescription { get; set; }
		public object Response { get; set; }

		public TaxjarException()
		{
		}

		public TaxjarException(HttpStatusCode statusCode, string statusDescription, string res)
		{
			HttpStatusCode = statusCode;
			HttpStatusDescription = statusDescription;
			Response = JsonConvert.DeserializeObject(res);
		}
	}
}

