using AutoMapper;
using Curso.Api.Entities;
using Curso.Api.Repositories;
using MediatR;

namespace Curso.Api.Features.Customers.Commands.CreateCustomerWithAddress;

public class CreateCustomerWithAddressCommandHandler : IRequestHandler<CreateCustomerWithAddressCommand, CreateCustomerWithAddressDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CreateCustomerWithAddressCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CreateCustomerWithAddressDto> Handle(CreateCustomerWithAddressCommand request, CancellationToken cancellationToken)
    {
        var customerEntity = _mapper.Map<Customer>(request);

        _customerRepository.AddCustomerWithAddresses(customerEntity);
        await _customerRepository.SaveChangesAsync();

        return _mapper.Map<CreateCustomerWithAddressDto>(customerEntity);


    }
}
