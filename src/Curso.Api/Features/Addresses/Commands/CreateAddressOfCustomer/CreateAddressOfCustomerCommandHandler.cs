using AutoMapper;
using Curso.Api.Entities;
using Curso.Api.Repositories;
using MediatR;

namespace Curso.Api.Features.Addresses.Command.CreateAddressOfCustomer;


// O primeiro parâmetro é o tipo da mensagem
// O segundo parâmetro é o tipo que se espera receber.
public class CreateAddressOfCustomerCommandHandler: IRequestHandler<CreateAddressOfCustomerCommand, CreateAddressOfCustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CreateAddressOfCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<CreateAddressOfCustomerDto> Handle(CreateAddressOfCustomerCommand request, CancellationToken cancellationToken)
    {
        var addressEntity = _mapper.Map<Address>(request);

        _customerRepository.AddAddressInCustomer(request.CustomerId, addressEntity);
        await _customerRepository.SaveChangesAsync();

        var addressToReturn = _mapper.Map<CreateAddressOfCustomerDto>(addressEntity);
        return addressToReturn;
    }
}