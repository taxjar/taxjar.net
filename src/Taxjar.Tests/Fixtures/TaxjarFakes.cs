namespace Taxjar.Tests.Fixtures;

public static class TaxjarFakes
{
    public static readonly Bogus.Faker Faker = new Bogus.Faker();
    private static List<string> exemptionTypes = new(){
    "wholesale", "government", "other", "non_exempt"
    };
    private static List<string> taxSources = new(){
    "origin", "destination "
    };
    public static int RandomSeed = 556;


    public static Bogus.Faker<CustomerResponse> FakeCustomerResponse()
    {
        return new Bogus.Faker<CustomerResponse>()
        .RuleFor(cr => cr.Customer, f => FakeCustomerResponseAttributes().Generate());
    }
    public static Bogus.Faker<Customer> FakeCustomer()
    {
        return new Bogus.Faker<Customer>()
          .RuleFor(c => c.CustomerId, f => f.Random.Number(5_000, 10_000).ToString())
          .RuleFor(c => c.ExemptionType, f => f.PickRandom(exemptionTypes))
          .RuleFor(c => c.ExemptRegions, f => Enumerable.Range(0, f.Random.Int(0, 3)).Select(_ => FakeExemptRegion().Generate()).ToList())
          .RuleFor(c => c.Name, f => f.Person.FullName)
          .RuleFor(c => c.Country, f => f.Address.CountryCode())
          .RuleFor(c => c.State, f => f.Address.StateAbbr())
          .RuleFor(c => c.City, f => f.Address.City())
          .RuleFor(c => c.Zip, f => f.Address.ZipCode("#####"))
          .RuleFor(c => c.Street, f => f.Address.StreetAddress());
    }

    public static Bogus.Faker<TaxjarCustomerRequest> FakeTaxjarCustomerRequest()
    {
        return new Bogus.Faker<TaxjarCustomerRequest>()
          .RuleFor(c => c.CustomerId, f => f.Random.Number(5_000, 10_000).ToString())
          .RuleFor(c => c.ExemptionType, f => f.PickRandom(exemptionTypes))
          .RuleFor(c => c.ExemptRegions, f => Enumerable.Range(0, f.Random.Int(0, 3)).Select(_ => FakeExemptRegion().Generate()).ToList())
          .RuleFor(c => c.Name, f => f.Person.FullName)
          .RuleFor(c => c.Country, f => f.Address.CountryCode())
          .RuleFor(c => c.State, f => f.Address.StateAbbr())
          .RuleFor(c => c.City, f => f.Address.City())
          .RuleFor(c => c.Zip, f => f.Address.ZipCode("#####"))
          .RuleFor(c => c.Street, f => f.Address.StreetAddress());
    }

    public static Bogus.Faker<CustomerResponseAttributes> FakeCustomerResponseAttributes()
    {
        return new Bogus.Faker<CustomerResponseAttributes>()
          .RuleFor(c => c.CustomerId, f => f.Random.Number(5_000, 10_000).ToString())
          .RuleFor(c => c.ExemptionType, f => f.PickRandom(exemptionTypes))
          .RuleFor(c => c.ExemptRegions, f => Enumerable.Range(0, f.Random.Int(0, 3)).Select(_ => FakeExemptRegion().Generate()).ToList())
          .RuleFor(c => c.Name, f => f.Person.FullName)
          .RuleFor(c => c.Country, f => f.Address.CountryCode())
          .RuleFor(c => c.State, f => f.Address.StateAbbr())
          .RuleFor(c => c.City, f => f.Address.City())
          .RuleFor(c => c.Zip, f => f.Address.ZipCode("#####"))
          .RuleFor(c => c.Street, f => f.Address.StreetAddress());
    }
    public static Bogus.Faker<ExemptRegion> FakeExemptRegion()
    {
        return new Bogus.Faker<ExemptRegion>()
          .RuleFor(er => er.Country, f => f.Address.CountryCode())
          .RuleFor(er => er.State, f => f.Address.StateAbbr());
    }

    public static Bogus.Faker<RateResponse> FakeRateResponse()
    {
        return new Bogus.Faker<RateResponse>()
        .RuleFor(rr => rr.Rate, FakeRateResponseAttributes().Generate());
    }

