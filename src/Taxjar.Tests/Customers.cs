using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Taxjar.Tests
{
    [TestFixture]
    public class CustomersTests
    {
        [SetUp]
        public void Init()
        {
            Bootstrap.client = new TaxjarApi(Bootstrap.apiKey, new { apiUrl = "http://localhost:9191" });
            Bootstrap.server.ResetMappings();
        }

        public void AssertCustomer(CustomerResponseAttributes customer)
        {
            Assert.AreEqual("123", customer.CustomerId);
            Assert.AreEqual("wholesale", customer.ExemptionType);
            Assert.AreEqual("US", customer.ExemptRegions[0].Country);
            Assert.AreEqual("FL", customer.ExemptRegions[0].State);
            Assert.AreEqual("US", customer.ExemptRegions[1].Country);
            Assert.AreEqual("PA", customer.ExemptRegions[1].State);
            Assert.AreEqual("Dunder Mifflin Paper Company", customer.Name);
            Assert.AreEqual("US", customer.Country);
            Assert.AreEqual("PA", customer.State);
            Assert.AreEqual("18504", customer.Zip);
            Assert.AreEqual("Scranton", customer.City);
            Assert.AreEqual("1725 Slough Avenue", customer.Street);
        }

        [Test]
        public void when_listing_customers()
        {
            var body = JsonConvert.DeserializeObject<CustomersResponse>(TaxjarFixture.GetJSON("customers/list.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/customers")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(body)
            );

            var customers = Bootstrap.client.ListCustomers();

            Assert.AreEqual("123", customers[0]);
            Assert.AreEqual("456", customers[1]);
        }

        [Test]
        public void when_showing_an_order_transaction()
        {
            var body = JsonConvert.DeserializeObject<CustomerResponse>(TaxjarFixture.GetJSON("customers/show.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/customers/123")
                    .UsingGet()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(body)
            );

            var customer = Bootstrap.client.ShowCustomer("123");
            AssertCustomer(customer);
        }

        [Test]
        public void when_creating_a_customer()
        {
            var body = JsonConvert.DeserializeObject<CustomerResponse>(TaxjarFixture.GetJSON("customers/show.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/customers")
                    .UsingPost()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.Created)
                    .WithBodyAsJson(body)
            );

            var customer = Bootstrap.client.CreateCustomer(new
            {
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

            AssertCustomer(customer);
        }

        [Test]
        public void when_updating_a_customer()
        {
            var body = JsonConvert.DeserializeObject<CustomerResponse>(TaxjarFixture.GetJSON("customers/show.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/customers/123")
                    .UsingPut()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(body)
            );

            var customer = Bootstrap.client.UpdateCustomer(new
            {
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

            AssertCustomer(customer);
        }

        [Test]
        public void when_updating_a_customer_with_missing_customer_id()
        {
            var body = JsonConvert.DeserializeObject<CustomerResponse>(TaxjarFixture.GetJSON("customers/show.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/customers/123")
                    .UsingPut()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(body)
            );

            var systemException = Assert.Throws<Exception>(() => Bootstrap.client.UpdateCustomer(new
            {
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
            }));

            Assert.AreEqual("Missing customer ID for `UpdateCustomer`", systemException.Message);
        }

        [Test]
        public void when_deleting_a_customer()
        {
            var body = JsonConvert.DeserializeObject<CustomerResponse>(TaxjarFixture.GetJSON("customers/show.json"));

            Bootstrap.server.Given(
                Request.Create()
                    .WithPath("/v2/customers/123")
                    .UsingDelete()
            ).RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithBodyAsJson(body)
            );

            var customer = Bootstrap.client.DeleteCustomer("123");
            AssertCustomer(customer);
        }
    }
}
