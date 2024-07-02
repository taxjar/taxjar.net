using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Taxjar;

public record TaxjarApiOptions : IEquatable<TaxjarApiOptions>
{
    public JsonSerializerOptions JsonSerializerOptions {get; set;} = TaxjarConstants.TaxJarDefaultSerializationOptions;
    [Required]
    public string ApiToken { get; set; } = string.Empty;
    public string ApiUrl { get; set; } = TaxjarConstants.DefaultApiUrl;
    public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    public TimeSpan Timeout { get; set; } = TimeSpan.FromMilliseconds(TaxjarConstants.TimeoutInMilliseconds);
    public bool UseSandbox { get; set; } = false;
    public string ApiVersion { get; set; } = TaxjarConstants.ApiVersion;

    public static string SectionName => nameof(Taxjar);
}