    public static Bogus.Faker<RateResponseAttributes> FakeRateResponseAttributes()
    {
        return new Bogus.Faker<RateResponseAttributes>()
        .RuleFor(rra => rra.Zip, f => f.Address.ZipCode("#####"))
        .RuleFor(rra => rra.City, f => f.Address.City())
        .RuleFor(rra => rra.CityRate, f => f.Random.Decimal(0.02m, 0.25m))
        .RuleFor(rra => rra.State, f => f.Address.StateAbbr())
        .RuleFor(rra => rra.StateRate, (f, ctx) => f.Random.Decimal(0.01m, ctx.CityRate - 0.01m))
        .RuleFor(rra => rra.County, f => f.Address.CountryCode())
        .RuleFor(rra => rra.CountryRate, (f, ctx) => f.Random.Decimal(0.01m, ctx.StateRate - 0.01m))
        .RuleFor(rra => rra.CombinedDistrictRate, (f, ctx) => ctx.CityRate + ctx.StateRate + ctx.CountyRate)
        .RuleFor(rra => rra.CombinedRate, (f, ctx) => ctx.CombinedDistrictRate)
        .RuleFor(rra => rra.FreightTaxable, f => f.Random.Bool())
        .RuleFor(rra => rra.Country, f => f.Address.CountryCode())
        .RuleFor(rra => rra.Name, f => f.Company.CompanyName())
        .RuleFor(rra => rra.CountryRate, f => f.Random.Decimal(0.15m, 0.42m))
        .RuleFor(rra => rra.StandardRate, f => f.Random.Decimal(0.02m, 0.25m))
        .RuleFor(rra => rra.ReducedRate, (f, ctx) => ctx.StandardRate - f.Random.Decimal(0.1m, ctx.StandardRate))
        .RuleFor(rra => rra.SuperReducedRate, (f, ctx) => ctx.ReducedRate - f.Random.Decimal(0.1m, ctx.ReducedRate))
        .RuleFor(rra => rra.ParkingRate, f => f.Random.Decimal(0.02m, 0.05m))
        .RuleFor(rra => rra.DistanceSaleThreshold, f => f.Random.Decimal(1.0m, 99.99m));
    }

    public static Bogus.Faker<Rate> FakeRate()
    {
        return new Bogus.Faker<Rate>()
        .RuleFor(r => r.Country, f => f.Address.CountryCode())
        .RuleFor(r => r.Zip, f => f.Address.ZipCode("#####"))
        .RuleFor(r => r.Country, f => f.Address.StateAbbr())
        .RuleFor(r => r.Country, f => f.Address.City())
        .RuleFor(r => r.Country, f => f.Address.StreetAddress());
    }

    public static Bogus.Faker<NexusRegionsResponse> FakeNexusRegionsResponse()
    {
        return new Bogus.Faker<NexusRegionsResponse>()
        .RuleFor(nrr => nrr.Regions, f => FakeNexusRegion().Generate(f.Random.Number(3, 5)));
    }

    public static Bogus.Faker<NexusRegion> FakeNexusRegion()
    {
        return new Bogus.Faker<NexusRegion>()
        .RuleFor(nr => nr.CountryCode, f => f.Address.CountryCode())
        .RuleFor(nr => nr.Country, f => f.Address.Country())
        .RuleFor(nr => nr.RegionCode, f => f.Address.StateAbbr())
        .RuleFor(nr => nr.Region, f => f.Address.State());
    }

    public static Bogus.Faker<SummaryRatesResponse> FakeSummaryRatesResponse()
    {
        return new Bogus.Faker<SummaryRatesResponse>()
        .RuleFor(nrr => nrr.SummaryRates, f => FakeSummaryRate().Generate(f.Random.Number(3, 5)));
    }

    public static Bogus.Faker<SummaryRate> FakeSummaryRate()
    {
        return new Bogus.Faker<SummaryRate>()
        .RuleFor(sr => sr.CountryCode, f => f.Address.CountryCode())
        .RuleFor(sr => sr.Country, f => f.Address.Country())
        .RuleFor(sr => sr.RegionCode, f => f.Address.StateAbbr())
        .RuleFor(sr => sr.Region, f => f.Address.State());
    }

    public static Bogus.Faker<SummaryRateObject> FakeSummaryRateObject()
    {
        return new Bogus.Faker<SummaryRateObject>()
        .RuleFor(sro => sro.Label, f => "Tax")
        .RuleFor(sro => sro.Rate, f => f.Random.Decimal(0.02m, 0.25m));
    }

