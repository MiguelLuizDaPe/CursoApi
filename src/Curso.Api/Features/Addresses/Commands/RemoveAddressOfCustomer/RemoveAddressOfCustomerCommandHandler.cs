using AutoMapper;
using Curso.Api.Entities;
using Curso.Api.Repositories;
using MediatR;

namespace Curso.Api.Features.Addresses.Command.RemoveAddressOfCustomer;


// O primeiro parâmetro é o tipo da mensagem
// O segundo parâmetro é o tipo que se espera receber.
public class RemoveAddressOfCustomerCommandHandler: IRequestHandler<RemoveAddressOfCustomerCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public RemoveAddressOfCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    async Task<bool> IRequestHandler<RemoveAddressOfCustomerCommand, bool>.Handle(RemoveAddressOfCustomerCommand request, CancellationToken cancellationToken)
    {
        var customerEntity = await _customerRepository.GetCustomerByIdAsync(request.CustomerId);
        var addressEntity = customerEntity?.Addresses.FirstOrDefault(a => a.Id == request.Id); 
        _customerRepository.RemoveAddressFromCustomer(addressEntity);
        await _customerRepository.SaveChangesAsync();
        return true;
    }
}