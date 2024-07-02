using FluentAssertions;
using Taxjar.Tests.Fixtures;


namespace Taxjar.Tests;

[TestFixture]
public class ToRequestExtensionsTests
{

    [Test]
    public void when_converting_customer_to_customer_request()
    {
        //arrange
        var customer = TaxjarFakes.FakeCustomer().Generate();

        //act
        var result = customer.ToRequest();

        //assert
        result.CustomerId.Should().Be(customer.CustomerId);
        result.ExemptionType.Should().Be(customer.ExemptionType);
        result.Name.Should().Be(customer.Name);
        result.Country.Should().Be(customer.Country); 
        result.State.Should().Be(customer.State);
        result.Zip.Should().Be(customer.Zip); 
        result.City.Should().Be(customer.City); 
        result.Street.Should().Be(customer.Street);
        result.ExemptRegions.Should().BeEquivalentTo(customer.ExemptRegions);
    }

    [Test]
    public void when_converting_order_to_order_request()
    {
        //arrange
        var order = TaxjarFakes.FakeOrder(generateLineItems: true, generateCustomerId: true).Generate();

        //act
        var result = order.ToRequest();

        //assert
        result.TransactionId.Should().Be(order.TransactionId);
        result.TransactionDate.Should().Be(order.TransactionDate);
        result.Provider.Should().Be(order.Provider);
        result.ExemptionType.Should().Be(order.ExemptionType);
        result.FromCountry.Should().Be(order.FromCountry);
        result.FromState.Should().Be(order.FromState);
        result.FromCity.Should().Be(order.FromCity);
        result.FromZip.Should().Be(order.FromZip);
        result.FromStreet.Should().Be(order.FromStreet);
        result.ToCountry.Should().Be(order.ToCountry);
        result.ToState.Should().Be(order.ToState);
        result.ToCity.Should().Be(order.ToCity);
        result.ToZip.Should().Be(order.ToZip);
        result.Amount.Should().Be(order.Amount);
        result.Shipping.Should().Be(order.Shipping);
        result.SalesTax.Should().Be(order.SalesTax);
        result.CustomerId.Should().Be(order.CustomerId);
        result.LineItems.Should().BeEquivalentTo(order.LineItems);
    }

    [Test]
    public void when_converting_refund_to_refund_request()
    {
        //arrange
        var refund = TaxjarFakes.FakeRefund(generateLineItems: true, generateCustomerId: true).Generate();

        //act
        var result = refund.ToRequest();

        //assert
        result.TransactionId.Should().Be(refund.TransactionId);
        result.TransactionReferenceId.Should().Be(refund.TransactionReferenceId);
        result.TransactionDate.Should().Be(refund.TransactionDate);
        result.Provider.Should().Be(refund.Provider);
        result.ExemptionType.Should().Be(refund.ExemptionType);
        result.FromCountry.Should().Be(refund.FromCountry);
        result.FromState.Should().Be(refund.FromState);
        result.FromCity.Should().Be(refund.FromCity);
        result.FromZip.Should().Be(refund.FromZip);
        result.FromStreet.Should().Be(refund.FromStreet);
        result.ToCountry.Should().Be(refund.ToCountry);
        result.ToState.Should().Be(refund.ToState);
        result.ToCity.Should().Be(refund.ToCity);
        result.ToZip.Should().Be(refund.ToZip);
        result.Amount.Should().Be(refund.Amount);
        result.Shipping.Should().Be(refund.Shipping);
        result.SalesTax.Should().Be(refund.SalesTax);
        result.CustomerId.Should().Be(refund.CustomerId);
        result.LineItems.Should().BeEquivalentTo(refund.LineItems);
    }

}