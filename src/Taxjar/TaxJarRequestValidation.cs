using System;
using System.Collections.Generic;
using System.Linq;
using Taxjar.Infrastructure;

namespace Taxjar;

public static class TaxjarRequestValidation
{

    private static string FormatErrorMessage(string objectName, List<string> invalidValues)
    {
       
        invalidValues.RemoveAll(string.IsNullOrWhiteSpace);

        if (invalidValues.Count == 1)
        {
            return string.Format("Invalid {0}. {1} cannot be null or empty.", objectName, invalidValues[0]);
        }

        if (invalidValues.Count == 2)
        {
            return string.Format("Invalid {0}. {1} and/or {2} cannot be null or empty.", objectName, invalidValues[0], invalidValues[1]);
        }

        return string.Format("Invalid {0}. {1}, and {2} cannot be null or empty.", objectName, string.Join(", ", invalidValues.Take(invalidValues.Count - 1)), invalidValues[^1]);
    }

    public static void ValidateCustomerId(string customerId)
    {
        if (string.IsNullOrWhiteSpace(customerId))
        {
            throw new ArgumentException(ErrorMessage.MissingCustomerId);
        }
    }

    public static void ValidateCustomer(TaxjarCustomerRequest customer)
    {
        ValidateCustomerId(customer.CustomerId);
        List<string> invalidValues = new List<string>();
        
        if (string.IsNullOrWhiteSpace(customer.Name))
        {
            invalidValues.Add(nameof(customer.Name));
        }

        if (string.IsNullOrWhiteSpace(customer.ExemptionType))
        {
            invalidValues.Add(nameof(customer.ExemptionType));
        }

        if (invalidValues.Count > 0)
        {
            throw new ArgumentException(FormatErrorMessage(nameof(Customer), invalidValues), nameof(customer));
        }
    }

    public static void ValidateZip(string? zip)
    {
        if (string.IsNullOrWhiteSpace(zip))
        {
            throw new ArgumentNullException(nameof(zip), "Zip is null or empty!");
        }
    }

    public static void ValidateTaxjarTaxCalculationRequest(TaxjarTaxCalculationRequest taxjarTaxCalculationRequest)
    {
        List<string> invalidValues = new List<string>();
        if (string.IsNullOrWhiteSpace(taxjarTaxCalculationRequest.ToCountry))
        {
            invalidValues.Add(nameof(TaxjarTaxCalculationRequest.ToCountry));
        }

        if (string.IsNullOrWhiteSpace(taxjarTaxCalculationRequest.ToZip))
        {
            invalidValues.Add(nameof(TaxjarTaxCalculationRequest.ToZip));
        }

        if (string.IsNullOrWhiteSpace(taxjarTaxCalculationRequest.ToState))
        {
            invalidValues.Add(nameof(TaxjarTaxCalculationRequest.ToState));
        }

        if (taxjarTaxCalculationRequest.Shipping is null)
        {
            invalidValues.Add(nameof(TaxjarTaxCalculationRequest.Shipping));
        }

        var amountErrorMessage = string.Empty;
        if (taxjarTaxCalculationRequest.Amount is null && !(taxjarTaxCalculationRequest.LineItems?.Count > 0))
        {
            if (invalidValues.Count == 0)
            {
                throw new ArgumentException(FormatErrorMessage(nameof(TaxjarTaxCalculationRequest), new List<string> { string.Format("Either {0} or {1}", nameof(TaxjarTaxCalculationRequest.Amount), nameof(TaxjarTaxCalculationRequest.LineItems)) }), nameof(taxjarTaxCalculationRequest));
            }

            amountErrorMessage = string.Format(" Additionally, either {0} or {1} is required.", nameof(TaxjarTaxCalculationRequest.Amount), nameof(TaxjarTaxCalculationRequest.LineItems));
            invalidValues.Add(string.Empty);
        }

        if (invalidValues.Count > 0)
        {
            throw new ArgumentException(string.Concat(FormatErrorMessage(nameof(TaxjarTaxCalculationRequest), invalidValues), amountErrorMessage), nameof(taxjarTaxCalculationRequest));
        }
    }

    public static void ValidateTransactionId(string transactionId)
    {
        if (string.IsNullOrWhiteSpace(transactionId))
        {
            throw new ArgumentException(ErrorMessage.MissingTransactionId);
        }
    }

