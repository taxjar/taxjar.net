using HttpMock;
using NUnit.Framework;

namespace Taxjar.Tests
{
	[TestFixture]
	public class TransactionsTests
	{
		internal TaxjarApi client;

		[SetUp]
		public void Init()
		{
			this.client = new TaxjarApi("foo123", new { apiUrl = "http://localhost:9191/v2/" });
		}

		public void AssertOrder(Order order)
		{
			Assert.AreEqual("123", order.TransactionId);
			Assert.AreEqual(10649, order.UserId);
			Assert.AreEqual("2015-05-14T00:00:00Z", order.TransactionDate);
			Assert.AreEqual("US", order.ToCountry);
			Assert.AreEqual("90002", order.ToZip);
			Assert.AreEqual("CA", order.ToState);
			Assert.AreEqual("LOS ANGELES", order.ToCity);
			Assert.AreEqual("123 Palm Grove Ln", order.ToStreet);
			Assert.AreEqual(17.95, order.Amount);
			Assert.AreEqual(2, order.Shipping);
			Assert.AreEqual(0.95, order.SalesTax);
			Assert.AreEqual(1, order.LineItems[0].Id);
			Assert.AreEqual(1, order.LineItems[0].Quantity);
			Assert.AreEqual("12-34243-0", order.LineItems[0].ProductIdentifier);
			Assert.AreEqual("Heavy Widget", order.LineItems[0].Description);
			Assert.AreEqual(15, order.LineItems[0].UnitPrice);
			Assert.AreEqual(0, order.LineItems[0].Discount);
			Assert.AreEqual(0.95, order.LineItems[0].SalesTax);
		}

		public void AssertRefund(Refund refund)
		{
			Assert.AreEqual("321", refund.TransactionId);
			Assert.AreEqual("123", refund.TransactionReferenceId);
			Assert.AreEqual(10649, refund.UserId);
			Assert.AreEqual("2015-05-14T00:00:00Z", refund.TransactionDate);
			Assert.AreEqual("US", refund.ToCountry);
			Assert.AreEqual("90002", refund.ToZip);
			Assert.AreEqual("CA", refund.ToState);
			Assert.AreEqual("LOS ANGELES", refund.ToCity);
			Assert.AreEqual("123 Palm Grove Ln", refund.ToStreet);
			Assert.AreEqual(17.95, refund.Amount);
			Assert.AreEqual(2, refund.Shipping);
			Assert.AreEqual(0.95, refund.SalesTax);
			Assert.AreEqual(1, refund.LineItems[0].Id);
			Assert.AreEqual(1, refund.LineItems[0].Quantity);
			Assert.AreEqual("12-34243-0", refund.LineItems[0].ProductIdentifier);
			Assert.AreEqual("Heavy Widget", refund.LineItems[0].Description);
			Assert.AreEqual(15, refund.LineItems[0].UnitPrice);
			Assert.AreEqual(0, refund.LineItems[0].Discount);
			Assert.AreEqual(0.95, refund.LineItems[0].SalesTax);
		}

		[Test]
		public void when_listing_order_transactions()
		{
			var stubHttp = HttpMockRepository.At("http://localhost:9191");

			stubHttp.Stub(x => x.Get("/v2/transactions/orders"))
					.Return(TaxjarFixture.GetJSON("orders/list.json"))
					.OK();

			var orders = client.ListOrders(new {
				from_transaction_date = "2015/05/01",
				to_transaction_date = "2015/05/31"
			});

			Assert.AreEqual("123", orders[0]);
			Assert.AreEqual("456", orders[1]);
		}

		[Test]
		public void when_showing_an_order_transaction()
		{
			var stubHttp = HttpMockRepository.At("http://localhost:9191");

			stubHttp.Stub(x => x.Get("/v2/transactions/orders/123"))
					.Return(TaxjarFixture.GetJSON("orders/show.json"))
					.OK();

			var order = client.ShowOrder(123);
			this.AssertOrder(order);
		}

		[Test]
		public void when_creating_an_order_transaction()
		{
			var stubHttp = HttpMockRepository.At("http://localhost:9191");

			stubHttp.Stub(x => x.Post("/v2/transactions/orders"))
					.Return(TaxjarFixture.GetJSON("orders/show.json"))
					.OK();

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

			this.AssertOrder(order);
		}

		[Test]
		public void when_updating_an_order_transaction()
		{
			var stubHttp = HttpMockRepository.At("http://localhost:9191");

			stubHttp.Stub(x => x.Put("/v2/transactions/orders/123"))
					.Return(TaxjarFixture.GetJSON("orders/show.json"))
					.OK();

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

			this.AssertOrder(order);
		}

		[Test]
		public void when_deleting_an_order_transaction()
		{
			var stubHttp = HttpMockRepository.At("http://localhost:9191");

			stubHttp.Stub(x => x.Delete("/v2/transactions/orders/123"))
					.Return(TaxjarFixture.GetJSON("orders/show.json"))
					.OK();

			var order = client.DeleteOrder(123);
			this.AssertOrder(order);
		}

		[Test]
		public void when_listing_refund_transactions()
		{
			var stubHttp = HttpMockRepository.At("http://localhost:9191");

			stubHttp.Stub(x => x.Get("/v2/transactions/refunds"))
					.Return(TaxjarFixture.GetJSON("refunds/list.json"))
					.OK();

			var refunds = client.ListRefunds(new
			{
				from_transaction_date = "2015/05/01",
				to_transaction_date = "2015/05/31"
			});

			Assert.AreEqual("321", refunds[0]);
			Assert.AreEqual("654", refunds[1]);
		}

		[Test]
		public void when_showing_a_refund_transaction()
		{
			var stubHttp = HttpMockRepository.At("http://localhost:9191");

			stubHttp.Stub(x => x.Get("/v2/transactions/refunds/321"))
					.Return(TaxjarFixture.GetJSON("refunds/show.json"))
					.OK();

			var refund = client.ShowRefund(321);
			this.AssertRefund(refund);
		}

		[Test]
		public void when_creating_a_refund_transaction()
		{
			var stubHttp = HttpMockRepository.At("http://localhost:9191");

			stubHttp.Stub(x => x.Post("/v2/transactions/refunds"))
					.Return(TaxjarFixture.GetJSON("refunds/show.json"))
					.OK();

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

			this.AssertRefund(refund);
		}

		[Test]
		public void when_updating_a_refund_transaction()
		{
			var stubHttp = HttpMockRepository.At("http://localhost:9191");

			stubHttp.Stub(x => x.Put("/v2/transactions/refunds/321"))
					.Return(TaxjarFixture.GetJSON("refunds/show.json"))
					.OK();

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

			this.AssertRefund(refund);
		}

		[Test]
		public void when_deleting_a_refund_transaction()
		{
			var stubHttp = HttpMockRepository.At("http://localhost:9191");

			stubHttp.Stub(x => x.Delete("/v2/transactions/refunds/321"))
					.Return(TaxjarFixture.GetJSON("refunds/show.json"))
					.OK();

			var refund = client.DeleteRefund(321);
			this.AssertRefund(refund);
		}
	}
}