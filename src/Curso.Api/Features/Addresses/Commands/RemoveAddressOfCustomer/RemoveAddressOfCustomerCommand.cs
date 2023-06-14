using MediatR;

namespace Curso.Api.Features.Addresses.Command.RemoveAddressOfCustomer;

public class RemoveAddressOfCustomerCommand : IRequest<bool>{
    public int Id {get;set;}
    public int CustomerId {get;set;}
    // public string Street {get; set;} = "";
    // public string City {get; set;} = "";
}