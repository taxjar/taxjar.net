# TaxJar Sales Tax API for .NET / C&#35;

Official .NET / C# client for [SmartCalcs](http://www.taxjar.com/api/) by [TaxJar](http://www.taxjar.com). For the API documentation, please visit [http://developers.taxjar.com/api](http://developers.taxjar.com/api).

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

// Async Method
var categories = await client.CategoriesAsync();
```

### List tax rates for a location (by zip/postal code)

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
```

### List tax rates for a location (by zip/postal code) using entity

```csharp
var rateEntity = new Rate {
  City = "LOS ANGELES",
  Country = "US"
};
var rates = client.RatesForLocation("90002",rateEntity);
```

### Calculate sales tax for an order

```csharp
var tax = client.TaxForOrder(new {
  from_country = "US",
  from_zip = "07001",
  from_state = "NJ",
  to_country = "US",
  to_zip = "07446",
  to_state = "NJ",
  amount = 16.50,
  shipping = 1.50
});

// Async Method
var tax = await client.TaxForOrderAsync(new {
  from_country = "US",
  from_zip = "07001",
  from_state = "NJ",
  to_country = "US",
  to_zip = "07446",
  to_state = "NJ",
  amount = 16.50,
  shipping = 1.50
});
```

### Calculate sales tax for an order using entity

```csharp
var taxEntity = new Tax
{
	FromCountry = "US",
	FromZip = "07001",
	FromState = "NJ",
	ToCountry = "US",
	ToZip = "07446",
	ToState = "NJ",
	Amount = 16.50M,
	Shipping = 1.50M
};
var rates = client.TaxForOrder(taxEntity);
```

### List order transactions

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
```

### List order transactions using entity

```csharp
var orderFilter = new OrderFilter
{
	FromTransactionDate = "2015/05/01",
	ToTransactionDate = "2015/05/31"
};
var orders = client.ListOrders(orderFilter);
```

### Show order transaction

```csharp
var order = client.ShowOrder("123");

// Async Method
var order = await client.ShowOrderAsync("123");
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