    public static void ValidateTaxjarCreateOrderRequest(TaxjarOrderRequest taxjarCreateOrderRequest)
    {
        List<string> invalidValues = new List<string>();
        var transactionIdIsValid = ValidateTaxjarOrderRequestTransactionId(taxjarCreateOrderRequest);
        if (!transactionIdIsValid.isValid)
        {
            invalidValues.Add(transactionIdIsValid.propertyName);
        }

        if (taxjarCreateOrderRequest.TransactionDate is null)
        {
            invalidValues.Add(nameof(taxjarCreateOrderRequest.TransactionDate));
        }

        if (string.IsNullOrWhiteSpace(taxjarCreateOrderRequest.ToCountry))
        {
            invalidValues.Add(nameof(taxjarCreateOrderRequest.ToCountry));
        }

        if (string.IsNullOrWhiteSpace(taxjarCreateOrderRequest.ToZip))
        {
            invalidValues.Add(nameof(taxjarCreateOrderRequest.ToZip));
        }

        if (string.IsNullOrWhiteSpace(taxjarCreateOrderRequest.ToState))
        {
            invalidValues.Add(nameof(taxjarCreateOrderRequest.ToState));
        }

        if (taxjarCreateOrderRequest.Amount is null)
        {
            invalidValues.Add(nameof(taxjarCreateOrderRequest.Amount));
        }

        if (taxjarCreateOrderRequest.Shipping is null)
        {
            invalidValues.Add(nameof(taxjarCreateOrderRequest.Shipping));
        }

        if (taxjarCreateOrderRequest.SalesTax is null)
        {
            invalidValues.Add(nameof(taxjarCreateOrderRequest.SalesTax));
        }

        if (invalidValues.Count > 0)
        {
            throw new ArgumentException(FormatErrorMessage(nameof(TaxjarOrderRequest), invalidValues), nameof(taxjarCreateOrderRequest));
        }
    }

    public static void ValidateTaxjarUpdateOrderRequest(TaxjarOrderRequest taxjarCreateOrderRequest)
    {
        List<string> invalidValues = new List<string>();
        var transactionIdIsValid = ValidateTaxjarOrderRequestTransactionId(taxjarCreateOrderRequest);
        if (!transactionIdIsValid.isValid)
        {
            invalidValues.Add(transactionIdIsValid.propertyName);
        }

        if (invalidValues.Count > 0)
        {
            throw new ArgumentException(FormatErrorMessage(nameof(TaxjarOrderRequest), invalidValues), nameof(taxjarCreateOrderRequest));
        }
    }

    private static (bool isValid, string propertyName) ValidateTaxjarOrderRequestTransactionId(TaxjarOrderRequest taxjarCreateOrderRequest)
    {
        if (string.IsNullOrWhiteSpace(taxjarCreateOrderRequest.TransactionId))
        {
            return (false, nameof(taxjarCreateOrderRequest.TransactionId));
        }
        return (true, string.Empty);
    }

    public static void ValidateTaxjarRefundRequest(TaxjarRefundRequest taxjarRefundRequest)
    {
        List<string> invalidValues = [.. ValidateTaxjarRefundRequestTransactionId(taxjarRefundRequest) ?? new List<string>()];

        if (string.IsNullOrWhiteSpace(taxjarRefundRequest.ToCountry))
        {
            invalidValues.Add(nameof(taxjarRefundRequest.ToCountry));
        }

        if (string.IsNullOrWhiteSpace(taxjarRefundRequest.ToZip))
        {
            invalidValues.Add(nameof(taxjarRefundRequest.ToZip));
        }

        if (string.IsNullOrWhiteSpace(taxjarRefundRequest.ToState))
        {
            invalidValues.Add(nameof(taxjarRefundRequest.ToState));
        }

        if (invalidValues.Count > 0)
        {
            throw new ArgumentException(FormatErrorMessage(nameof(TaxjarRefundRequest), invalidValues), nameof(taxjarRefundRequest));
        }
    }

    public static List<string>? ValidateTaxjarRefundRequestTransactionId(TaxjarRefundRequest taxjarRefundRequest)
    {
        if(!string.IsNullOrWhiteSpace(taxjarRefundRequest.TransactionId) && !string.IsNullOrWhiteSpace(taxjarRefundRequest.TransactionReferenceId))
        {
            return null;
        }

        List<string> invalidValues = new List<string>();

        if (string.IsNullOrWhiteSpace(taxjarRefundRequest.TransactionId))
        {
            invalidValues.Add(nameof(taxjarRefundRequest.TransactionId));
        }

        if (string.IsNullOrWhiteSpace(taxjarRefundRequest.TransactionReferenceId))
        {
            invalidValues.Add(nameof(taxjarRefundRequest.TransactionReferenceId));
        }

        return invalidValues;
    }
    
    public static void ValidateVat(string varNumber)
    {
        if (string.IsNullOrWhiteSpace(varNumber))
        {
            throw new ArgumentException(ErrorMessage.MissingCustomerVat);
        }
    }
}
