using Curso.Api;
using Microsoft.AspNetCore.Mvc;
using Curso.Api.Entities;
using Curso.Api.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Curso.Api.DbContexts;
using Microsoft.EntityFrameworkCore;
using Curso.Api.Repositories;
using MediatR;
using Curso.Api.Features.Customers.Queries.GetCustomerDetail;
using Curso.Api.Features.Customers.Queries.GetCustomerByCpf;
using Curso.Api.Features.Customers.Queries.GetCustomerWithAddress;
using Curso.Api.Features.Customers.Queries.GetCustomers;
using Curso.Api.Features.Customers.Queries.GetCustomersWithAddresses;

namespace Curso.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : MainController{

    private readonly Data _data;
    private readonly IMapper _mapper;
    private readonly CustomerContext _context;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMediator _mediator; 

    public CustomersController(Data data, IMapper mapper, CustomerContext context, ICustomerRepository customerRepository, IMediator mediator){//essa porra é uma injeção de dependência
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    // private int addrIdMax = 0;
    // private int genUniqueAddrId(){
    //     if(addrIdMax == 0){
    //         foreach(Customer c in _data.Customers){
    //             if(!c.Addresses.Any()){continue;}
    //             int idMax = c.Addresses.Max(n => n.Id);
    //             if(idMax > addrIdMax){addrIdMax = idMax;}
    //         }
    //     }
    //     addrIdMax += 1;
    //     return addrIdMax;
    // }

    [HttpGet]//não sei como fazer esse
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers(){//callback hell da uma pesquisadinha depois
        var getCustomers = new GetCustomersQuery();
        var customersForReturn = await _mediator.Send(getCustomers);

       return Ok(customersForReturn);
    }

    [HttpGet("{customerId}", Name = "GetCustomerById")]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(int customerId){
        var getCustomerDetailQuery = new GetCustomerDetailQuery{Id = customerId};
        var customerForReturn = await _mediator.Send(getCustomerDetailQuery);

        if(customerForReturn == null) return NotFound();

        return Ok(customerForReturn);

        //testa customer diferente de null, se for efetua a opção 1 se não efetua a segunda opção
        // return customer != null ? Ok(customer) : NotFound();

    }

    [HttpGet("cpf/{customerCpf}")]
    public async Task<ActionResult<CustomerDto>> GetCustomerCpf(string customerCpf){
        //O n dentro da lambda é o objeto customer pq o FirstOrDefault() retorna elemento de mesma tipagem, por isso é possível interagi com o Id e compara-lo
        var getCustomerByCpfQuery = new GetCustomerByCpfQuery{Cpf = customerCpf};
        var customerForReturn = await _mediator.Send(getCustomerByCpfQuery);

        if(customerForReturn == null) return NotFound();

        return Ok(customerForReturn);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer(CustomerForCreationDto customerForCreationDto){

        var customerEntity = _mapper.Map<Customer>(customerForCreationDto);
        _customerRepository.AddCustomer(customerEntity);
        await _customerRepository.SaveChangesAsync();
        var customerForReturn = _mapper.Map<CustomerDto>(customerEntity);


        return CreatedAtRoute(
            "GetCustomerById",
            new {customerId = customerForReturn.Id},
            customerForReturn
        );
    }

    [HttpPut("{customerId}")]
    public async Task<ActionResult> UpdateCustomer(int customerId, CustomerForUpdateDto customerForUpdateDto){
        //verificação de Id pq pode acontecer um ataque malicioso com intenções maléficas de pura maldade
        if(customerId != customerForUpdateDto.Id){return BadRequest();}

        var rightCustomer = await _customerRepository.GetCustomerByIdAsync(customerId);
        if(rightCustomer == null){return NotFound();};

        _customerRepository.UpdateCustomer(customerForUpdateDto, rightCustomer);
        await _customerRepository.SaveChangesAsync();
    
        return NoContent();
    }

    [HttpDelete("{customerId}")]
    public async Task<ActionResult<CustomerDto>> DeleteCustomer(int customerId){
        var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
        if(customer == null){return NotFound();}
        _customerRepository.RemoveCustomer(customer);
        await _customerRepository.SaveChangesAsync();
        return NoContent();
    }

    //quando possível pesquisar sobre isso (talvez)
    [HttpPatch("{customerId}")]
    public async Task<ActionResult> PartiallyUpdateCustomer(JsonPatchDocument<CustomerForPatchDto> patchDocument, int customerId){
        var customerFromDatabase = await _customerRepository.GetCustomerByIdAsync(customerId);
        if(customerFromDatabase == null){return NotFound();}

        var customerToPatchDto = _mapper.Map<CustomerForPatchDto>(customerFromDatabase);

        patchDocument.ApplyTo(customerToPatchDto, ModelState);

        if(!TryValidateModel(customerToPatchDto)){
            return ValidationProblem(ModelState);
        }

        _customerRepository.PatchCustomer(customerToPatchDto, customerFromDatabase);
        await _customerRepository.SaveChangesAsync();

        return NoContent();

    }

    [HttpGet("with-address")]//não sei como fazer esse
    public async Task<ActionResult<IEnumerable<CustomerWithAddressesDto>>> GetCustomersWithAddresses(){

        var getCustomersWithAddresses = new GetCustomersWithAddressesQuery();
        var customersForReturn = await _mediator.Send(getCustomersWithAddresses);

        return Ok(customersForReturn); // O customerToReturn não precisa de .ToList() pq o Ok() serializa
    }

    [HttpGet("{customerId}/with-address", Name = "GetCustomerByIdWithAddresses")]
    public async Task<ActionResult<CustomerWithAddressesDto>> GetCustomerWithAddresses(int customerId){

        var getCustomerWithAddressQuery = new GetCustomerWithAddressQuery{Id = customerId};
        var customerForReturn = await _mediator.Send(getCustomerWithAddressQuery);

        if(customerForReturn == null){return NotFound();}
        return Ok(customerForReturn);
    }

    [HttpPost("with-address")]
    public async Task<ActionResult<CustomerWithAddressesDto>> CreatCustomerWithAddresses(CustomerWithAddressesForCreationDto customerWithAddressesForCreationDto){
            // int n = 0;
            // int x = n++ + ++n;

        var customerEntity = _mapper.Map<Customer>(customerWithAddressesForCreationDto);

        _customerRepository.AddCustomerWithAddresses(customerEntity);
        await _customerRepository.SaveChangesAsync();

        var customerForReturn = _mapper.Map<CustomerWithAddressesDto>(customerEntity);

        return CreatedAtRoute(
            "GetCustomerByIdWithAddresses",
            new {customerId = customerForReturn.Id},
            customerForReturn
        );
    }

    [HttpPut("{customerId}/with-address")]
    public async Task<ActionResult> UpdateCustomerWithAddresses(int customerId, CustomerWithAddressesForUpdateDto customerWithAddressesForUpdateDto){
        if(customerId != customerWithAddressesForUpdateDto.Id){ return BadRequest();}

        var rightCustomer = await _customerRepository.GetCustomerByIdAsync(customerId);//teste/////////
        // var rightCustomer = _context.Customers.Include(c => c.Addresses).FirstOrDefault(n => n.Id == id);//o que "funciona"////////
        if(rightCustomer == null){ return NotFound();}

        _customerRepository.UpdateCustomerWithAddresses(customerWithAddressesForUpdateDto, rightCustomer);
        await _customerRepository.SaveChangesAsync();

        return NoContent();

    }

}