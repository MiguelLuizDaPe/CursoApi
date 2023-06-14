using AutoMapper;
using Curso.Api.Entities;
using Curso.Api.Models;
using Curso.Api.Repositories;
using MediatR;

namespace Curso.Api.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, UpdateCustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<UpdateCustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var rightCustomer = await _customerRepository.GetCustomerByIdAsync(request.Id);
        var customerForUpdateDto = _mapper.Map<CustomerForUpdateDto>(request);
        _customerRepository.UpdateCustomer(customerForUpdateDto, rightCustomer);
        await _customerRepository.SaveChangesAsync();
        var customerForReturn = _mapper.Map<UpdateCustomerDto>(customerForUpdateDto);
        return customerForReturn;
    }
}
