using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("TaxJar.Tests")]

namespace Taxjar.Infrastructure
{
    internal static class ErrorMessage
    {
        public const string MissingTransactionId = "Transaction ID cannot be null or an empty string.";
        public const string MissingCustomerId = "Customer ID cannot be null or an empty string.";
    }
}
