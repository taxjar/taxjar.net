#if NETSTANDARD2_0
using System;

namespace Taxjar.Options
{
    public class TaxjarOptions
    {

        /// <summary>
        /// Taxjar token.
        /// </summary>
        public string ApiToken { get; set; }

        /// <summary>
        /// Is Sandbox?
        /// </summary>
        public bool IsSandBox { get; set; }

        /// <summary>
        /// Timeout for the HttpClient.
        /// </summary>
        public TimeSpan Timeout { get; set; }
    }
}
#endif