    public static Bogus.Faker<TaxjarTaxCalculationRequest> FakeTaxjarTaxCalculationRequest(bool generateLineItems = false)
    {
        return new Bogus.Faker<TaxjarTaxCalculationRequest>()
        .RuleFor(tcr => tcr.ExemptionType, f => f.PickRandom(exemptionTypes))
        .RuleFor(tcr => tcr.FromCountry, f => f.Address.CountryCode())
        .RuleFor(tcr => tcr.FromZip, f => f.Address.ZipCode("#####"))
        .RuleFor(tcr => tcr.FromState, f => f.Address.StateAbbr())
        .RuleFor(tcr => tcr.FromCity, f => f.Address.City())
        .RuleFor(tcr => tcr.FromStreet, f => f.Address.StreetAddress())
        .RuleFor(tcr => tcr.ToCountry, (f, ctx) => ctx.FromCountry)
        .RuleFor(tcr => tcr.ToZip, f => f.Address.ZipCode("#####"))
        .RuleFor(tcr => tcr.ToState, f => f.Address.StateAbbr())
        .RuleFor(tcr => tcr.ToCity, f => f.Address.City())
        .RuleFor(tcr => tcr.ToStreet, f => f.Address.StreetAddress())
        .RuleFor(tcr => tcr.Amount, f => !generateLineItems ? f.Finance.Amount(9.99m, 9_999.99m) : null)
        .RuleFor(tcr => tcr.Shipping, f => f.Finance.Amount(9.99m, 100m))
        .RuleFor(tcr => tcr.CustomerId, f => f.Random.AlphaNumeric(15))
        .RuleFor(tcr => tcr.LineItems, f => generateLineItems ? FakeTaxLineItem().Generate(f.Random.Number(1, 5)) : null)
        .RuleFor(tcr => tcr.NexusAddresses, f => NexusAddress().Generate(1));
    }

    public static Bogus.Faker<NexusAddress> NexusAddress()
    {
        return new Bogus.Faker<NexusAddress>()
        .StrictMode(false)
        .RuleFor(na => na.Id, f => f.Random.AlphaNumeric(12))
        .RuleFor(na => na.Country, f => f.Address.CountryCode())
        .RuleFor(na => na.Zip, f => f.Address.ZipCode("#####-####"))
        .RuleFor(na => na.State, f => f.Address.StateAbbr())
        .RuleFor(na => na.City, f => f.Address.City())
        .RuleFor(na => na.Street, f => f.Address.StreetAddress());
    }

    public static Bogus.Faker<LineItem> FakeLineItem(bool hasSalesTax = false)
    {
        return new Bogus.Faker<LineItem>()
       .StrictMode(false)
       .RuleFor(li => li.Id, f => f.Random.AlphaNumeric(12))
       .RuleFor(li => li.Quantity, f => f.Random.Number(1, 99))
       .RuleFor(li => li.ProductIdentifier, f => f.Commerce.Ean13())
       .RuleFor(li => li.Description, f => f.Commerce.ProductName())
       .RuleFor(li => li.ProductTaxCode, f => f.Random.Number(1_000, 9_999).ToString())
       .RuleFor(li => li.UnitPrice, f => f.Finance.Amount(1.99m, 199.99m))
       .RuleFor(li => li.Discount, (f, ctx) => f.Finance.Amount(0m, 0.33m) * ctx.UnitPrice * ctx.Quantity)
       .RuleFor(li => li.SalesTax, (f, ctx) => hasSalesTax ? f.Finance.Amount(0.01m, 0.25m) * ctx.UnitPrice * ctx.Quantity : null);
    }


    public static Bogus.Faker<TaxLineItem> FakeTaxLineItem(bool hasSalesTax = false)
    {
        return new Bogus.Faker<TaxLineItem>()
       .StrictMode(false)
       .RuleFor(li => li.Id, f => f.Random.AlphaNumeric(12))
       .RuleFor(li => li.Quantity, f => f.Random.Number(1, 99))
       .RuleFor(li => li.ProductTaxCode, f => f.Random.Number(1_000, 9_999).ToString())
       .RuleFor(li => li.UnitPrice, f => f.Finance.Amount(1.99m, 199.99m))
       .RuleFor(li => li.Discount, (f, ctx) => f.Finance.Amount(0m, 0.33m) * ctx.UnitPrice * ctx.Quantity);
    }
    public static Bogus.Faker<TaxResponse> FakeTaxResponse(bool generateBreakdown = false) => new Bogus.Faker<TaxResponse>()
    .RuleFor(tr => tr.Tax, f => FakeTaxResponseAttributes(generateBreakdown).Generate());

