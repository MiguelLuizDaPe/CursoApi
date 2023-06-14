namespace Curso.Api.Features.Addresses.Command.RemoveAddressOfCustomer;

public class RemoveAddressOfCustomerDto{
    public int Id {get;set;}
    public string Street {get; set;} = "";
    public string City {get; set;} = "";
}
