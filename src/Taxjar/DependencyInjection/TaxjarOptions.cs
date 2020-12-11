#if NETSTANDARD2_0
using System;

namespace Taxjar.Options
{
    /// <summary>
    /// The options for <see cref="ITaxjarApi"/> client.
    /// </summary>
    public class TaxjarOptions
    {
        /// <summary>
        /// Taxjar token.
        /// </summary>
        public string ApiToken { get; set; }

        /// <summary>
        /// The base url used by <see cref="TaxjarApi"/> client.
        /// </summary>
        public string ApiUrl { get; set; } = TaxjarConstants.DefaultApiUrl;


        /// <summary>
        /// Timeout in seconds for the HttpClient.
        /// </summary>
        public int Timeout { get; set; }
    }
}
#endif