    public static Bogus.Faker<TaxResponseAttributes> FakeTaxResponseAttributes(bool generateBreakdown = false) => new Bogus.Faker<TaxResponseAttributes>()
    .StrictMode(false)
    .RuleFor(tra => tra.OrderTotalAmount, f => f.Finance.Amount(1.99m, 199.99m))
    .RuleFor(tra => tra.Shipping, f => f.Finance.Amount(0m, 15m))
    .RuleFor(tra => tra.TaxableAmount, f => f.Finance.Amount())
    .RuleFor(tra => tra.AmountToCollect, (f, ctx) => ctx.TaxableAmount)
    .RuleFor(tra => tra.Rate, (f, ctx) => ctx.AmountToCollect / ctx.OrderTotalAmount)
    .RuleFor(tra => tra.HasNexus, f => f.Random.Bool())
    .RuleFor(tra => tra.FreightTaxable, f => f.Random.Bool())
    .RuleFor(tra => tra.TaxSource, f => f.PickRandom(taxSources))
    .RuleFor(tra => tra.ExemptionType, f => f.PickRandom(exemptionTypes))
    .RuleFor(tra => tra.Jurisdictions, f => FakeTaxJurisdictions().Generate())
    .RuleFor(tra => tra.Breakdown, f => generateBreakdown ? FakeTaxBreakdowns().Generate() : null);

    public static Bogus.Faker<TaxJurisdictions> FakeTaxJurisdictions() => new Bogus.Faker<TaxJurisdictions>()
     .RuleFor(tra => tra.Country, f => f.Address.CountryCode())
     .RuleFor(tra => tra.County, f => f.Address.County())
     .RuleFor(tra => tra.State, f => f.Address.StateAbbr())
     .RuleFor(tra => tra.City, f => f.Address.City());
    public static Bogus.Faker<TaxBreakdown> FakeTaxBreakdowns() => new Bogus.Faker<TaxBreakdown>()
    .RuleFor(tb => tb.StateTaxRate, f => f.Random.Decimal(0.10m, 0.15m))
    .RuleFor(tb => tb.StateTaxCollectable, f => f.Finance.Amount(1, 500, 3))
    .RuleFor(tb => tb.CountyTaxCollectable, (f, ctx) => f.Finance.Amount(1, ctx.StateTaxCollectable))
    .RuleFor(tb => tb.CityTaxCollectable, (f, ctx) => f.Finance.Amount(1, ctx.CountyTaxCollectable))
    .RuleFor(tb => tb.SpecialDistrictTaxableAmount, (f, ctx) => f.Finance.Amount(1, ctx.CityTaxCollectable))
    .RuleFor(tb => tb.SpecialDistrictTaxRate, f => f.Random.Decimal(0.10m, 0.15m))
    .RuleFor(tb => tb.SpecialDistrictTaxCollectable, (f, ctx) => f.Finance.Amount(1, ctx.SpecialDistrictTaxableAmount))
    .RuleFor(tb => tb.Shipping, f => FakeTaxBreakdownShipping().Generate())
    .RuleFor(tb => tb.LineItems, f => FakeTaxBreakdownLineItem().Generate(f.Random.Number(1, 5)));

    public static Bogus.Faker<TaxBreakdownShipping> FakeTaxBreakdownShipping() => new Bogus.Faker<TaxBreakdownShipping>()
    .RuleFor(tbs => tbs.StateSalesTaxRate, f => f.Random.Decimal(0.10m, 0.15m))
    .RuleFor(tbs => tbs.StateAmount, f => f.Finance.Amount(1, 500, 3))
    .RuleFor(tbs => tbs.CountyAmount, (f, ctx) => f.Finance.Amount(1, ctx.StateAmount))
    .RuleFor(tbs => tbs.CityAmount, (f, ctx) => f.Finance.Amount(1, ctx.CountyAmount))
    .RuleFor(tbs => tbs.SpecialDistrictTaxableAmount, (f, ctx) => f.Finance.Amount(1, ctx.CityAmount))
    .RuleFor(tbs => tbs.SpecialDistrictTaxRate, f => f.Random.Decimal(0.10m, 0.15m))
    .RuleFor(tbs => tbs.SpecialDistrictAmount, (f, ctx) => f.Finance.Amount(1, ctx.SpecialDistrictTaxableAmount));

