using MediatR;

namespace Curso.Api.Features.Addresses.Queries.GetAddressesOfCustomer;

public class GetAddressesOfCustomerQuery : IRequest<IEnumerable<GetAddressesOfCustomerDto>>{
    public int CustomerId {get; set;}
    
}