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
            var dateOfBirth = new BirthDate { Day = 23, Month = 8, Year = 84 };

            return new Customer { DateOfBirth = dateOfBirth, Forename = "Onam", Surname = "Chilwan" };
        }
    }
}