    public static Bogus.Faker<TaxBreakdownLineItem> FakeTaxBreakdownLineItem() => new Bogus.Faker<TaxBreakdownLineItem>()
    .RuleFor(tbli => tbli.Id, f => f.Random.AlphaNumeric(0))
    .RuleFor(tbs => tbs.StateSalesTaxRate, f => f.Random.Decimal(0.10m, 0.15m))
    .RuleFor(tbs => tbs.StateAmount, f => f.Finance.Amount(1, 500, 3))
    .RuleFor(tbs => tbs.CountyAmount, (f, ctx) => f.Finance.Amount(1, ctx.StateAmount))
    .RuleFor(tbs => tbs.CityAmount, (f, ctx) => f.Finance.Amount(1, ctx.CountyAmount))
    .RuleFor(tbs => tbs.SpecialDistrictTaxableAmount, (f, ctx) => f.Finance.Amount(1, ctx.CityAmount))
    .RuleFor(tbs => tbs.SpecialTaxRate, f => f.Random.Decimal(0.10m, 0.15m))
    .RuleFor(tbs => tbs.SpecialDistrictAmount, (f, ctx) => f.Finance.Amount(1, ctx.SpecialDistrictTaxableAmount));

    public static Bogus.Faker<OrderResponse> FakeOrderResponse(bool generateLineItems = false) => new Bogus.Faker<OrderResponse>()
    .RuleFor(rsp => rsp.Order, f => FakeOrderResponseAttributes(generateLineItems).Generate());

    public static Bogus.Faker<OrdersResponse> FakeOrdersResponse() => new Bogus.Faker<OrdersResponse>()
    .RuleFor(rsp => rsp.Orders, f => Enumerable.Range(0, f.Random.Int(0, 3)).Select(_ => f.Random.Number(1_000, 5_000).ToString()).ToList());

    public static Bogus.Faker<OrderResponse> FakeDeleteOrderResponse() => new Bogus.Faker<OrderResponse>()
    .StrictMode(false)
   .RuleFor(rsp => rsp.Order, f => FakeDeleteOrderResponseAttributes().Generate());

    public static Bogus.Faker<OrderResponseAttributes> FakeOrderResponseAttributes(bool generateLineItems = false) => new Bogus.Faker<OrderResponseAttributes>()
.RuleFor(order => order.TransactionId, f => f.Random.Number(1_000, 9_000).ToString())
.RuleFor(order => order.UserId, f => f.Random.Number(1_000, 9_000))
.RuleFor(order => order.TransactionDate, f => f.Date.Past(1))
.RuleFor(order => order.Provider, "api")
.RuleFor(order => order.ExemptionType, f => f.PickRandom(exemptionTypes))
.RuleFor(order => order.FromCountry, f => f.Address.CountryCode())
.RuleFor(order => order.FromZip, f => f.Address.ZipCode("#####"))
.RuleFor(order => order.FromState, f => f.Address.StateAbbr())
.RuleFor(order => order.FromCity, f => f.Address.City())
.RuleFor(order => order.FromStreet, f => f.Address.StreetAddress())
.RuleFor(order => order.ToCountry, (f, ctx) => ctx.FromCountry)
.RuleFor(order => order.ToZip, f => f.Address.ZipCode("#####"))
.RuleFor(order => order.ToState, f => f.Address.StateAbbr())
.RuleFor(order => order.ToCity, f => f.Address.City())
.RuleFor(order => order.ToStreet, f => f.Address.StreetAddress())
.RuleFor(order => order.Shipping, f => f.Finance.Amount(0m, 15m))
.RuleFor(order => order.LineItems, f => generateLineItems ? FakeLineItem().Generate(f.Random.Number(1, 5)) : null)
.RuleFor(order => order.Amount, (f, ctx) => !generateLineItems ? f.Finance.Amount(9.99m, 9_999.99m) : ctx.LineItems?.Sum(li => (li.UnitPrice * li.Quantity) - li.Discount) + ctx.Shipping)
.RuleFor(order => order.SalesTax, (f, ctx) => f.Finance.Amount(0.01m, 0.25m) * ctx.Amount);

    public static Bogus.Faker<OrderResponseAttributes> FakeDeleteOrderResponseAttributes() => new Bogus.Faker<OrderResponseAttributes>()
    .StrictMode(false)
    .RuleFor(order => order.TransactionId, f => f.Random.Number(1_000, 9_000).ToString())
    .RuleFor(order => order.UserId, f => f.Random.Number(1_000, 9_000))
    .RuleFor(order => order.Provider, "api");

