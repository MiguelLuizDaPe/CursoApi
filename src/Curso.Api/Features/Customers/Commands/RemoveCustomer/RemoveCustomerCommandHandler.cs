using AutoMapper;
using Curso.Api.Repositories;
using MediatR;

namespace Curso.Api.Features.Customers.Commands.RemoveCustomer;

public class RemoveCustomerCommandHandler : IRequestHandler<RemoveCustomerCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public RemoveCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<bool> Handle(RemoveCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(request.Id);
        _customerRepository.RemoveCustomer(customer);
        await _customerRepository.SaveChangesAsync();
        return true;
    }
}
