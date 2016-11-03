# TaxJar Sales Tax API for .NET / C&#35;

Official .NET / C# client for [SmartCalcs](http://www.taxjar.com/api/) by [TaxJar](http://www.taxjar.com). For the API documentation, please visit [http://developers.taxjar.com/api](http://developers.taxjar.com/api).

## Getting Started

We recommend installing TaxJar.net via [NuGet](https://www.nuget.org/). Before authenticating, [get your API key from TaxJar](https://app.taxjar.com/api_sign_up/plus/).

Use the NuGet package manager inside Visual Studio, Xamarin Studio, or run the following command in the [Package Manager Console](https://docs.nuget.org/ndocs/tools/package-manager-console):

```
PM> Install-Package TaxJar
```

## Package Dependencies

TaxJar.net requires the following dependencies:

- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) Popular high-performance JSON framework for .NET
- [RestSharp](https://github.com/restsharp/RestSharp) Simple REST and HTTP API Client for .NET

These packages are automatically included when installing via [NuGet](https://www.nuget.org/).

## Authentication

To authenticate with our API, add a new AppSetting with your TaxJar API key to your project's `Web.config` / `App.config` file or directly supply the API key when instantiating the client:

### Method A

```xml
<!-- Web.config / App.config -->
<appSettings>
...
  <add key="TaxjarApiKey" value="[Your TaxJar API Key]" />
...
</appSettings>
```
```csharp
var client = new TaxjarApi();
```

### Method B

```csharp
var client = new TaxjarApi("[Your TaxJar API Key]");
```

## Usage

### List all tax categories

```csharp
var categories = client.Categories();
```

### List tax rates for a location (by zip/postal code)

```csharp
var rates = client.RatesForLocation("90002", new {
  city = "LOS ANGELES",
  country = "US"
});
```

### Calculate sales tax for an order

```csharp
var rates = client.TaxForOrder(new {
  from_country =  "US",
  from_zip = "07001",
  from_state = "NJ",
  to_country = "US",
  to_zip = "07446",
  to_state = "NJ",
  amount = 16.50,
  shipping = 1.50
});
```

### List order transactions

```csharp
var orders = client.ListOrders(new {
	from_transaction_date = "2015/05/01",
	to_transaction_date = "2015/05/31"
});
```

### Show order transaction

```
var order = client.ShowOrder(123);
```

### Create order transaction

```csharp
var order = client.CreateOrder(new {
  transaction_id = "123",
  transaction_date = "2015/05/04",
  to_country = "US",
  to_zip = "90002",
  to_city = "Los Angeles",
  to_street = "123 Palm Grove Ln",
  amount = 17,
  shipping = 2,
  sales_tax = 0.95,
  line_items = new[] {
    new {
      quantity = 1,
      product_identifier = "12-34243-0",
      description = "Heavy Widget",
      unit_price = 15,
      sales_tax = 0.95
    }
  }
});
```

### Update order transaction

```csharp
var order = client.UpdateOrder(new
{
  transaction_id = "123",
  amount = 17,
  shipping = 2,
  line_items = new[] {
    new {
      quantity = 1,
      product_identifier = "12-34243-0",
      description = "Heavy Widget",
      unit_price = 15,
      discount = 0,
      sales_tax = 0.95
    }
  }
});
```

### Delete order transaction

```csharp
var order = client.DeleteOrder(123);
```

### List refund transactions

```csharp
var refunds = client.ListRefunds(new
{
  from_transaction_date = "2015/05/01",
  to_transaction_date = "2015/05/31"
});
```

### Show refund transaction

```csharp
var refund = client.ShowRefund(321);
```

### Create refund transaction

```csharp
var refund = client.CreateRefund(new
{
  transaction_id = "321",
  transaction_date = "2015/05/04",
  transaction_reference_id = "123",
  to_country = "US",
  to_zip = "90002",
  to_city = "Los Angeles",
  to_street = "123 Palm Grove Ln",
  amount = 17,
  shipping = 2,
  sales_tax = 0.95,
  line_items = new[] {
    new {
      quantity = 1,
      product_identifier = "12-34243-0",
      description = "Heavy Widget",
      unit_price = 15,
      sales_tax = 0.95
    }
  }
});
```

### Update refund transaction

```csharp
var refund = client.UpdateRefund(new
{
  transaction_id = "321",
  amount = 17,
  shipping = 2,
  line_items = new[] {
    new {
      quantity = 1,
      product_identifier = "12-34243-0",
      description = "Heavy Widget",
      unit_price = 15,
      discount = 0,
      sales_tax = 0.95
    }
  }
});
```

### Delete refund transaction

```csharp
var refund = client.DeleteRefund(321);
```

### List nexus regions

```csharp
var nexusRegions = client.NexusRegions();
```

### Validate a VAT number

```csharp
var validation = client.Validate(new {
  vat = "FR40303265045"
});
```

### Summarize tax rates for all regions

```csharp
var summaryRates = client.SummaryRates();
```

## Tests

We use [NUnit](https://github.com/nunit/nunit) and [HttpMock](https://github.com/hibri/HttpMock) to directly test client methods inside Xamarin Studio.

## More Information

More information can be found at [TaxJar Developers](http://developers.taxjar.com).

## License

TaxJar.net is released under the [MIT License](https://github.com/taxjar/taxjar.net/blob/master/LICENSE.txt).

## Support

Bug reports and feature requests should be filed on the [GitHub issue tracking page](https://github.com/taxjar/taxjar.net/issues). 

## Contributing

1. Fork it
2. Create your feature branch (`git checkout -b my-new-feature`)
3. Commit your changes (`git commit -am 'Add some feature'`)
4. Push to the branch (`git push origin my-new-feature`)
5. Create new pull request