    public static Bogus.Faker<Order> FakeOrder(bool generateLineItems = false, bool generateCustomerId = false) => new Bogus.Faker<Order>()
    .RuleFor(order => order.TransactionId, f => f.Random.Number(1_000, 9_000).ToString())
    .RuleFor(order => order.TransactionDate, f => f.Date.Past(1))
    .RuleFor(order => order.Provider, "api")
    .RuleFor(order => order.ExemptionType, f => f.PickRandom(exemptionTypes))
    .RuleFor(order => order.FromCountry, f => f.Address.CountryCode())
    .RuleFor(order => order.FromZip, f => f.Address.ZipCode("#####"))
    .RuleFor(order => order.FromState, f => f.Address.StateAbbr())
    .RuleFor(order => order.FromCity, f => f.Address.City())
    .RuleFor(order => order.FromStreet, f => f.Address.StreetAddress())
    .RuleFor(order => order.ToCountry, (f, ctx) => ctx.FromCountry)
    .RuleFor(order => order.ToZip, f => f.Address.ZipCode("#####"))
    .RuleFor(order => order.ToState, f => f.Address.StateAbbr())
    .RuleFor(order => order.ToCity, f => f.Address.City())
    .RuleFor(order => order.ToStreet, f => f.Address.StreetAddress())
    .RuleFor(order => order.Shipping, f => f.Finance.Amount(0m, 15m))
    .RuleFor(order => order.LineItems, f => generateLineItems ? FakeLineItem().Generate(f.Random.Number(1, 5)) : null)
    .RuleFor(order => order.Amount, (f, ctx) => !generateLineItems ? f.Finance.Amount(9.99m, 9_999.99m) : ctx.LineItems?.Sum(li => (li.UnitPrice * li.Quantity) - li.Discount) + ctx.Shipping)
    .RuleFor(order => order.SalesTax, (f, ctx) => f.Finance.Amount(0.01m, 0.25m) * ctx.Amount)
    .RuleFor(order => order.CustomerId, f => generateCustomerId ? f.Random.AlphaNumeric(15) : null);

    public static Bogus.Faker<TaxjarOrderRequest> FakeTaxjarCreateOrderRequest(bool generateLineItems = false, bool generateCustomerId = false) => new Bogus.Faker<TaxjarOrderRequest>()
    .RuleFor(req => req.TransactionId, f => f.Random.Number(1_000, 9_000).ToString())
    .RuleFor(req => req.TransactionDate, f => f.Date.Past(1))
    .RuleFor(req => req.Provider, "api")
    .RuleFor(req => req.ExemptionType, f => f.PickRandom(exemptionTypes))
    .RuleFor(req => req.FromCountry, f => f.Address.CountryCode())
    .RuleFor(req => req.FromZip, f => f.Address.ZipCode("#####"))
    .RuleFor(req => req.FromState, f => f.Address.StateAbbr())
    .RuleFor(req => req.FromCity, f => f.Address.City())
    .RuleFor(req => req.FromStreet, f => f.Address.StreetAddress())
    .RuleFor(req => req.ToCountry, (f, ctx) => ctx.FromCountry)
    .RuleFor(req => req.ToZip, f => f.Address.ZipCode("#####"))
    .RuleFor(req => req.ToState, f => f.Address.StateAbbr())
    .RuleFor(req => req.ToCity, f => f.Address.City())
    .RuleFor(req => req.ToStreet, f => f.Address.StreetAddress())
    .RuleFor(req => req.Shipping, f => f.Finance.Amount(0m, 15m))
    .RuleFor(req => req.LineItems, f => generateLineItems ? FakeLineItem().Generate(f.Random.Number(1, 5)) : null)
    .RuleFor(req => req.Amount, (f, ctx) => !generateLineItems ? f.Finance.Amount(9.99m, 9_999.99m) : ctx.LineItems?.Sum(li => (li.UnitPrice * li.Quantity) - li.Discount) + ctx.Shipping)
    .RuleFor(req => req.SalesTax, (f, ctx) => f.Finance.Amount(0.01m, 0.25m) * ctx.Amount)
    .RuleFor(req => req.CustomerId, f => generateCustomerId ? f.Random.AlphaNumeric(15) : null);

