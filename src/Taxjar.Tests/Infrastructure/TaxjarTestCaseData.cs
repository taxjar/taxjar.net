using System.Collections;
using Taxjar;
using Taxjar.Tests;
using Taxjar.Tests.Fixtures;

namespace TaxJar.Tests;

public static class TaxjarTestCaseData
{

    public static IEnumerable CustomerTestCases
    {
        get
        {
            const string testPrefix = "when_updating_a_customer_with_missing_";

            yield return new TestCaseData((TaxjarFakes.FakeTaxjarCustomerRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { CustomerId = string.Empty }, "Customer ID cannot be null or an empty string*"))
            .SetName($"{testPrefix}{nameof(Customer.CustomerId)}");
            yield return new TestCaseData((TaxjarFakes.FakeTaxjarCustomerRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { Name = string.Empty }, "*Invalid customer.*Name*"))
             .SetName($"{testPrefix}{nameof(Customer.Name)}");
            yield return new TestCaseData((TaxjarFakes.FakeTaxjarCustomerRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { ExemptionType = string.Empty }, "*Invalid customer.*ExemptionType*"))
              .SetName($"{testPrefix}{nameof(Customer.ExemptionType)}");
            yield return new TestCaseData((TaxjarFakes.FakeTaxjarCustomerRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { CustomerId = TaxjarFakes.Faker.Random.AlphaNumeric(7), Name = string.Empty, ExemptionType = string.Empty }, "Invalid Customer. Name and/or ExemptionType cannot be null or empty.*"))
             .SetName($"{testPrefix}all_required");

        }
    }
    public static IEnumerable RatesTestCasesAsync
    {
        get
        {
            const string testPrefix = "when_showing_tax_rates_for_a_location";

            yield return new TestCaseData("rates/rates.json").SetName($"{testPrefix}_async");
            yield return new TestCaseData("rates/rates_sst.json").SetName($"{testPrefix}_sst_async");
            yield return new TestCaseData("rates/rates_ca.json").SetName($"{testPrefix}_ca_async");
            yield return new TestCaseData("rates/rates_au.json").SetName($"{testPrefix}_au_async");

        }
    }

    public static IEnumerable TaxjarTaxCalculationRequestTestCases
    {
        get
        {
            const string testPrefix = "when_calculating_sales_tax_for_an_order_with_missing_";
            const string errorMessagePrefix = $"Invalid {nameof(TaxjarTaxCalculationRequest)}";

            yield return new TestCaseData((TaxjarFakes.FakeTaxjarTaxCalculationRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { ToCountry = string.Empty }, $"*{errorMessagePrefix}*ToCountry*"))
                .SetName($"{testPrefix}{nameof(TaxjarTaxCalculationRequest.ToCountry)}");
            yield return new TestCaseData((TaxjarFakes.FakeTaxjarTaxCalculationRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { ToZip = string.Empty }, $"*{errorMessagePrefix}*ToZip*"))
                .SetName($"{testPrefix}{nameof(TaxjarTaxCalculationRequest.ToZip)}");
            yield return new TestCaseData((TaxjarFakes.FakeTaxjarTaxCalculationRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { ToState = string.Empty }, $"*{errorMessagePrefix}*ToState*"))
                .SetName($"{testPrefix}{nameof(TaxjarTaxCalculationRequest.ToState)}");
            yield return new TestCaseData((TaxjarFakes.FakeTaxjarTaxCalculationRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { Shipping = null }, $"*{errorMessagePrefix}*Shipping*"))
                .SetName($"{testPrefix}{nameof(TaxjarTaxCalculationRequest.Shipping)}");
            yield return new TestCaseData((TaxjarFakes.FakeTaxjarTaxCalculationRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { Amount = null, LineItems = null }, $"*{errorMessagePrefix}*Either Amount or LineItems*"))
                .SetName($"{testPrefix}Amount_or_LineItems");
            yield return new TestCaseData((new TaxjarTaxCalculationRequest(), $"{errorMessagePrefix}. ToCountry, ToZip, ToState, and Shipping cannot be null or empty.*Additionally, either Amount or LineItems is required.*"))
                .SetName($"{testPrefix}all_required");
        }
    }

