using AutoMapper;
using Curso.Api.Repositories;
using MediatR;

namespace Curso.Api.Features.Customers.Queries.GetCustomers;

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, IEnumerable<GetCustomersDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomersQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetCustomersDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customersFromDatabase = await _customerRepository.GetCustomersAsync();
        return _mapper.Map<IEnumerable<GetCustomersDto>>(customersFromDatabase);
    }
}
