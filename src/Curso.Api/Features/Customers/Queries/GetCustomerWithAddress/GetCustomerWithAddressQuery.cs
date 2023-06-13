using MediatR;

namespace Curso.Api.Features.Customers.Queries.GetCustomerWithAddress;


public class GetCustomerWithAddressQuery : IRequest<GetCustomerWithAddressDto>
{
    public int Id { get; set; }    
}