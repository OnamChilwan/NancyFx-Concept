namespace Nancy.Api.Queries
{
    using Nancy.Api.Objects;

    public interface IGetCustomerQuery
    {
        Customer Execute(int customerId);
    }

    public class GetCustomerQuery : IGetCustomerQuery
    {
        public Customer Execute(int customerId)
        {
            return new Customer();
        }
    }
}