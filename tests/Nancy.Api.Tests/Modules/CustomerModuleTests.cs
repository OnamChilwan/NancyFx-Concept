namespace Nancy.Api.Tests.Modules
{
    using Moq;

    using Nancy.Api.Modules;
    using Nancy.Api.Objects;
    using Nancy.Responses.Negotiation;
    using Nancy.Testing;

    using NUnit.Framework;

    [TestFixture]
    public class CustomerModuleTests
    {
        private Browser browser;
        private BrowserResponse response;
        private Mock<IGetCustomerQuery> getCustomerQuery;

        [SetUp]
        public void Setup()
        {
            this.getCustomerQuery = new Mock<IGetCustomerQuery>();

            this.browser = new Browser(
                with =>
                {
                    with.AllDiscoveredModules();
                    with.ResponseProcessor<JsonProcessor>();
                    with.Dependencies(this.getCustomerQuery.Object);
                });
        }

        [Test]
        public void Successfully_Retrieving_A_Customer()
        {
            var customerId = 123;
            var customer = new Customer
            {
                Forename = "Onam",
                Surname = "Chilwan",
                DateOfBirth = new BirthDate
                {
                    Day = 23,
                    Month = 8,
                    Year = 1984
                }
            };
            
            this.GivenSuccessfullyRequestingACustomer(customerId, customer)
                .WhenCustomerIdIsSupplied(customerId)
                .ThenAnOkResponseIsReturned()
                .ThenTheResponseIsCorrect();
        }

        [Test]
        public void Retrieving_A_Customer_Which_Does_Not_Exist()
        {
            var customerId = 999;

            this.GivenRequestingACustomerWhichDoesNotExist(customerId)
                .WhenCustomerIdIsSupplied(customerId)
                .ThenANotFoundResponseIsReturned();
        }

        private CustomerModuleTests GivenSuccessfullyRequestingACustomer(int customerId, Customer customer)
        {
            this.getCustomerQuery.Setup(x => x.Execute(customerId)).Returns(customer);

            return this;
        }

        private CustomerModuleTests GivenRequestingACustomerWhichDoesNotExist(int customerId)
        {
            this.getCustomerQuery.Setup(x => x.Execute(customerId)).Returns((Customer)null);

            return this;
        }

        private CustomerModuleTests WhenCustomerIdIsSupplied(int customerId)
        {
            this.response = this.browser.Get($"api/customers/{customerId}",
                with =>
                {
                    with.HttpRequest();
                });

            return this;
        }

        private CustomerModuleTests ThenAnOkResponseIsReturned()
        {
            Assert.That(this.response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            return this;
        }

        private CustomerModuleTests ThenANotFoundResponseIsReturned()
        {
            Assert.That(this.response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

            return this;
        }

        private CustomerModuleTests ThenTheResponseIsCorrect()
        {
            var customer = this.response.Body.DeserializeJson<Customer>();

            Assert.That(customer.Forename, Is.EqualTo("Onam"));
            Assert.That(customer.Surname, Is.EqualTo("Chilwan"));
            Assert.That(customer.DateOfBirth.Day, Is.EqualTo(23));
            Assert.That(customer.DateOfBirth.Month, Is.EqualTo(8));
            Assert.That(customer.DateOfBirth.Year, Is.EqualTo(1984));

            return this;
        }
    }
}