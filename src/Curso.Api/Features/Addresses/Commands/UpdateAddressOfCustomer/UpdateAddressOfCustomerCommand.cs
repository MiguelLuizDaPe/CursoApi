using MediatR;

namespace Curso.Api.Features.Addresses.Command.UpdateAddressOfCustomer;

public class UpdateAddressOfCustomerCommand : IRequest<UpdateAddressOfCustomerDto>{
    public int Id {get; set;}
    public string Street {get; set;} = "";
    public string City {get; set;} = "";
    public int CustomerId {get; set;}
    
}
