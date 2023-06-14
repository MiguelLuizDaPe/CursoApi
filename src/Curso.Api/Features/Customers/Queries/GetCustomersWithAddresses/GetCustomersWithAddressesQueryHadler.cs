using AutoMapper;
using Curso.Api.Repositories;
using MediatR;

namespace Curso.Api.Features.Customers.Queries.GetCustomersWithAddresses;

public class GetCustomersWithAddressesQueryHandler : IRequestHandler<GetCustomersWithAddressesQuery, IEnumerable<GetCustomersWithAddressesDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomersWithAddressesQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetCustomersWithAddressesDto>> Handle(GetCustomersWithAddressesQuery request, CancellationToken cancellationToken)
    {
        var customersFromDatabase = await _customerRepository.GetCustomersWithAddressesAsync();
        return _mapper.Map<IEnumerable<GetCustomersWithAddressesDto>>(customersFromDatabase);
    }
}