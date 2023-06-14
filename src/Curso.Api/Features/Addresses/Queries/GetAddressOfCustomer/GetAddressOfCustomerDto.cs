namespace Curso.Api.Features.Addresses.Queries.GetAddressOfCustomer;

public class GetAddressOfCustomerDto{
    public int Id {get; set;}
    public string Street {get; set;} = "";
    public string City {get; set;} = "";
    public int CustomerId {get; set;}
}