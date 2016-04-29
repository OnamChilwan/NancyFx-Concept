namespace Nancy.Api.Tests.Modules
{
    using Moq;

    using Nancy.Api.Objects;
    using Nancy.Api.Queries;
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
            const int CustomerId = 123;
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
            
            this.Given_I_Have_A_Valid_CustomerId(CustomerId, customer)
                .When_Requesting_A_Customer(CustomerId)
                .Then_An_Ok_Response_Is_Returned()
                .Then_The_Response_Is_Correct();
        }

        [Test]
        public void Retrieving_A_Customer_Which_Does_Not_Exist()
        {
            const int CustomerId = 999;

            this.Given_I_Have_An_Invalid_Customer(CustomerId)
                .When_Requesting_A_Customer(CustomerId)
                .Then_A_Not_Found_Response_Is_Returned();
        }

        private CustomerModuleTests Given_I_Have_A_Valid_CustomerId(int customerId, Customer customer)
        {
            this.getCustomerQuery.Setup(x => x.Execute(customerId)).Returns(customer);

            return this;
        }

        private CustomerModuleTests Given_I_Have_An_Invalid_Customer(int customerId)
        {
            this.getCustomerQuery.Setup(x => x.Execute(customerId)).Returns((Customer)null);

            return this;
        }

        private CustomerModuleTests When_Requesting_A_Customer(int customerId)
        {
            this.response = this.browser.Get($"api/customers/{customerId}",
                with =>
                {
                    with.HttpRequest();
                });

            return this;
        }

        private CustomerModuleTests Then_An_Ok_Response_Is_Returned()
        {
            Assert.That(this.response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            return this;
        }

        private CustomerModuleTests Then_A_Not_Found_Response_Is_Returned()
        {
            Assert.That(this.response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

            return this;
        }

        private CustomerModuleTests Then_The_Response_Is_Correct()
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