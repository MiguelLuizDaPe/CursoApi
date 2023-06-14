using MediatR;

namespace Curso.Api.Features.Customers.Commands.RemoveCustomer;

public class RemoveCustomerCommand : IRequest<bool>{
    public int Id {get;set;}
}