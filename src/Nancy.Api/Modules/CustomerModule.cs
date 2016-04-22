namespace Nancy.Api.Modules
{
    using Nancy.Api.Objects;
    using Nancy.Responses.Negotiation;

    public class CustomerModule : NancyModule
    {
        private readonly IGetCustomerQuery getCustomerQuery;

        public CustomerModule(IGetCustomerQuery getCustomerQuery)
        {
            this.getCustomerQuery = getCustomerQuery;

            this.Get["api/customers/{id:int}"] = args => this.GetCustomers(args.id);
        }

        private Negotiator GetCustomers(int customerId)
        {
            var customer = this.getCustomerQuery.Execute(customerId);
            var statusCode = HttpStatusCode.OK;

            if (customer == null)
            {
                statusCode = HttpStatusCode.NotFound;
            }

            return this.Negotiate
                .WithStatusCode(statusCode)
                .WithModel(customer);
        }
    }

    public interface IGetCustomerQuery
    {
        Customer Execute(int customerId);
    }
}