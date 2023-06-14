using MediatR;

namespace Curso.Api.Features.Addresses.Queries.GetAddressOfCustomer;

public class GetAddressOfCustomerQuery : IRequest<GetAddressOfCustomerDto>{
    public int Id {get; set;}
    public int CustomerId {get; set;}

}