    public static IEnumerable TaxjarCreateOrderRequestTestCases
    {
        get
        {
            const string testPrefix = "when_creating_an_order_transaction_with_missing_";
            const string errorMessagePrefix = $"Invalid {nameof(TaxjarOrderRequest)}";

            yield return new TestCaseData((TaxjarFakes.FakeTaxjarCreateOrderRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { TransactionId = string.Empty }, $"*{errorMessagePrefix}*TransactionId*"))
                .SetName($"{testPrefix}{nameof(TaxjarOrderRequest.TransactionId)}");
            yield return new TestCaseData((TaxjarFakes.FakeTaxjarCreateOrderRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { TransactionDate = null }, $"*{errorMessagePrefix}*TransactionDate*"))
                .SetName($"{testPrefix}{nameof(TaxjarOrderRequest.TransactionDate)}");
            yield return new TestCaseData((TaxjarFakes.FakeTaxjarCreateOrderRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { ToCountry = string.Empty}, $"*{errorMessagePrefix}*ToCountry*"))
                .SetName($"{testPrefix}{nameof(TaxjarOrderRequest.ToCountry)}");
            yield return new TestCaseData((TaxjarFakes.FakeTaxjarCreateOrderRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { ToZip = string.Empty }, $"*{errorMessagePrefix}*ToZip*"))
                .SetName($"{testPrefix}{nameof(TaxjarOrderRequest.ToZip)}");
            yield return new TestCaseData((TaxjarFakes.FakeTaxjarCreateOrderRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { ToState = string.Empty }, $"*{errorMessagePrefix}*ToState*"))
                .SetName($"{testPrefix}{nameof(TaxjarOrderRequest.ToState)}");
            yield return new TestCaseData((TaxjarFakes.FakeTaxjarCreateOrderRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { Amount = null}, $"*{errorMessagePrefix}*Amount*"))
                .SetName($"{testPrefix}{nameof(TaxjarOrderRequest.Amount)}");
            yield return new TestCaseData((TaxjarFakes.FakeTaxjarCreateOrderRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { Shipping = null }, $"*{errorMessagePrefix}*Shipping*"))
                .SetName($"{testPrefix}{nameof(TaxjarOrderRequest.Shipping)}");
            yield return new TestCaseData((TaxjarFakes.FakeTaxjarCreateOrderRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { SalesTax = null}, $"*{errorMessagePrefix}*SalesTax*"))
                .SetName($"{testPrefix}{nameof(TaxjarOrderRequest.SalesTax)}");
            yield return new TestCaseData((new TaxjarOrderRequest(), $"{errorMessagePrefix}. TransactionId, TransactionDate, ToCountry, ToZip, ToState, Amount, Shipping, and SalesTax cannot be null or empty.*"))
                .SetName($"{testPrefix}all_required");
        }
    }

    public static IEnumerable TaxjarRefundRequestTestCases
    {
        get
        {
            const string testPrefix = "when_creating_a_refund_transaction_with_missing_";
            const string errorMessagePrefix = $"Invalid {nameof(TaxjarRefundRequest)}";

            yield return new TestCaseData((TaxjarFakes.FakeTaxjarRefundRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { TransactionId = string.Empty }, $"*{errorMessagePrefix}*TransactionId*"))
                .SetName($"{testPrefix}{nameof(TaxjarRefundRequest.TransactionId)}");
            yield return new TestCaseData((TaxjarFakes.FakeTaxjarRefundRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { TransactionReferenceId = string.Empty }, $"*{errorMessagePrefix}*TransactionReferenceId*"))
                .SetName($"{testPrefix}{nameof(TaxjarRefundRequest.TransactionReferenceId)}");
            yield return new TestCaseData((TaxjarFakes.FakeTaxjarRefundRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { ToCountry = string.Empty}, $"*{errorMessagePrefix}*ToCountry*"))
                .SetName($"{testPrefix}{nameof(TaxjarRefundRequest.ToCountry)}");
            yield return new TestCaseData((TaxjarFakes.FakeTaxjarRefundRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { ToState = string.Empty }, $"*{errorMessagePrefix}*ToState*"))
                    .SetName($"{testPrefix}{nameof(TaxjarRefundRequest.ToState)}");
            yield return new TestCaseData((TaxjarFakes.FakeTaxjarRefundRequest().UseSeed(TaxjarFakes.RandomSeed).Generate() with { ToZip = string.Empty }, $"*{errorMessagePrefix}*ToZip*"))
                    .SetName($"{testPrefix}{nameof(TaxjarRefundRequest.ToZip)}");
            yield return new TestCaseData((new TaxjarRefundRequest(), $"{errorMessagePrefix}. TransactionId, TransactionReferenceId, ToCountry, ToZip, and ToState cannot be null or empty.*"))
                .SetName($"{testPrefix}all_required");
        }
    }

}
