namespace Taxjar;

public static class TaxjarRequestExtensions
{
    public static TaxjarCustomerRequest ToRequest(this Customer customer) => new TaxjarCustomerRequest
    {
        CustomerId = customer.CustomerId,
        ExemptionType = customer.ExemptionType,
        Name = customer.Name,
        ExemptRegions = customer.ExemptRegions,
        Country = customer.Country,
        State = customer.State,
        Zip = customer.Zip,
        City = customer.City,
        Street = customer.Street
    };

    public static TaxjarOrderRequest ToRequest(this Order order) => new TaxjarOrderRequest
    {
        TransactionId = order.TransactionId,
        TransactionDate = order.TransactionDate!.Value,
        Provider = order.Provider,
        ExemptionType = order.ExemptionType,
        FromCountry = order.FromCountry,
        FromState = order.FromState,
        FromCity = order.FromCity,
        FromZip = order.FromZip,
        FromStreet = order.FromStreet,
        ToCountry = order.ToCountry,
        ToState = order.ToState,
        ToCity = order.ToCity,
        ToZip = order.ToZip,
        Amount = order.Amount!.Value,
        Shipping = order.Shipping!.Value,
        SalesTax = order.SalesTax!.Value,
        CustomerId = order.CustomerId,
        LineItems = order.LineItems
    };

    public static TaxjarRefundRequest ToRequest(this Refund refund) => new TaxjarRefundRequest
    {
        TransactionId = refund.TransactionId,
        TransactionReferenceId = refund.TransactionReferenceId,
        TransactionDate = refund.TransactionDate,
        Provider = refund.Provider,
        ExemptionType = refund.ExemptionType,
        FromCountry = refund.FromCountry,
        FromState = refund.FromState,
        FromCity = refund.FromCity,
        FromZip = refund.FromZip,
        FromStreet = refund.FromStreet,
        ToCountry = refund.ToCountry,
        ToState = refund.ToState,
        ToCity = refund.ToCity,
        ToZip = refund.ToZip,
        Amount = refund.Amount,
        Shipping = refund.Shipping,
        SalesTax = refund.SalesTax,
        CustomerId = refund.CustomerId,
        LineItems = refund.LineItems
    };

}