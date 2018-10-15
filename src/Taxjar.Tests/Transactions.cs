using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Taxjar.Tests
{
	[TestFixture]
	public class TransactionsTests
	{
        [SetUp]
        public void Init()
        {
            Bootstrap.client = new TaxjarApi(Bootstrap.apiKey, new { apiUrl = "http://localhost:9191" });
            Bootstrap.server.ResetMappings();
        }

		public void AssertOrder(OrderResponseAttributes order)
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
			Assert.AreEqual("1", order.LineItems[0].Id);
			Assert.AreEqual(1, order.LineItems[0].Quantity);
			Assert.AreEqual("12-34243-0", order.LineItems[0].ProductIdentifier);
			Assert.AreEqual("Heavy Widget", order.LineItems[0].Description);
			Assert.AreEqual("20010", order.LineItems[0].ProductTaxCode);
			Assert.AreEqual(15, order.LineItems[0].UnitPrice);
			Assert.AreEqual(0, order.LineItems[0].Discount);
			Assert.AreEqual(0.95, order.LineItems[0].SalesTax);
		}

        public void AssertDeletedOrder(OrderResponseAttributes order)
		{
			Assert.AreEqual("123", order.TransactionId);
			Assert.AreEqual(null, order.TransactionDate);
			Assert.AreEqual(0, order.Amount);
			Assert.AreEqual(0, order.Shipping);
			Assert.AreEqual(0, order.SalesTax);
		}

		public void AssertRefund(RefundResponseAttributes refund)
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
			Assert.AreEqual("1", refund.LineItems[0].Id);
			Assert.AreEqual(1, refund.LineItems[0].Quantity);
			Assert.AreEqual("12-34243-0", refund.LineItems[0].ProductIdentifier);
			Assert.AreEqual("Heavy Widget", refund.LineItems[0].Description);
			Assert.AreEqual("20010", refund.LineItems[0].ProductTaxCode);
			Assert.AreEqual(15, refund.LineItems[0].UnitPrice);
			Assert.AreEqual(0, refund.LineItems[0].Discount);
			Assert.AreEqual(0.95, refund.LineItems[0].SalesTax);
		}

        public void AssertDeletedRefund(RefundResponseAttributes refund)
		{
			Assert.AreEqual("321", refund.TransactionId);
			Assert.AreEqual(null, refund.TransactionDate);
			Assert.AreEqual(0, refund.Amount);
			Assert.AreEqual(0, refund.Shipping);
			Assert.AreEqual(0, refund.SalesTax);
		}

		[Test]
		public void when_listing_order_transactions()
		{
            var body = JsonConvert.DeserializeObject<OrdersResponse>(TaxjarFixture.GetJSON("orders/list.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/transactions/orders")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

			var orders = Bootstrap.client.ListOrders(new {
				from_transaction_date = "2015/05/01",
				to_transaction_date = "2015/05/31"
			});

			Assert.AreEqual("123", orders[0]);
			Assert.AreEqual("456", orders[1]);
		}

        [Test]
        public async Task when_listing_order_transactions_async()
        {
            var body = JsonConvert.DeserializeObject<OrdersResponse>(TaxjarFixture.GetJSON("orders/list.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/transactions/orders")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

            var ordersRequest = await Bootstrap.client.ListOrdersAsync(new
            {
                from_transaction_date = "2015/05/01",
                to_transaction_date = "2015/05/31"
            });

            var orders = ordersRequest.Orders;

            Assert.AreEqual("123", orders[0]);
            Assert.AreEqual("456", orders[1]);
        }

        [Test]
		public void when_showing_an_order_transaction()
		{
            var body = JsonConvert.DeserializeObject<OrderResponse>(TaxjarFixture.GetJSON("orders/show.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/transactions/orders/123")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

			var order = Bootstrap.client.ShowOrder("123");
			AssertOrder(order);
		}

        [Test]
        public async Task when_showing_an_order_transaction_async()
        {
            var body = JsonConvert.DeserializeObject<OrderResponse>(TaxjarFixture.GetJSON("orders/show.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/transactions/orders/123")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

            var orderRequest = await Bootstrap.client.ShowOrderAsync("123");
            var order = orderRequest.Order;

            AssertOrder(order);
        }

        [Test]
		public void when_creating_an_order_transaction()
		{
            var body = JsonConvert.DeserializeObject<OrderResponse>(TaxjarFixture.GetJSON("orders/show.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/transactions/orders")
                    .UsingPost()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.Created)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

			var order = Bootstrap.client.CreateOrder(new {
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
						product_tax_code = "20010",
						unit_price = 15,
						sales_tax = 0.95
					}
				}
			});

			AssertOrder(order);
		}

        [Test]
        public async Task when_creating_an_order_transaction_async()
        {
            var body = JsonConvert.DeserializeObject<OrderResponse>(TaxjarFixture.GetJSON("orders/show.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/transactions/orders")
                    .UsingPost()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.Created)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

            var orderRequest = await Bootstrap.client.CreateOrderAsync(new
            {
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
                        product_tax_code = "20010",
                        unit_price = 15,
                        sales_tax = 0.95
                    }
                }
            });

            var order = orderRequest.Order;

            AssertOrder(order);
        }

        [Test]
		public void when_updating_an_order_transaction()
		{
            var body = JsonConvert.DeserializeObject<OrderResponse>(TaxjarFixture.GetJSON("orders/show.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/transactions/orders/123")
                    .UsingPut()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

			var order = Bootstrap.client.UpdateOrder(new
			{
				transaction_id = "123",
				amount = 17.95,
				shipping = 2,
				line_items = new[] {
					new {
						quantity = 1,
						product_identifier = "12-34243-0",
						description = "Heavy Widget",
						product_tax_code = "20010",
						unit_price = 15,
						discount = 0,
						sales_tax = 0.95
					}
				}
			});

			AssertOrder(order);
		}

        [Test]
        public async Task when_updating_an_order_transaction_async()
        {
            var body = JsonConvert.DeserializeObject<OrderResponse>(TaxjarFixture.GetJSON("orders/show.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/transactions/orders/123")
                    .UsingPut()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

            var orderRequest = await Bootstrap.client.UpdateOrderAsync(new
            {
                transaction_id = "123",
                amount = 17.95,
                shipping = 2,
                line_items = new[] {
                    new {
                        quantity = 1,
                        product_identifier = "12-34243-0",
                        description = "Heavy Widget",
                        product_tax_code = "20010",
                        unit_price = 15,
                        discount = 0,
                        sales_tax = 0.95
                    }
                }
            });

            var order = orderRequest.Order;

            AssertOrder(order);
        }

        [Test]
		public void when_deleting_an_order_transaction()
		{
            Bootstrap.client = new TaxjarApi(Bootstrap.apiKey, new { apiUrl = "https://api.sandbox.taxjar.com" });

			var order = Bootstrap.client.DeleteOrder("123");
			AssertDeletedOrder(order);
		}

        [Test]
        public async Task when_deleting_an_order_transaction_async()
        {
            Bootstrap.client = new TaxjarApi(Bootstrap.apiKey, new { apiUrl = "https://api.sandbox.taxjar.com" });

            var orderRequest = await Bootstrap.client.DeleteOrderAsync("123");
            var order = orderRequest.Order;
            AssertDeletedOrder(order);
        }

        [Test]
		public void when_listing_refund_transactions()
		{
            var body = JsonConvert.DeserializeObject<RefundsResponse>(TaxjarFixture.GetJSON("refunds/list.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/transactions/refunds")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

			var refunds = Bootstrap.client.ListRefunds(new
			{
				from_transaction_date = "2015/05/01",
				to_transaction_date = "2015/05/31"
			});

			Assert.AreEqual("321", refunds[0]);
			Assert.AreEqual("654", refunds[1]);
		}

        [Test]
        public async Task when_listing_refund_transactions_async()
        {
            var body = JsonConvert.DeserializeObject<RefundsResponse>(TaxjarFixture.GetJSON("refunds/list.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/transactions/refunds")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

            var refundsRequest = await Bootstrap.client.ListRefundsAsync(new
            {
                from_transaction_date = "2015/05/01",
                to_transaction_date = "2015/05/31"
            });

            var refunds = refundsRequest.Refunds;

            Assert.AreEqual("321", refunds[0]);
            Assert.AreEqual("654", refunds[1]);
        }

        [Test]
		public void when_showing_a_refund_transaction()
		{
            var body = JsonConvert.DeserializeObject<RefundResponse>(TaxjarFixture.GetJSON("refunds/show.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/transactions/refunds/321")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

			var refund = Bootstrap.client.ShowRefund("321");
			AssertRefund(refund);
		}

        [Test]
        public async Task when_showing_a_refund_transaction_async()
        {
            var body = JsonConvert.DeserializeObject<RefundResponse>(TaxjarFixture.GetJSON("refunds/show.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/transactions/refunds/321")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

            var refundRequest = await Bootstrap.client.ShowRefundAsync("321");
            var refund = refundRequest.Refund;
            AssertRefund(refund);
        }

        [Test]
		public void when_creating_a_refund_transaction()
		{
            var body = JsonConvert.DeserializeObject<RefundResponse>(TaxjarFixture.GetJSON("refunds/show.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/transactions/refunds")
                    .UsingPost()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.Created)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

			var refund = Bootstrap.client.CreateRefund(new
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
						product_tax_code = "20010",
						unit_price = 15,
						sales_tax = 0.95
					}
				}
			});

			AssertRefund(refund);
		}

        [Test]
        public async Task when_creating_a_refund_transaction_async()
        {
            var body = JsonConvert.DeserializeObject<RefundResponse>(TaxjarFixture.GetJSON("refunds/show.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/transactions/refunds")
                    .UsingPost()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.Created)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

            var refundRequest = await Bootstrap.client.CreateRefundAsync(new
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
                        product_tax_code = "20010",
                        unit_price = 15,
                        sales_tax = 0.95
                    }
                }
            });

            var refund = refundRequest.Refund;

            AssertRefund(refund);
        }

        [Test]
		public void when_updating_a_refund_transaction()
		{
            var body = JsonConvert.DeserializeObject<RefundResponse>(TaxjarFixture.GetJSON("refunds/show.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/transactions/refunds/321")
                    .UsingPut()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.Created)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

			var refund = Bootstrap.client.UpdateRefund(new
			{
				transaction_id = "321",
				amount = 17.95,
				shipping = 2,
				line_items = new[] {
					new {
						quantity = 1,
						product_identifier = "12-34243-0",
						description = "Heavy Widget",
						product_tax_code = "20010",
						unit_price = 15,
						discount = 0,
						sales_tax = 0.95
					}
				}
			});

			AssertRefund(refund);
		}

        [Test]
        public async Task when_updating_a_refund_transaction_async()
        {
            var body = JsonConvert.DeserializeObject<RefundResponse>(TaxjarFixture.GetJSON("refunds/show.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/transactions/refunds/321")
                    .UsingPut()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.Created)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(body)
            );

            var refundRequest = await Bootstrap.client.UpdateRefundAsync(new
            {
                transaction_id = "321",
                amount = 17.95,
                shipping = 2,
                line_items = new[] {
                    new {
                        quantity = 1,
                        product_identifier = "12-34243-0",
                        description = "Heavy Widget",
                        product_tax_code = "20010",
                        unit_price = 15,
                        discount = 0,
                        sales_tax = 0.95
                    }
                }
            });

            var refund = refundRequest.Refund;

            AssertRefund(refund);
        }

        [Test]
		public void when_deleting_a_refund_transaction()
		{
            Bootstrap.client = new TaxjarApi(Bootstrap.apiKey, new { apiUrl = "https://api.sandbox.taxjar.com" });

			var refund = Bootstrap.client.DeleteRefund("321");
			AssertDeletedRefund(refund);
		}

        [Test]
        public async Task when_deleting_a_refund_transaction_async()
        {
            Bootstrap.client = new TaxjarApi(Bootstrap.apiKey, new { apiUrl = "https://api.sandbox.taxjar.com" });

            var refundRequest = await Bootstrap.client.DeleteRefundAsync("321");
            var refund = refundRequest.Refund;
            AssertDeletedRefund(refund);
        }
    }
}