    public static Bogus.Faker<TaxjarRefundRequest> FakeTaxjarRefundRequest(bool generateLineItems = false, bool generateCustomerId = false) => new Bogus.Faker<TaxjarRefundRequest>()
.RuleFor(req => req.TransactionId, f => f.Random.Number(5_000, 9_999).ToString())
.RuleFor(req => req.TransactionReferenceId, f => f.Random.Number(1_000, 4_999).ToString())
.RuleFor(req => req.TransactionDate, f => f.Date.Past(1))
.RuleFor(req => req.Provider, "api")
.RuleFor(req => req.ExemptionType, f => f.PickRandom(exemptionTypes))
.RuleFor(req => req.FromCountry, f => f.Address.CountryCode())
.RuleFor(req => req.FromZip, f => f.Address.ZipCode("#####"))
.RuleFor(req => req.FromState, f => f.Address.StateAbbr())
.RuleFor(req => req.FromCity, f => f.Address.City())
.RuleFor(req => req.FromStreet, f => f.Address.StreetAddress())
.RuleFor(req => req.ToCountry, (f, ctx) => ctx.FromCountry)
.RuleFor(req => req.ToZip, f => f.Address.ZipCode("#####"))
.RuleFor(req => req.ToState, f => f.Address.StateAbbr())
.RuleFor(req => req.ToCity, f => f.Address.City())
.RuleFor(req => req.ToStreet, f => f.Address.StreetAddress())
.RuleFor(req => req.Shipping, f => f.Finance.Amount(0m, 15m))
.RuleFor(req => req.LineItems, f => generateLineItems ? FakeLineItem().Generate(f.Random.Number(1, 5)) : null)
.RuleFor(req => req.Amount, (f, ctx) => !generateLineItems ? f.Finance.Amount(9.99m, 9_999.99m) : ctx.LineItems?.Sum(li => (li.UnitPrice * li.Quantity) - li.Discount) + ctx.Shipping)
.RuleFor(req => req.SalesTax, (f, ctx) => f.Finance.Amount(0.01m, 0.25m) * ctx.Amount)
.RuleFor(req => req.CustomerId, f => generateCustomerId ? f.Random.AlphaNumeric(15) : null);

    public static Bogus.Faker<Refund> FakeRefund(bool generateLineItems = false, bool generateCustomerId = false) => new Bogus.Faker<Refund>()
    .RuleFor(refund => refund.TransactionId, f => f.Random.Number(5_000, 9_999).ToString())
    .RuleFor(refund => refund.TransactionReferenceId, f => f.Random.Number(1_000, 4_999).ToString())
    .RuleFor(refund => refund.TransactionDate, f => f.Date.Past(1))
    .RuleFor(refund => refund.Provider, "api")
    .RuleFor(refund => refund.ExemptionType, f => f.PickRandom(exemptionTypes))
    .RuleFor(refund => refund.FromCountry, f => f.Address.CountryCode())
    .RuleFor(refund => refund.FromZip, f => f.Address.ZipCode("#####"))
    .RuleFor(refund => refund.FromState, f => f.Address.StateAbbr())
    .RuleFor(refund => refund.FromCity, f => f.Address.City())
    .RuleFor(refund => refund.FromStreet, f => f.Address.StreetAddress())
    .RuleFor(refund => refund.ToCountry, (f, ctx) => ctx.FromCountry)
    .RuleFor(refund => refund.ToZip, f => f.Address.ZipCode("#####"))
    .RuleFor(refund => refund.ToState, f => f.Address.StateAbbr())
    .RuleFor(refund => refund.ToCity, f => f.Address.City())
    .RuleFor(refund => refund.ToStreet, f => f.Address.StreetAddress())
    .RuleFor(refund => refund.Shipping, f => f.Finance.Amount(0m, 15m))
    .RuleFor(refund => refund.LineItems, f => generateLineItems ? FakeLineItem().Generate(f.Random.Number(1, 5)) : null)
    .RuleFor(refund => refund.Amount, (f, ctx) => !generateLineItems ? f.Finance.Amount(9.99m, 9_999.99m) : ctx.LineItems?.Sum(li => (li.UnitPrice * li.Quantity) - li.Discount) + ctx.Shipping)
    .RuleFor(refund => refund.SalesTax, (f, ctx) => f.Finance.Amount(0.01m, 0.25m) * ctx.Amount)
    .RuleFor(refund => refund.CustomerId, f => generateCustomerId ? f.Random.AlphaNumeric(15) : null);