// Async Method
var order = await client.CreateOrderAsync(new {
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

### Create order transaction with order entity

```csharp
var orderEntity = new Order
{
	TransactionId = "123",
	TransactionDate = "2015/05/04",
	ToState = "CA",
	ToCountry = "US",
	ToZip = "90002",
	ToCity = "Los Angeles",
	ToStreet = "123 Palm Grove Ln",
	Amount = 17M,
	Shipping = 2M,
	SalesTax = 0.95M,
	LineItems = new List<LineItem>
	{
		new LineItem
		{
	  		Quantity = 1,
	  		ProductIdentifier = "12-34243-0",
	 		Description = "Heavy Widget",
	  		UnitPrice = 15M,
	  		SalesTax = 0.95M
		}
	}
};
var order = client.CreateOrder(orderEntity);
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

// Async Method
var order = await client.UpdateOrderAsync(new
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

### Update order transaction with order entity

```csharp
orderEntity = new Order
{
	TransactionId = "123",
	Amount = 17M,
	Shipping = 2M,
	LineItems = new List<LineItem>
	{
		new LineItem
			{
	  			Quantity = 1,
	  			ProductIdentifier = "12-34243-0",
	  			Description = "Heavy Widget",
	  			UnitPrice = 15M,
	  			Discount = 0M,
	  			SalesTax = 0.95M
			}
  	}
};
var order = client.UpdateOrder(orderEntity);
```


### Delete order transaction

```csharp
var order = client.DeleteOrder("123");

// Async Method
var order = await client.DeleteOrderAsync("123");
```

### List refund transactions

```csharp
var refunds = client.ListRefunds(new
{
  from_transaction_date = "2015/05/01",
  to_transaction_date = "2015/05/31"
});

// Async Method
var refunds = await client.ListRefundsAsync(new
{
  from_transaction_date = "2015/05/01",
  to_transaction_date = "2015/05/31"
});
```
### List refund transactions with entity

```csharp
var refundFilterEntity = new RefundFilter
{
	FromTransactionDate = "2015/05/01",
	ToTransactionDate = "2015/05/31"
};
var refunds = client.ListRefunds(refundFilterEntity)
```

### Show refund transaction

```csharp
var refund = client.ShowRefund("321");

// Async Method
var refund = await client.ShowRefundAsync("321");
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

// Async Method
var refund = await client.CreateRefundAsync(new
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
### Create refund transaction

```csharp
var refundEntity = new Refund
{
  TransactionId = "321",
  TransactionDate = "2015/05/04",
  TransactionReferenceId = "123",
  ToCountry = "US",
  ToZip = "90002",
  ToCity = "Los Angeles",
  ToStreet = "123 Palm Grove Ln",
  Amount = 17,
  Shipping = 2M,
  SalesTax = 0.95M,
  LineItems = new List<LineItem> {
    new LineItem{
      Quantity = 1,
      ProductIdentifier = "12-34243-0",
      Description = "Heavy Widget",
      UnitPrice = 15M,
      SalesTax = 0.95M
    }
  }
};
var refund = client.CreateRefund(refundEntity);
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

// Async Method
var refund = await client.UpdateRefundAsync(new
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
### Update refund transaction

```csharp
var refundEntity = new Refund
{
  TransactionId = "321",
  Amount = 17M,
  Shipping = 2M,
  LineItems = new List<LineItem>{
    new LineItem{
      Quantity = 1,
      ProductIdentifier = "12-34243-0",
      Description = "Heavy Widget",
      UnitPrice = 15M,
      Discount = 0M,
      SalesTax = 0.95M
    }
  }
};
var refund = client.UpdateRefund(refundEntity);
```

### Delete refund transaction

```csharp
var refund = client.DeleteRefund("321");

// Async Method
var refund = await client.DeleteRefundAsync("321");
```

### List customers

```csharp
var customers = client.ListCustomers();

// Async Method
var customers = await client.ListCustomersAsync();
```

### Show customer

```csharp
var customer = client.ShowCustomer("123");

// Async Method
var customer = await client.ShowCustomerAsync("123");
```

### Create customer

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
```
### Create customer using entity

```csharp
var customerEntity = new Customer
{
	CustomerId = "123",
	ExemptionType = "wholesale",
	Name = "Dunder Mifflin Paper Company",
	ExemptRegions = new List<ExemptRegion>
	{
		new ExemptRegion 
		{
	  		Country = "US",
	  		State = "FL"
		},
		new ExemptRegion
		{
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

### Update customer

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
```

### Update customer using entity

```csharp
var customerEntity = new Customer {
  CustomerId = "123",
  ExemptionType = "wholesale",
  Name = "Sterling Cooper",
  ExemptRegions = new List<ExemptRegion>{
    new ExemptRegion{
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

### Delete customer

```csharp
var customer = client.DeleteCustomer("123");

// Async Method
var customer = await client.DeleteCustomerAsync("123");
```

### List nexus regions

```csharp
var nexusRegions = client.NexusRegions();

// Async Method
var nexusRegions = await client.NexusRegionsAsync();
```

### Validate a VAT number

```csharp
var validation = client.ValidateVat(new {
  vat = "FR40303265045"
});

// Async Method
var validation = await client.ValidateVatAsync(new {
  vat = "FR40303265045"
});
```
### Validate a VAT number using entity

```csharp
var vatEntity = new Validation
{
  Vat = "FR40303265045"
};
var validation = client.Validate(vatEntity);
```


### Summarize tax rates for all regions

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
var client = new TaxjarApi("[Your TaxJar API Key]", new { apiUrl = "https://api.taxjar.com", timeout = 30000 });

// Custom timeout via `SetApiConfig`
client.SetApiConfig("timeout", 30000);
```

## Sandbox Environment

You can easily configure the client to use the [TaxJar Sandbox](https://developers.taxjar.com/api/reference/#sandbox-environment):

```csharp
var client = new TaxjarApi("[Your TaxJar Sandbox API Key]", new { apiUrl = "https://api.sandbox.taxjar.com" });
```

For testing specific [error response codes](https://developers.taxjar.com/api/reference/#errors), pass the custom `X-TJ-Expected-Response` header:

```csharp
client.SetApiConfig("headers", new Dictionary<string, string>
{
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
    to_country = "US",
    to_state = "CA",
    to_zip = "90002",
    amount = 17.45,
    shipping = 1.5,
    sales_tax = 0.95
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

We use [NUnit](https://github.com/nunit/nunit) and [WireMock.Net](https://github.com/WireMock-Net/WireMock.Net) for testing. Before running the specs, create a `.env` file inside the `Taxjar.Tests` directory with your sandbox API key:

```
TAXJAR_API_KEY=YOUR_TAXJAR_SANDBOX_API_KEY
```

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
