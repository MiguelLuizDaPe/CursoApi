using MediatR;

namespace Curso.Api.Features.Customers.Queries.GetCustomers;

public class GetCustomersQuery : IRequest<IEnumerable<GetCustomersDto>>{

}