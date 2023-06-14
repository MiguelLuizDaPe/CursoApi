using AutoMapper;
using Curso.Api.Entities;
using Curso.Api.Repositories;
using MediatR;

namespace Curso.Api.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CreateCustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customerEntity = _mapper.Map<Customer>(request);
        _customerRepository.AddCustomer(customerEntity);
        await _customerRepository.SaveChangesAsync();
        var customerForReturn = _mapper.Map<CreateCustomerDto>(customerEntity);
        return customerForReturn;
    }
}