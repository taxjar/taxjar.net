using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Taxjar
{
    public record CategoriesResponse
    {
        [JsonPropertyName("categories")]
        public List<Category> Categories { get; set; } = new List<Category>();
    }

    public record Category
    {
        ///<summary>
        ///Name of the given product category.
        ///</summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        ///<summary>
        ///Tax code of the given product category.
        ///</summary>
        [JsonPropertyName("product_tax_code")]
        public string ProductTaxCode { get; set; } = string.Empty;

        ///<summary>
        ///Description of the given product category.
        ///</summary>
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }
}
