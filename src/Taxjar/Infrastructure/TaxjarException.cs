using System;
using System.Net;

namespace Taxjar
{
	[Serializable]
	public class TaxjarException : ApplicationException
	{
		public HttpStatusCode HttpStatusCode { get; set; }
		public TaxjarError TaxjarError { get; set; }

		public TaxjarException()
		{
		}

		public TaxjarException(HttpStatusCode statusCode, TaxjarError taxjarError, string message) : base(message)
		{
			HttpStatusCode = statusCode;
			TaxjarError = taxjarError;
		}
	}
}

