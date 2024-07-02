# TaxJar Sales Tax API for .NET / C&#35; [![Nuget](https://img.shields.io/nuget/v/taxjar)](https://www.nuget.org/packages/TaxJar/)

Official .NET / C# client for the [TaxJar API](https://www.taxjar.com/api/reference/?csharp). For the API documentation, please visit [https://developers.taxjar.com/api](https://developers.taxjar.com/api/reference/?csharp).

<hr>

[Getting Started](#getting-started)<br>
[Package Dependencies](#package-dependencies)<br>
[Authentication](#authentication)<br>
[Usage](#usage)<br>
[Custom Options](#custom-options)<br>
[Sandbox Environment](#sandbox-environment)<br>
[Error Handling](#error-handling)<br>
[Tests](#tests)<br>
[More Information](#more-information)<br>
[License](#license)<br>
[Support](#support)<br>
[Contributing](#contributing)

<hr>

## Getting Started

We recommend installing TaxJar.net via [NuGet](https://www.nuget.org/). Before authenticating, [get your API key from TaxJar](https://app.taxjar.com/api_sign_up/plus/).

Use the NuGet package manager inside Visual Studio, Xamarin Studio, or run the following command in the [Package Manager Console](https://docs.nuget.org/ndocs/tools/package-manager-console):

```
PM> Install-Package TaxJar
```

Call `AddTaxJar`:
   ```csharp
   public void ConfigureServices(IServiceCollection services)
   {
       services.AddTaxJar();
   }
   ```

   The optional `setupAction` allows consumers to manually configure the `TaxJarApiOptions` object:

   ```csharp
   public void ConfigureServices(IServiceCollection services)
   {
       services.AddTaxJar(
           options =>
           {
               options.ApiToken = "< Taxjar Api Token >";
               options.Timeout = TimeSpan.FromMilliseconds(5000);
           });
   }
   ```

## Package Dependencies

TaxJar.net comes with assemblies for **.NET 8**. It requires the following dependencies:

- [System.Text.Json](https://www.nuget.org/packages/System.Text.Json) &quot;Provides high-performance and low-allocating types that serialize objects to JavaScript Object Notation (JSON) text and deserialize JSON text to objects, with UTF-8 support built-in.&quot;
- [Microsoft.Extensions.Options.ConfigurationExtensions](https://www.nuget.org/packages/Microsoft.Extensions.Options.ConfigurationExtensions) &quot;Microsoft.Extensions.Options.ConfigurationExtensions provides additional configuration-specific functionality related to Options.&quot;
- [Microsoft.Extensions.Http](https://www.nuget.org/packages/Microsoft.Extensions.Http) &quot;Microsoft.Extensions.Http package provides AddHttpClient extension methods for IServiceCollection, IHttpClientFactory interface and its default implementation. This provides the ability to set up named HttpClient configurations in a DI container and later retrieve them via an injected IHttpClientFactory instance.&quot;

These packages are automatically included when installing via [NuGet](https://www.nuget.org/).

## Authentication

To authenticate with our API, add a new AppSetting with your TaxJar API key to your project's  `appsettings.json` file:

### Method A

```json
<!-- appsettings.json -->

...
  "Taxjar": {
    "ApiToken": "< Taxjar Api Token >"
  }
...

```

You're now ready to use TaxJar! [Check out our quickstart guide](https://developers.taxjar.com/api/guides/csharp/#csharp-quickstart) to get up and running quickly.

## Usage

[`Categories` - List all tax categories](#list-all-tax-categories-api-docs)<br>
[`TaxForOrder` - Calculate sales tax for an order](#calculate-sales-tax-for-an-order-api-docs)<br>
[`ListOrders` - List order transactions](#list-order-transactions-api-docs)<br>
[`ShowOrder` - Show order transaction](#show-order-transaction-api-docs)<br>
[`CreateOrder` - Create order transaction](#create-order-transaction-api-docs)<br>
[`UpdateOrder` - Update order transaction](#update-order-transaction-api-docs)<br>
[`DeleteOrder` - Delete order transaction](#delete-order-transaction-api-docs)<br>
[`ListRefunds` - List refund transactions](#list-refund-transactions-api-docs)<br>
[`ShowRefund` - Show refund transaction](#show-refund-transaction-api-docs)<br>
[`CreateRefund` - Create refund transaction](#create-refund-transaction-api-docs)<br>
[`UpdateRefund` - Update refund transaction](#update-refund-transaction-api-docs)<br>
[`DeleteRefund` - Delete refund transaction](#delete-refund-transaction-api-docs)<br>
[`ListCustomers` - List customers](#list-customers-api-docs)<br>
[`ShowCustomer` - Show customer](#show-customer-api-docs)<br>
[`CreateCustomer` - Create customer](#create-customer-api-docs)<br>
[`UpdateCustomer` - Update customer](#update-customer-api-docs)<br>
[`DeleteCustomer` - Delete customer](#delete-customer-api-docs)<br>
[`RatesForLocation` - List tax rates for a location (by zip/postal code)](#list-tax-rates-for-a-location-by-zippostal-code-api-docs)<br>
[`NexusRegions` - List nexus regions](#list-nexus-regions-api-docs)<br>
[`ValidateAddress` - Validate an address](#validate-an-address-api-docs)<br>
[`ValidateVat` - Validate a VAT number](#validate-a-vat-number-api-docs)<br>
[`SummaryRates` - Summarize tax rates for all regions](#summarize-tax-rates-for-all-regions-api-docs)

### List all tax categories <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-list-tax-categories))_</small>

> The TaxJar API provides product-level tax rules for a subset of product categories. These categories are to be used for products that are either exempt from sales tax in some jurisdictions or are taxed at reduced rates. You need not pass in a product tax code for sales tax calculations on product that is fully taxable. Simply leave that parameter out.

```csharp
var categories = await client.CategoriesAsync();
```

### Calculate sales tax for an order <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#post-calculate-sales-tax-for-an-order))_</small>

> Shows the sales tax that should be collected for a given order.

```csharp
var tax = await client.TaxForOrderAsync(new () {
  FromCountry = "US",
  FromZip = "07001",
  FromState = "NJ",
  FromCity = "Avenel",
  FromCity = "305 W Village Dr",
  ToCountry = "US",
  ToZip = "07446",
  ToState = "NJ",
  ToCity = "Ramsey",
  ToStreet = "63 W Main St",
  Amount = 16.50,
  Shipping = 1.50,
  LineItems = new List<LineItem> {
    new() {
        Id = "1",
        Quantity = 1,
        ProductTaxCode = "31000",
        UnitPrice = 15.00m,
        Discount = 0
      }
    }
});

// Request Entity
var taxRequestEntity = new TaxjarTaxCalculationRequest {
  FromCountry = "US",
  FromZip = "07001",
  FromState = "NJ",
  FromCity = "Avenel",
  FromStreet = "305 W Village Dr",
  ToCountry = "US",
  ToZip = "07446",
  ToState = "NJ",
  ToCity = "Ramsey",
  ToStreet = "63 W Main St",
  Amount = 16.50,
  Shipping = 1.50,
  LineItems = new List<TaxLineItem> {
    new() {
      Id = "1",
      ProductIdentifier = "Airstream Deluxe A4",
      Quantity = 1,
      product_tax_code = "31000",
      UnitPrice = 15,
      Discount = 0
    }
  }
};

var tax = await client.TaxForOrderAsync(taxRequestEntity);
```

### List order transactions <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-list-order-transactions))_</small>

> Lists existing order transactions created through the API.

```csharp
var orders = await client.ListOrdersAsync(new() {
  FromTransactionDate = DateTime.Parse("2024/05/01", CultureInfo.CurrentCulture),
  ToTransactionDate = DateTime.Parse("2024/05/01", CultureInfo.CurrentCulture),
});

// Request Entity
var orderFilter = new OrderFilter {
  FromTransactionDate = DateTime.Parse("2024/05/01", CultureInfo.CurrentCulture),
  ToTransactionDate = DateTime.Parse("2024/05/01", CultureInfo.CurrentCulture),
  ToTransactionDate = DateTime.Parse("2024/15/01", CultureInfo.CurrentCulture),
  Provider = "api"
};

```

### Show order transaction <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-show-an-order-transaction))_</small>

> Shows an existing order transaction created through the API.

```csharp
var order = await client.ShowOrderAsync("123");
```

### Create order transaction <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#post-create-an-order-transaction))_</small>

> Creates a new order transaction.

```csharp
var order = await client.CreateOrderAsync(new() {
  TransactionId = "123",
  TransactionDate = DateTime.Parse("2015/05/04", CultureInfo.CurrentCulture),
  FromCountry = "US",
  FromZip = "92093",
  FromState = "CA",
  FromCity = "La Jolla",
  FromStreet = "9500 Gilman Drive",
  ToCountry = "US",
  ToZip = "90002",
  ToState = "CA",
  ToCity = "Los Angeles",
  ToStreet = "123 Palm Grove Ln",
  Amount = 17,
  Shipping = 2,
  SalesTax = 0.95,
  LineItems = new List<LineItem> {
    new {
      Id = "1",
      Quantity = 1,
      ProductIdentifier = "12-34243-0",
      Description = "Heavy Widget",
      UnitPrice = 15,
      Discount = 0,
      SalesTax = 0.95
    }
  }
});

// Request Entity
var orderRequestEntity = new TaxjarOrderRequest {
  TransactionId = "123",
  TransactionDate = DateTime.Parse("2024/15/01", CultureInfo.CurrentCulture),
  FromCountry = "US",
  FromZip = "92093",
  FromState = "CA",
  FromCity = "La Jolla",
  FromStreet = "9500 Gilman Drive",
  ToCountry = "US",
  ToZip = "90002",
  ToState = "CA",
  ToCity = "Los Angeles",
  ToStreet = "123 Palm Grove Ln",
  Amount = 17,
  Shipping = 2,
  SalesTax = 0.95,
  LineItems = new List<LineItem> {
    new {
      Id = "1",
      Quantity = 1,
      ProductIdentifier = "12-34243-0",
      Description = "Heavy Widget",
      UnitPrice = 15,
      Discount = 0,
      SalesTax = 0.95
    }
  }
};

var order = client.CreateOrderAsync(orderRequestEntity);
```

### Update order transaction <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#put-update-an-order-transaction))_</small>

> Updates an existing order transaction created through the API.

```csharp
var order = await client.UpdateOrderAsync(new() {
  TransactionId = "123",
  Amount = 17,
  Shipping = 2,
  LineItems = new List<LineItem> {
    new {
      Quantity = 1,
      ProductIdentifier = "12-34243-0",
      Description = "Heavy Widget",
      UnitPrice = 15,
      Discount = 0,
      SalesTax = 0.95
    }
  }
});

// Request Entity
var orderRequestEntity = new TaxjarOrderRequest {
  TransactionId = "123",
  Amount = 17,
  Shipping = 2,
  LineItems = new List<LineItem> {
    new LineItem {
      Quantity = 1,
      ProductIdentifier = "12-34243-0",
      Description = "Heavy Widget",
      UnitPrice = 15,
      Discount = 0,
      SalesTax = 0.95
    }
  }
};

var order = await client.UpdateOrderAsync(orderRequestEntity);
```

### Delete order transaction <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#delete-delete-an-order-transaction))_</small>

> Deletes an existing order transaction created through the API.

```csharp
var order = await client.DeleteOrderAsync("123");
```

### List refund transactions <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-list-refund-transactions))_</small>

> Lists existing refund transactions created through the API.

```csharp
var refunds = await client.ListRefundsAsync(new {
  FromTransactionDate =  DateTime.Parse("2015/05/01", CultureInfo.CurrentCulture),
  ToTransactionDate =  DateTime.Parse("2015/05/31", CultureInfo.CurrentCulture)
});

// Request Entity
var refundFilter = new RefundFilter {
  FromTransactionDate = DateTime.Parse("2024/05/01", CultureInfo.CurrentCulture),
  ToTransactionDate = DateTime.Parse("2024/05/01", CultureInfo.CurrentCulture),
  ToTransactionDate = DateTime.Parse("2024/15/01", CultureInfo.CurrentCulture),
  Provider = "api"
};

var refunds = await client.ListRefundsAsync(refundFilter);
```

### Show refund transaction <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-show-a-refund-transaction))_</small>

> Shows an existing refund transaction created through the API.

```csharp
var refund = client.ShowRefund("123-refund");

// Async Method
var refund = await client.ShowRefundAsync("123-refund");
```

### Create refund transaction <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#post-create-a-refund-transaction))_</small>

> Creates a new refund transaction.

```csharp
var refund = await client.CreateRefundAsync(new() {
  TransactionId = "123-refund",
  transaction_reference_id = "123",
  TransactionDate = DateTime.Parse("2015/05/04", CultureInfo.CurrentCulture),
  FromCountry = "US",
  FromZip = "92093",
  FromState = "CA",
  FromCity = "La Jolla",
  FromStreet = "9500 Gilman Drive",
  ToCountry = "US",
  ToZip = "90002",
  ToState = "CA",
  ToCity = "Los Angeles",
  ToStreet = "123 Palm Grove Ln",
  Amount = -17,
  Shipping = -2,
  SalesTax = -0.95m,
  LineItems = new List<LineItem> {
    new() {
      Id = "1",
      Quantity = 1,
      ProductIdentifier = "12-34243-0",
      Description = "Heavy Widget",
      UnitPrice = -15,
      Discount = -0,
      SalesTax = -0.95m
    }
  }
});

// Request Entity
var refundRequestEntity = new TaxjarRefundRequest {
  TransactionId = "123-refund",
  TransactionReferenceId = "123",
  TransactionDate = DateTime.Parse("2015/05/04", CultureInfo.CurrentCulture),
  FromCountry = "US",
  FromZip = "92093",
  FromState = "CA",
  FromCity = "La Jolla",
  FromStreet = "9500 Gilman Drive",
  ToCountry = "US",
  ToZip = "90002",
  ToState = "CA",
  ToCity = "Los Angeles",
  ToStreet = "123 Palm Grove Ln",
  Amount = -17,
  Shipping = -2,
  SalesTax = -0.95m,
  LineItems = new List<LineItem> {
    new {
      Id = "1",
      Quantity = 1,
      ProductIdentifier = "12-34243-0",
      Description = "Heavy Widget",
      UnitPrice = -15,
      Discount = -0,
      SalesTax = -0.95m
    }
  }
};

var refund = await client.CreateRefundAsync(refundRequestEntity);
```

### Update refund transaction <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#put-update-a-refund-transaction))_</small>

> Updates an existing refund transaction created through the API.

```csharp
var refund = await client.UpdateRefundAsync(new() {
  TransactionId = "123-refund",
  TransactionReferenceId = "123",
  Amount = -17,
  Shipping = -2,
  LineItems = new List<LineItem> {
    new() {
      Id = "1",
      Quantity = 1,
      ProductIdentifier = "12-34243-0",
      Description = "Heavy Widget",
      UnitPrice = -15,
      Discount = -0,
      SalesTax = -0.95m
    }
  }
});

// Request Entity
var refundRequestEntity = new TaxjarRefundRequest {
  TransactionId = "123-refund",
  TransactionReferenceId = "123",
  Amount = -17,
  Shipping = -2,
  LineItems = new List<LineItem> {
    new() {
      Id = "1",
      Quantity = 1,
      ProductIdentifier = "12-34243-0",
      Description = "Heavy Widget",
      UnitPrice = -15,
      Discount = -0,
      SalesTax = -0.95m
    }
  }
};

var refund = await client.UpdateRefundAsync(refundRequestEntity);
```

### Delete refund transaction <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#delete-delete-a-refund-transaction))_</small>

> Deletes an existing refund transaction created through the API.

```csharp
var refund = await client.DeleteRefundAsync("123-refund");
```

### List customers <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-list-customers))_</small>

> Lists existing customers created through the API.

```csharp
var customers = await client.ListCustomersAsync();
```

### Show customer <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-show-a-customer))_</small>

> Shows an existing customer created through the API.

```csharp
var customer = await client.ShowCustomerAsync("123");
```

### Create customer <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#post-create-a-customer))_</small>

> Creates a new customer.

```csharp
var customer = await client.CreateCustomerAsync(new() {
  CustomerId = "123",
  ExemptionType = "wholesale",
  Name = "Dunder Mifflin Paper Company",
  ExemptRegions = new() {
    new() {
      Country = "US",
      State = "FL"
    },
    new() {
      Country = "US",
      State = "PA"
    }
  },
  Country = "US",
  State = "PA",
  Zip = "18504",
  City = "Scranton",
  Street = "1725 Slough Avenue"
});

// Request Entity
var customerRequestEntity = new Customer {
  CustomerId = "123",
  ExemptionType = "wholesale",
  Name = "Dunder Mifflin Paper Company",
  ExemptRegions = new List<ExemptRegion> {
    new ExemptRegion {
      Country = "US",
      State = "FL"
    },
    new ExemptRegion {
      Country = "US",
      State = "PA"
    }
  },
  Country = "US",
  State = "PA",
  Zip = "18504",
  City = "Scranton",
  Street = "1725 Slough Avenue"
};

var customer = await client.CreateCustomerAsync(customerRequestEntity);
```

### Update customer <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#put-update-a-customer))_</small>

> Updates an existing customer created through the API.

```csharp

// Async Method
var customer = await client.UpdateCustomerAsync(new() {
  CustomerId = "123",
  ExemptionType = "wholesale",
  Name = "Sterling Cooper",
  ExemptRegions = new() {
    new() {
      Country = "US",
      State = "NY"
    }
  },
  Country = "US",
  State = "NY",
  Zip = "10010",
  City = "New York",
  Street = "405 Madison Ave"
});

// Request Entity
var customerRequestEntity = new Customer {
  CustomerId = "123",
  ExemptionType = "wholesale",
  Name = "Sterling Cooper",
  ExemptRegions = new List<ExemptRegion> {
    new ExemptRegion {
      Country = "US",
      State = "NY"
    }
  },
  Country = "US",
  State = "NY",
  Zip = "10010",
  City = "New York",
  Street = "405 Madison Ave"
};

var customer = client.UpdateCustomer(customerRequestEntity);
```

### Delete customer <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#delete-delete-a-customer))_</small>

> Deletes an existing customer created through the API.

```csharp
var customer = await client.DeleteCustomerAsync("123");
```

### List tax rates for a location (by zip/postal code) <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-show-tax-rates-for-a-location))_</small>

> Shows the sales tax rates for a given location.
>
> **Please note this method only returns the full combined rate for a given location.** It does not support nexus determination, sourcing based on a ship from and ship to address, shipping taxability, product exemptions, customer exemptions, or sales tax holidays. We recommend using [`TaxForOrder` to accurately calculate sales tax for an order](#calculate-sales-tax-for-an-order-api-docs).

```csharp
var rates = await client.RatesForLocationAsync("90002", new {
  City = "LOS ANGELES",
  Country = "US"
});

// Request Entity
var rateEntity = new Rate {
  City = "LOS ANGELES",
  State = "CA",
  Zip = "90002",
  Country = "US",
  Street = "1001 Stadium Dr,"
};

var rates = client.RatesForLocationAsync("90002", rateEntity);
```

### List nexus regions <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-list-nexus-regions))_</small>

> Lists existing nexus locations for a TaxJar account.

```csharp
var nexusRegions = await client.NexusRegionsAsync();
```

### Validate an address <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#post-validate-an-address))_</small>

> Validates a customer address and returns back a collection of address matches. **Address validation requires a [TaxJar Plus](https://www.taxjar.com/plus/) subscription.**

```csharp
var addresses = client.ValidateAddressAsync(new {
  Country = "US",
  State = "AZ",
  Zip = "85297",
  City = "Gilbert",
  Street = "3301 South Greenfield Rd"
});
```

### Validate a VAT number <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-validate-a-vat-number))_</small>

> Validates an existing VAT identification number against [VIES](http://ec.europa.eu/taxation_customs/vies/).

```csharp
var validation = await client.ValidateVatAsync(new {
  vat = "FR40303265045"
});

// Request Entity
var vatEntity = new Validation {
  Vat = "FR40303265045"
};

var validation = await client.ValidateVatAsync(vatEntity);
```

### Summarize tax rates for all regions <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-summarize-tax-rates-for-all-regions))_</small>

> Retrieve minimum and average sales tax rates by region as a backup.
>
> This method is useful for periodically pulling down rates to use if the TaxJar API is unavailable. However, it does not support nexus determination, sourcing based on a ship from and ship to address, shipping taxability, product exemptions, customer exemptions, or sales tax holidays. We recommend using [`TaxForOrder` to accurately calculate sales tax for an order](#calculate-sales-tax-for-an-order-api-docs).

```csharp
var summaryRates = await client.SummaryRatesAsync();
```

## Custom Options

You can pass additional options using `setupAction` when calling `AddTaxJar` when adding to the services collections as following:

### Timeouts

```csharp
   public void ConfigureServices(IServiceCollection services)
   {
       services.AddTaxJar(
           options =>
           {
               options.ApiToken = "< Taxjar Api Token >";
               options.Timeout = TimeSpan.FromMilliseconds(5000);
           });
   }
```

### API Version

   ```csharp
   public void ConfigureServices(IServiceCollection services)
   {
       services.AddTaxJar(
           options =>
           {
            ...
               options.Headers =  new Dictionary<string, string> {
              { "x-api-version", "2020-08-07" }
            };
            ...
           });
   }

   // Custom API version via Headers property
   client.Headers.Add( "x-api-version", "2020-08-07");
  ```

## Sandbox Environment

You can easily configure the client to use the [TaxJar Sandbox](https://developers.taxjar.com/api/reference/?csharp#sandbox-environment):

   ```csharp
   public void ConfigureServices(IServiceCollection services)
   {
       services.AddTaxJar(
           options =>
           {
            ...
               options.UseSandbox = true;
            ...
           });
   }
  ```

For testing specific [error response codes](https://developers.taxjar.com/api/reference/?csharp#errors), pass the custom `X-TJ-Expected-Response` header:

   ```csharp
   public void ConfigureServices(IServiceCollection services)
   {
       services.AddTaxJar(
           options =>
           {
            ...
               options.Headers =  new Dictionary<string, string> {
              { "X-TJ-Expected-Response", "422" }
            };
            ...
           });
   } 

    //error response codes via Headers property
   client.Headers.Add( "X-TJ-Expected-Response", "422");

    //remove error response codes via Headers property
    client.Headers.Remove( "X-TJ-Expected-Response");
  ```

## Error Handling

When invalid data is sent to TaxJar or we encounter an error, we’ll throw a `TaxjarException` with the HTTP status code and error message. To catch these exceptions, refer to the example below. [Click here](https://developers.taxjar.com/api/guides/csharp/#error-handling) for a list of common error response classes.

```csharp
...
try
{
  // Invalid request
  var order = client.CreateOrder(new {
    TransactionDate = DateTime.Parse("2015/05/04", CultureInfo.CurrentCulture),,
    FromCountry = "US",
    FromZip = "07001",
    FromState = "NJ",
    FromCity = "Avenel",
    FromStreet = "305 W Village Dr",
    ToCountry = "US",
    ToZip = "90002",
    ToState = "CA",
    ToCity = "Ramsey",
    ToStreet = "63 W Main St",
    Amount = 17.45,
    Shipping = 1.5,
    SalesTax = 0.95
    LineItems = new List<LineItem> {
      new {
        Id = "1",
        Quantity = 1,
        product_tax_code = "31000",
        UnitPrice = 15,
        Discount = 0,
        SalesTax = 0.95
      }
    }
  });
}
catch(TaxjarException e)
{
  // 406 Not Acceptable – transaction_id is missing
  e.TaxjarError.Error;
  e.TaxjarError.Detail;
  e.TaxjarError.StatusCode;
}
...
```

## Tests

We use [NUnit](https://github.com/nunit/nunit), [fluentassertions](https://fluentassertions.com/),  [Bogus](https://github.com/bchavez/Bogus), [NSubstitute](https://nsubstitute.github.io/), [RichardSzalay.MockHttp](https://github.com/richardszalay/mockhttp) and [Microsoft.Extensions.Configuration.UserSecrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=windows) for testing. Before running the specs, create a [`secrets.json`](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=windows). If using vscode you can install the .NET Core User Secrets extension.

```
    "Taxjar":
    {
        "ApiToken":"< Taxjar Api Token >"
    }
```

## More Information

More information can be found at [TaxJar Developers](https://developers.taxjar.com).

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
