using AutoMapper;
using Curso.Api.Repositories;
using MediatR;

namespace Curso.Api.Features.Customers.Queries.GetCustomerWithAddress;

public class GetCustomerWithAddressQueryHandler : IRequestHandler<GetCustomerWithAddressQuery, GetCustomerWithAddressDto>{
    private readonly IMapper _mapper;
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerWithAddressQueryHandler(IMapper mapper, ICustomerRepository customerRepository){
        _mapper = mapper;
        _customerRepository = customerRepository;
    }

    public async Task<GetCustomerWithAddressDto> Handle(GetCustomerWithAddressQuery request, CancellationToken cancellationToken)
    {
        var customerFromDatabase = await _customerRepository.GetCustomerWithAddressesAsync(request.Id);
        return _mapper.Map<GetCustomerWithAddressDto>(customerFromDatabase);
    }
}