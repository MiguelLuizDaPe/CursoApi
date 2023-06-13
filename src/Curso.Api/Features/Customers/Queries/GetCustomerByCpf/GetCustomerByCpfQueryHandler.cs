using AutoMapper;
using Curso.Api.Repositories;
using MediatR;

namespace Curso.Api.Features.Customers.Queries.GetCustomerByCpf;

public class GetCustomerByCpfQueryHandler : IRequestHandler<GetCustomerByCpfQuery, GetCustomerByCpfDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public GetCustomerByCpfQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<GetCustomerByCpfDto> Handle(GetCustomerByCpfQuery request, CancellationToken cancellationToken)
    {
        var customerFromDatabase = await _customerRepository.GetCustomerByCpfAsync(request.Cpf);
        return _mapper.Map<GetCustomerByCpfDto>(customerFromDatabase);
    }
}