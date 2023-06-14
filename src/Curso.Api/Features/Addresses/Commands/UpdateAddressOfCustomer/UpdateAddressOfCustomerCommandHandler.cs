using AutoMapper;
using Curso.Api.Entities;
using Curso.Api.Models;
using Curso.Api.Repositories;
using MediatR;

namespace Curso.Api.Features.Addresses.Command.UpdateAddressOfCustomer;


// O primeiro parâmetro é o tipo da mensagem
// O segundo parâmetro é o tipo que se espera receber.
public class UpdateAddressOfCustomerCommandHandler: IRequestHandler<UpdateAddressOfCustomerCommand, UpdateAddressOfCustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public UpdateAddressOfCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<UpdateAddressOfCustomerDto> Handle(UpdateAddressOfCustomerCommand request, CancellationToken cancellationToken)
    {
        var rightAddressFromCustomer = await _customerRepository.GetAddressFromCustomerAsync(request.CustomerId, request.Id);
        var addressForUpdateDto = _mapper.Map<AddressForUpdateDto>(request);
        
        _customerRepository.UpdateAddressInCustomer(addressForUpdateDto, rightAddressFromCustomer);
        await _customerRepository.SaveChangesAsync();

        var addressesToReturn = _mapper.Map<UpdateAddressOfCustomerDto>(addressForUpdateDto);
        return addressesToReturn;
    }
}
