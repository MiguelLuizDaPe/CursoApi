using MediatR;

namespace Curso.Api.Features.Customers.Queries.GetCustomerByCpf;


public class GetCustomerByCpfQuery : IRequest<GetCustomerByCpfDto>
{
    public string Cpf { get; set; } = string.Empty;
}