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

## Package Dependencies

TaxJar.net comes with assemblies for **.NET 4.5.2** and **.NET Standard 2.0**. It requires the following dependencies:

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
var client = new TaxjarApi(ConfigurationManager.AppSettings["TaxjarApiKey"]);
```

Note: This method requires [System.Configuration.ConfigurationManager](https://docs.microsoft.com/en-us/dotnet/api/system.configuration.configurationmanager?view=netframework-4.5.2) on .NET Framework 4.x. If you'd like to use this method on .NET Standard or Core, reference the [NuGet package](https://www.nuget.org/packages/System.Configuration.ConfigurationManager/) in your project.

### Method B

```csharp
var client = new TaxjarApi("[Your TaxJar API Key]");
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
var categories = client.Categories();

// Async Method
var categories = await client.CategoriesAsync();
```

### Calculate sales tax for an order <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#post-calculate-sales-tax-for-an-order))_</small>

> Shows the sales tax that should be collected for a given order.

```csharp
var tax = client.TaxForOrder(new {
  from_country = "US",
  from_zip = "07001",
  from_state = "NJ",
  from_city = "Avenel",
  from_street = "305 W Village Dr",
  to_country = "US",
  to_zip = "07446",
  to_state = "NJ",
  to_city = "Ramsey",
  to_street = "63 W Main St",
  amount = 16.50,
  shipping = 1.50,
  line_items = new[] {
    new {
      id = "1",
      quantity = 1,
      product_tax_code = "31000",
      unit_price = 15,
      discount = 0
    }
  }
});

// Async Method
var tax = await client.TaxForOrderAsync(new {
  from_country = "US",
  from_zip = "07001",
  from_state = "NJ",
  from_city = "Avenel",
  from_street = "305 W Village Dr",
  to_country = "US",
  to_zip = "07446",
  to_state = "NJ",
  to_city = "Ramsey",
  to_street = "63 W Main St",
  amount = 16.50,
  shipping = 1.50,
  line_items = new[] {
    new {
      id = "1",
      quantity = 1,
      product_tax_code = "31000",
      unit_price = 15,
      discount = 0
    }
  }
});

// Request Entity
var taxEntity = new Tax {
  from_country = "US",
  from_zip = "07001",
  from_state = "NJ",
  from_city = "Avenel",
  from_street = "305 W Village Dr",
  to_country = "US",
  to_zip = "07446",
  to_state = "NJ",
  to_city = "Ramsey",
  to_street = "63 W Main St",
  amount = 16.50,
  shipping = 1.50,
  line_items = new[] {
    new {
      id = "1",
      quantity = 1,
      product_tax_code = "31000",
      unit_price = 15,
      discount = 0
    }
  }
};

var tax = client.TaxForOrder(taxEntity);
```

### List order transactions <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-list-order-transactions))_</small>

> Lists existing order transactions created through the API.

```csharp
var orders = client.ListOrders(new {
  from_transaction_date = "2015/05/01",
  to_transaction_date = "2015/05/31"
});

// Async Method
var orders = await client.ListOrdersAsync(new {
  from_transaction_date = "2015/05/01",
  to_transaction_date = "2015/05/31"
});

// Request Entity
var orderFilter = new OrderFilter {
  FromTransactionDate = "2015/05/01",
  ToTransactionDate = "2015/05/31"
};

var orders = client.ListOrders(orderFilter);
```

### Show order transaction <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-show-an-order-transaction))_</small>

> Shows an existing order transaction created through the API.

```csharp
var order = client.ShowOrder("123");

