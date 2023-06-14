using MediatR;

namespace Curso.Api.Features.Customers.Queries.GetCustomersWithAddresses;

public class GetCustomersWithAddressesQuery : IRequest<IEnumerable<GetCustomersWithAddressesDto>>{

}