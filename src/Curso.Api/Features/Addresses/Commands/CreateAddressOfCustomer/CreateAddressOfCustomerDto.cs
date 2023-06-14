namespace Curso.Api.Features.Addresses.Command.CreateAddressOfCustomer;

public class CreateAddressOfCustomerDto{
    public int Id {get; set;}
    public string Street {get; set;} = "";
    public string City {get; set;} = "";
    public int CustomerId {get; set;}
}