    public static Bogus.Faker<RefundResponse> FakeRefundResponse(bool generateLineItems = false, bool generateCustomerId = false) => new Bogus.Faker<RefundResponse>()
    .RuleFor(rsp => rsp.Refund, f => FakeRefundResponseAttributes(generateLineItems: generateLineItems, generateCustomerId: generateCustomerId).Generate());

    public static Bogus.Faker<RefundsResponse> FakeRefundsResponse() => new Bogus.Faker<RefundsResponse>()
.RuleFor(rsp => rsp.Refunds, f => Enumerable.Range(0, f.Random.Int(0, 3)).Select(_ => f.Random.Number(1_000, 5_000).ToString()).ToList());

    public static Bogus.Faker<RefundResponseAttributes> FakeRefundResponseAttributes(bool generateLineItems = false, bool generateCustomerId = false) => new Bogus.Faker<RefundResponseAttributes>()
    .RuleFor(rra => rra.TransactionId, f => f.Random.Number(5_000, 9_999).ToString())
    .RuleFor(rra => rra.TransactionReferenceId, f => f.Random.Number(1_000, 4_999).ToString())
    .RuleFor(rra => rra.TransactionDate, f => f.Date.Past(1))
    .RuleFor(rra => rra.Provider, "api")
    .RuleFor(rra => rra.UserId, f => f.Random.Number(1_000, 9_000))
    .RuleFor(rra => rra.ExemptionType, f => f.PickRandom(exemptionTypes))
    .RuleFor(rra => rra.FromCountry, f => f.Address.CountryCode())
.RuleFor(rra => rra.FromZip, f => f.Address.ZipCode("#####"))
.RuleFor(rra => rra.FromState, f => f.Address.StateAbbr())
.RuleFor(rra => rra.FromCity, f => f.Address.City())
.RuleFor(rra => rra.FromStreet, f => f.Address.StreetAddress())
.RuleFor(rra => rra.ToCountry, (f, ctx) => ctx.FromCountry)
.RuleFor(rra => rra.ToZip, f => f.Address.ZipCode("#####"))
.RuleFor(rra => rra.ToState, f => f.Address.StateAbbr())
.RuleFor(rra => rra.ToCity, f => f.Address.City())
.RuleFor(rra => rra.ToStreet, f => f.Address.StreetAddress())
.RuleFor(rra => rra.Shipping, f => f.Finance.Amount(0m, 15m))
.RuleFor(rra => rra.LineItems, f => generateLineItems ? FakeLineItem().Generate(f.Random.Number(1, 5)) : null)
.RuleFor(rra => rra.Amount, (f, ctx) => !generateLineItems ? f.Finance.Amount(9.99m, 9_999.99m) : ctx.LineItems?.Sum(li => (li.UnitPrice * li.Quantity) - li.Discount) + ctx.Shipping)
.RuleFor(rra => rra.SalesTax, (f, ctx) => f.Finance.Amount(0.01m, 0.25m) * ctx.Amount);

    public static Bogus.Faker<RefundResponse> FakeDeleteRefundResponse() => new Bogus.Faker<RefundResponse>()
    .StrictMode(false)
   .RuleFor(rsp => rsp.Refund, FakeDeleteRefundResponseAttributes().Generate());

    public static Bogus.Faker<RefundResponseAttributes> FakeDeleteRefundResponseAttributes() => new Bogus.Faker<RefundResponseAttributes>()
 .StrictMode(false)
 .RuleFor(rra => rra.TransactionId, f => f.Random.Number(1_000, 9_000).ToString())
 .RuleFor(rra => rra.UserId, f => f.Random.Number(1_000, 9_000))
 .RuleFor(rra => rra.Provider, "api");

    public static Bogus.Faker<AddressValidationResponse> FakeAddressValidationResponse() => new Bogus.Faker<AddressValidationResponse>()
    .RuleFor(avr => avr.Addresses, f => FakeAddress().Generate(f.Random.Number(2, 3)));

    public static Bogus.Faker<Address> FakeAddress() => new Bogus.Faker<Address>()
    .RuleFor(a => a.Street, f => f.Address.StreetAddress())
    .RuleFor(a => a.City, f => f.Address.City())
    .RuleFor(a => a.State, f => f.Address.StateAbbr())
    .RuleFor(a => a.Zip, f => f.Address.ZipCode("#####-####"))
    .RuleFor(a => a.Country, "US");
}