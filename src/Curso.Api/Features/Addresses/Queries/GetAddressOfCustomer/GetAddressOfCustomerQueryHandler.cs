using AutoMapper;
using Curso.Api.Repositories;
using MediatR;

namespace Curso.Api.Features.Addresses.Queries.GetAddressOfCustomer;

public class GetCustomerByCpfQueryHandler : IRequestHandler<GetAddressOfCustomerQuery, GetAddressOfCustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomerByCpfQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<GetAddressOfCustomerDto> Handle(GetAddressOfCustomerQuery request, CancellationToken cancellationToken)
    {
        var addressFromCustomer = await _customerRepository.GetAddressFromCustomerAsync(request.CustomerId, request.Id);
        return _mapper.Map<GetAddressOfCustomerDto>(addressFromCustomer);
    }
}