// Async Method
var order = await client.ShowOrderAsync("123");
```

### Create order transaction <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#post-create-an-order-transaction))_</small>

> Creates a new order transaction.

```csharp
var order = client.CreateOrder(new {
  transaction_id = "123",
  transaction_date = "2015/05/04",
  from_country = "US",
  from_zip = "92093",
  from_state = "CA",
  from_city = "La Jolla",
  from_street = "9500 Gilman Drive",
  to_country = "US",
  to_zip = "90002",
  to_state = "CA",
  to_city = "Los Angeles",
  to_street = "123 Palm Grove Ln",
  amount = 17,
  shipping = 2,
  sales_tax = 0.95,
  line_items = new[] {
    new {
      id = "1",
      quantity = 1,
      product_identifier = "12-34243-0",
      description = "Heavy Widget",
      unit_price = 15,
      discount = 0,
      sales_tax = 0.95
    }
  }
});

// Async Method
var order = await client.CreateOrderAsync(new {
  transaction_id = "123",
  transaction_date = "2015/05/04",
  from_country = "US",
  from_zip = "92093",
  from_state = "CA",
  from_city = "La Jolla",
  from_street = "9500 Gilman Drive",
  to_country = "US",
  to_zip = "90002",
  to_state = "CA",
  to_city = "Los Angeles",
  to_street = "123 Palm Grove Ln",
  amount = 17,
  shipping = 2,
  sales_tax = 0.95,
  line_items = new[] {
    new {
      id = "1",
      quantity = 1,
      product_identifier = "12-34243-0",
      description = "Heavy Widget",
      unit_price = 15,
      discount = 0,
      sales_tax = 0.95
    }
  }
});

// Request Entity
var orderEntity = new Order {
  transaction_id = "123",
  transaction_date = "2015/05/04",
  from_country = "US",
  from_zip = "92093",
  from_state = "CA",
  from_city = "La Jolla",
  from_street = "9500 Gilman Drive",
  to_country = "US",
  to_zip = "90002",
  to_state = "CA",
  to_city = "Los Angeles",
  to_street = "123 Palm Grove Ln",
  amount = 17,
  shipping = 2,
  sales_tax = 0.95,
  line_items = new[] {
    new {
      id = "1",
      quantity = 1,
      product_identifier = "12-34243-0",
      description = "Heavy Widget",
      unit_price = 15,
      discount = 0,
      sales_tax = 0.95
    }
  }
};

