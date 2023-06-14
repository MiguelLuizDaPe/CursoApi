namespace Curso.Api.Features.Addresses.Queries.GetAddressesOfCustomer;

public class GetAddressesOfCustomerDto{
    public int Id {get; set;}
    public string Street {get; set;} = "";
    public string City {get; set;} = "";
    public int CustomerId {get; set;}
}