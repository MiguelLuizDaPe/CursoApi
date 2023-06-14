using AutoMapper;
using Curso.Api.Repositories;
using MediatR;

namespace Curso.Api.Features.Addresses.Queries.GetAddressesOfCustomer;

public class GetAddressesOfCustomerQueryHandler : IRequestHandler<GetAddressesOfCustomerQuery, IEnumerable<GetAddressesOfCustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetAddressesOfCustomerQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetAddressesOfCustomerDto>> Handle(GetAddressesOfCustomerQuery request, CancellationToken cancellationToken)
    {
        var addressesFromDatabase = await _customerRepository.GetAddressesFromCustomerAsync(request.CustomerId);
        return _mapper.Map<IEnumerable<GetAddressesOfCustomerDto>>(addressesFromDatabase);
    }
}