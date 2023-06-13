using MediatR;

namespace Curso.Api.Features.Customers.Queries.GetCustomerDetail;

public class GetCustomerDetailQuery : IRequest<GetCustomerDetailDto>
{
    public int Id {get; set;}
}