var order = client.CreateOrder(orderEntity);
```

### Update order transaction <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#put-update-an-order-transaction))_</small>

> Updates an existing order transaction created through the API.

```csharp
var order = client.UpdateOrder(new {
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

// Async Method
var order = await client.UpdateOrderAsync(new {
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

// Request Entity
var orderEntity = new Order {
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

var order = client.UpdateOrder(orderEntity);
```

### Delete order transaction <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#delete-delete-an-order-transaction))_</small>

> Deletes an existing order transaction created through the API.

```csharp
var order = client.DeleteOrder("123");

// Async Method
var order = await client.DeleteOrderAsync("123");
```

### List refund transactions <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-list-refund-transactions))_</small>

> Lists existing refund transactions created through the API.

```csharp
var refunds = client.ListRefunds(new {
  from_transaction_date = "2015/05/01",
  to_transaction_date = "2015/05/31"
});

// Async Method
var refunds = await client.ListRefundsAsync(new {
  from_transaction_date = "2015/05/01",
  to_transaction_date = "2015/05/31"
});

// Request Entity
var refundFilter = new RefundFilter {
  FromTransactionDate = "2015/05/01",
  ToTransactionDate = "2015/05/31"
};

var refunds = client.ListRefunds(refundFilter);
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
var refund = client.CreateRefund(new {
  transaction_id = "123-refund",
  transaction_reference_id = "123",
  transaction_date = "2015/05/04",
  from_country = "US",
  from_zip = "92093",
  from_state = "CA",
  from_city = "La Jolla",
  from_street = "9500 Gilman Drive",
  to_country = "US",
  to_zip = "90002",
  to_state = "CA",
  to_city = "Los Angeles",
  to_street = "123 Palm Grove Ln",
  amount = -17,
  shipping = -2,
  sales_tax = -0.95,
  line_items = new[] {
    new {
      id = "1",
      quantity = 1,
      product_identifier = "12-34243-0",
      description = "Heavy Widget",
      unit_price = -15,
      discount = -0,
      sales_tax = -0.95
    }
  }
});

// Async Method
var refund = await client.CreateRefundAsync(new {
  transaction_id = "123-refund",
  transaction_reference_id = "123",
  transaction_date = "2015/05/04",
  from_country = "US",
  from_zip = "92093",
  from_state = "CA",
  from_city = "La Jolla",
  from_street = "9500 Gilman Drive",
  to_country = "US",
  to_zip = "90002",
  to_state = "CA",
  to_city = "Los Angeles",
  to_street = "123 Palm Grove Ln",
  amount = -17,
  shipping = -2,
  sales_tax = -0.95,
  line_items = new[] {
    new {
      id = "1",
      quantity = 1,
      product_identifier = "12-34243-0",
      description = "Heavy Widget",
      unit_price = -15,
      discount = -0,
      sales_tax = -0.95
    }
  }
});

// Request Entity
var refundEntity = new Refund {
  transaction_id = "123-refund",
  transaction_reference_id = "123",
  transaction_date = "2015/05/04",
  from_country = "US",
  from_zip = "92093",
  from_state = "CA",
  from_city = "La Jolla",
  from_street = "9500 Gilman Drive",
  to_country = "US",
  to_zip = "90002",
  to_state = "CA",
  to_city = "Los Angeles",
  to_street = "123 Palm Grove Ln",
  amount = -17,
  shipping = -2,
  sales_tax = -0.95,
  line_items = new[] {
    new {
      id = "1",
      quantity = 1,
      product_identifier = "12-34243-0",
      description = "Heavy Widget",
      unit_price = -15,
      discount = -0,
      sales_tax = -0.95
    }
  }
};

var refund = client.CreateRefund(refundEntity);
```

### Update refund transaction <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#put-update-a-refund-transaction))_</small>

> Updates an existing refund transaction created through the API.

```csharp
var refund = client.UpdateRefund(new {
  transaction_id = "123-refund",
  transaction_reference_id = "123",
  amount = -17,
  shipping = -2,
  line_items = new[] {
    new {
      id = "1",
      quantity = 1,
      product_identifier = "12-34243-0",
      description = "Heavy Widget",
      unit_price = -15,
      discount = -0,
      sales_tax = -0.95
    }
  }
});

// Async Method
var refund = await client.UpdateRefundAsync(new {
  transaction_id = "123-refund",
  transaction_reference_id = "123",
  amount = -17,
  shipping = -2,
  line_items = new[] {
    new {
      id = "1",
      quantity = 1,
      product_identifier = "12-34243-0",
      description = "Heavy Widget",
      unit_price = -15,
      discount = -0,
      sales_tax = -0.95
    }
  }
});

// Request Entity
var refundEntity = new Refund {
  transaction_id = "123-refund",
  transaction_reference_id = "123",
  amount = -17,
  shipping = -2,
  line_items = new[] {
    new {
      id = "1",
      quantity = 1,
      product_identifier = "12-34243-0",
      description = "Heavy Widget",
      unit_price = -15,
      discount = -0,
      sales_tax = -0.95
    }
  }
};

var refund = client.UpdateRefund(refundEntity);
```

### Delete refund transaction <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#delete-delete-a-refund-transaction))_</small>

> Deletes an existing refund transaction created through the API.

```csharp
var refund = client.DeleteRefund("123-refund");

// Async Method
var refund = await client.DeleteRefundAsync("123-refund");
```

### List customers <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-list-customers))_</small>

> Lists existing customers created through the API.

```csharp
var customers = client.ListCustomers();

// Async Method
var customers = await client.ListCustomersAsync();
```

### Show customer <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-show-a-customer))_</small>

> Shows an existing customer created through the API.

```csharp
var customer = client.ShowCustomer("123");

// Async Method
var customer = await client.ShowCustomerAsync("123");
```

### Create customer <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#post-create-a-customer))_</small>

> Creates a new customer.

```csharp
var customer = client.CreateCustomer(new {
  customer_id = "123",
  exemption_type = "wholesale",
  name = "Dunder Mifflin Paper Company",
  exempt_regions = new[] {
    new {
      country = "US",
      state = "FL"
    },
    new {
      country = "US",
      state = "PA"
    }
  },
  country = "US",
  state = "PA",
  zip = "18504",
  city = "Scranton",
  street = "1725 Slough Avenue"
});

// Async Method
var customer = await client.CreateCustomerAsync(new {
  customer_id = "123",
  exemption_type = "wholesale",
  name = "Dunder Mifflin Paper Company",
  exempt_regions = new[] {
    new {
      country = "US",
      state = "FL"
    },
    new {
      country = "US",
      state = "PA"
    }
  },
  country = "US",
  state = "PA",
  zip = "18504",
  city = "Scranton",
  street = "1725 Slough Avenue"
});

// Request Entity
var customerEntity = new Customer {
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

var customer = client.CreateCustomer(customerEntity);
```

### Update customer <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#put-update-a-customer))_</small>

> Updates an existing customer created through the API.

```csharp
var customer = client.UpdateCustomer(new {
  customer_id = "123",
  exemption_type = "wholesale",
  name = "Sterling Cooper",
  exempt_regions = new[] {
    new {
      country = "US",
      state = "NY"
    }
  },
  country = "US",
  state = "NY",
  zip = "10010",
  city = "New York",
  street = "405 Madison Ave"
});

// Async Method
var customer = await client.UpdateCustomerAsync(new {
  customer_id = "123",
  exemption_type = "wholesale",
  name = "Sterling Cooper",
  exempt_regions = new[] {
    new {
      country = "US",
      state = "NY"
    }
  },
  country = "US",
  state = "NY",
  zip = "10010",
  city = "New York",
  street = "405 Madison Ave"
});

// Request Entity
var customerEntity = new Customer {
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

var customer = client.UpdateCustomer(customerEntity);
```

### Delete customer <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#delete-delete-a-customer))_</small>

> Deletes an existing customer created through the API.

```csharp
var customer = client.DeleteCustomer("123");

// Async Method
var customer = await client.DeleteCustomerAsync("123");
```

### List tax rates for a location (by zip/postal code) <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-show-tax-rates-for-a-location))_</small>

> Shows the sales tax rates for a given location.
>
> **Please note this method only returns the full combined rate for a given location.** It does not support nexus determination, sourcing based on a ship from and ship to address, shipping taxability, product exemptions, customer exemptions, or sales tax holidays. We recommend using [`TaxForOrder` to accurately calculate sales tax for an order](#calculate-sales-tax-for-an-order-api-docs).

```csharp
var rates = client.RatesForLocation("90002", new {
  city = "LOS ANGELES",
  country = "US"
});

// Async Method
var rates = await client.RatesForLocationAsync("90002", new {
  city = "LOS ANGELES",
  country = "US"
});

// Request Entity
var rateEntity = new Rate {
  City = "LOS ANGELES",
  Country = "US"
};

var rates = client.RatesForLocation("90002", rateEntity);
```

### List nexus regions <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-list-nexus-regions))_</small>

> Lists existing nexus locations for a TaxJar account.

```csharp
var nexusRegions = client.NexusRegions();

// Async Method
var nexusRegions = await client.NexusRegionsAsync();
```

### Validate an address <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#post-validate-an-address))_</small>

> Validates a customer address and returns back a collection of address matches. **Address validation requires a [TaxJar Plus](https://www.taxjar.com/plus/) subscription.**

```csharp
var addresses = client.ValidateAddress(new {
  country = "US",
  state = "AZ",
  zip = "85297",
  city = "Gilbert",
  street = "3301 South Greenfield Rd"
});

// Async Method
var addresses = client.ValidateAddressAsync(new {
  country = "US",
  state = "AZ",
  zip = "85297",
  city = "Gilbert",
  street = "3301 South Greenfield Rd"
});
```

### Validate a VAT number <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-validate-a-vat-number))_</small>

> Validates an existing VAT identification number against [VIES](http://ec.europa.eu/taxation_customs/vies/).

```csharp
var validation = client.ValidateVat(new {
  vat = "FR40303265045"
});

// Async Method
var validation = await client.ValidateVatAsync(new {
  vat = "FR40303265045"
});

// Request Entity
var vatEntity = new Validation {
  Vat = "FR40303265045"
};

var validation = client.ValidateVat(vatEntity);
```

### Summarize tax rates for all regions <small>_([API docs](https://developers.taxjar.com/api/reference/?csharp#get-summarize-tax-rates-for-all-regions))_</small>

> Retrieve minimum and average sales tax rates by region as a backup.
>
> This method is useful for periodically pulling down rates to use if the TaxJar API is unavailable. However, it does not support nexus determination, sourcing based on a ship from and ship to address, shipping taxability, product exemptions, customer exemptions, or sales tax holidays. We recommend using [`TaxForOrder` to accurately calculate sales tax for an order](#calculate-sales-tax-for-an-order-api-docs).

```csharp
var summaryRates = client.SummaryRates();

// Async Method
var summaryRates = await client.SummaryRatesAsync();
```

## Custom Options

You can pass additional options using `SetApiConfig` or when instantiating the client for the following:

### Timeouts

```csharp
// Custom timeout when instantiating the client
var client = new TaxjarApi("[Your TaxJar API Key]", new { apiUrl = "https://api.taxjar.com", timeout = 30 * 1000 });

// Custom timeout via `SetApiConfig`
client.SetApiConfig("timeout", 30 * 1000);
```

### API Version
```csharp
// Custom API version when instantiating the client
var client = new TaxjarApi("[Your TaxJar API Key]", new { apiUrl = "https://api.taxjar.com", headers = new Dictionary<string, string> {
  { "x-api-version", "2020-08-07" }
}});

// Custom API version via `SetApiConfig`
client.SetApiConfig("headers", new Dictionary<string, string> {
  { "x-api-version", "2020-08-07" }
});
```

## Sandbox Environment

You can easily configure the client to use the [TaxJar Sandbox](https://developers.taxjar.com/api/reference/?csharp#sandbox-environment):

```csharp
var client = new TaxjarApi("[Your TaxJar Sandbox API Key]", new { apiUrl = "https://api.sandbox.taxjar.com" });
```

For testing specific [error response codes](https://developers.taxjar.com/api/reference/?csharp#errors), pass the custom `X-TJ-Expected-Response` header:

```csharp
client.SetApiConfig("headers", new Dictionary<string, string> {
  { "X-TJ-Expected-Response", "422" }
});
```

## Error Handling

When invalid data is sent to TaxJar or we encounter an error, we’ll throw a `TaxjarException` with the HTTP status code and error message. To catch these exceptions, refer to the example below. [Click here](https://developers.taxjar.com/api/guides/csharp/#error-handling) for a list of common error response classes.

```csharp
using Taxjar;
var client = new TaxjarApi();

try
{
  // Invalid request
  var order = client.CreateOrder(new {
    transaction_date = "2015/05/04",
    from_country = "US",
    from_zip = "07001",
    from_state = "NJ",
    from_city = "Avenel",
    from_street = "305 W Village Dr",
    to_country = "US",
    to_zip = "90002",
    to_state = "CA",
    to_city = "Ramsey",
    to_street = "63 W Main St",
    amount = 17.45,
    shipping = 1.5,
    sales_tax = 0.95
    line_items = new[] {
      new {
        id = "1",
        quantity = 1,
        product_tax_code = "31000",
        unit_price = 15,
        discount = 0,
        sales_tax = 0.95
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
```

## Tests

We use [NUnit](https://github.com/nunit/nunit) and [WireMock.Net](https://github.com/WireMock-Net/WireMock.Net) for testing. Before running the specs, create a `secrets.json` file inside the `Taxjar.Tests` directory with your sandbox API Token:

```
{
  "ApiToken": "YOUR_SANDBOX_KEY"
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
