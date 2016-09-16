# TaxJar Sales Tax API for .NET / C#

Official .NET / C# client for Sales Tax API v2. For the REST documentation, please visit [http://developers.taxjar.com/api](http://developers.taxjar.com/api).

## Authentication

```csharp
var client = new TaxjarApi("TAXJAR API KEY");
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
  amount = 17.95,
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
  amount = 17.95,
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
  amount = 17.95,
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
  amount = 17.95,
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