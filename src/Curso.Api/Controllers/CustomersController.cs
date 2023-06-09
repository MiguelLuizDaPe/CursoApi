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

namespace Curso.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : MainController{

    // private readonly Data _data;
    private readonly IMapper _mapper;
    private readonly CustomerContext _context;
    private readonly ICustomerRepository _customerRepository;

    public CustomersController(/*Data data, */IMapper mapper, CustomerContext context, ICustomerRepository customerRepository){//essa porra é uma injeção de dependência
        // _data = data ?? throw new ArgumentNullException(nameof(data));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers(){
       var customersFromDatabase = await _customerRepository.GetCustomersAsync();//callback hell da uma pesquisadinha depois
       var customersForReturn = _mapper.Map<IEnumerable<CustomerDto>>(customersFromDatabase);

       return Ok(customersForReturn);
    }

    [HttpGet("{customerId}", Name = "GetCustomerById")]
    public ActionResult<CustomerDto> GetCustomerById(int customerId){
        //O n dentro da lambda é o objeto customer pq o FirstOrDefault() retorna elemento de mesma tipagem, por isso é possível interagi com o Id e compara-lo
        var customerFromDatabase = _customerRepository.GetCustomerById(customerId);//metodos de agregação não precisam de ToList(), como o FirstOrDefault()
        if(customerFromDatabase == null){return NotFound();}
        var customerForReturn = _mapper.Map<CustomerDto>(customerFromDatabase);
        // var customerForReturn = new CustomerDto{
        //     Id = customerFromDatabase.Id,
        //     Name = customerFromDatabase.Name, 
        //     Cpf = customerFromDatabase.Cpf
        // };

        return Ok(customerForReturn);

        //testa customer diferente de null, se for efetua a opção 1 se não efetua a segunda opção
        // return customer != null ? Ok(customer) : NotFound();

    }

    [HttpGet("cpf/{cpf}")]
    public ActionResult<CustomerDto> GetCustomerCpf(string cpf){
        //O n dentro da lambda é o objeto customer pq o FirstOrDefault() retorna elemento de mesma tipagem, por isso é possível interagi com o Id e compara-lo
        var customerFromDatabase = _context.Customers.FirstOrDefault(n => n.Cpf == cpf);
        if(customerFromDatabase == null){return NotFound();}
        var customerForReturn = _mapper.Map<CustomerDto>(customerFromDatabase);

        // var customerForReturn = new CustomerDto{
        //     Id = customerFromDatabase.Id,
        //     Name = customerFromDatabase.Name, 
        //     Cpf = customerFromDatabase.Cpf
        // };

        return Ok(customerForReturn);

        //testa customer diferente de null, se for efetua a opção 1 se não efetua a segunda opção
        // return customer != null ? Ok(customer) : NotFound();

    }

    [HttpPost]
    public ActionResult<CustomerDto> CreateCustomer(CustomerForCreationDto customerForCreationDto){

        // var customerEntity = new Customer{
        //     Id = _data.Customers.Max(n => n.Id) + 1, 
        //     Name = customerForCreationDto.Name, 
        //     Cpf = customerForCreationDto.Cpf
        // };

        // _data.Customers.Add(customerEntity);

        // var customerForReturn = new CustomerDto{
        //     Id = customerEntity.Id, 
        //     Name = customerForCreationDto.Name, 
        //     Cpf = customerForCreationDto.Cpf
        // };

        var customerEntity = _mapper.Map<Customer>(customerForCreationDto);
        // customerEntity.Id = _data.Customers.Max(n => n.Id) + 1;
        _context.Customers.Add(customerEntity);
        _context.SaveChanges();
        var customerForReturn = _mapper.Map<CustomerDto>(customerEntity);


        return CreatedAtRoute(
            "GetCustomerById",
            new {id = customerForReturn.Id},
            customerForReturn
        );
    }

    [HttpPut("{id}")]
    public ActionResult UpdateCustomer(int Id, CustomerForUpdateDto customerForUpdateDto){
        //verificação de Id pq pode acontecer um ataque malicioso com intenções maléficas de pura maldade
        if(Id != customerForUpdateDto.Id){return BadRequest();}

        var rightCustomer = _context.Customers.FirstOrDefault(n => n.Id == Id);
        if(rightCustomer == null){return NotFound();}

        // var updatedCustomer = new Customer(
        //     customerForUpdateDto.Id,
        //     customerForUpdateDto.Name,
        //     customerForUpdateDto.Cpf
        // );

        _mapper.Map(customerForUpdateDto, rightCustomer);
        _context.SaveChanges();

        // rightCustomer.Name = customerForUpdateDto.Name;
        // rightCustomer.Cpf = customerForUpdateDto.Cpf;
    
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult<CustomerDto> DeleteCustomer(int id){
        var customer = _context.Customers.FirstOrDefault(n => n.Id == id);
        if(customer == null){return NotFound();}
        _context.Customers.Remove(customer);
        _context.SaveChanges();
        return NoContent();
    }

    //quando possível pesquisar sobre isso (talvez)
    [HttpPatch("{id}")]
    public ActionResult PartiallyUpdateCustomer([FromBody]JsonPatchDocument<CustomerForPatchDto> patchDocument, [FromRoute] int id){
        var customerFromDatabase = _context.Customers.FirstOrDefault(n => n.Id == id);
        if(customerFromDatabase == null){return NotFound();}

        // var customerToPatchDto = new CustomerForPatchDto{
        //     Name = customerFromDatabase.Name,
        //     Cpf = customerFromDatabase.Cpf,
        // };

        var customerToPatchDto = _mapper.Map<CustomerForPatchDto>(customerFromDatabase);

        patchDocument.ApplyTo(customerToPatchDto, ModelState);

        if(!TryValidateModel(customerToPatchDto)){
            return ValidationProblem(ModelState);
        }

        // customerFromDatabase.Name = customerToPatchDto.Name;
        // customerFromDatabase.Cpf = customerToPatchDto.Cpf;

        _mapper.Map(customerToPatchDto, customerFromDatabase);
        _context.SaveChanges();

        return NoContent();

    }

    [HttpGet("with-address")]
    public ActionResult<IEnumerable<CustomerWithAddressesDto>> GetCustomersWithAddresses(){
        
        var customersFromDatabase = _context.Customers.Include(c => c.Addresses).ToList();

       var customersForReturn = _mapper.Map<IEnumerable<CustomerWithAddressesDto>>(customersFromDatabase);

        // var customersToReturn = customersFromDatabase.Select(customer => new CustomerWithAddressesDto{
        //     Id = customer.Id,
        //     Name = customer.Name,
        //     Cpf = customer.Cpf,
        //     Addresses = customer.Addresses.Select(address => new AddressDto{
        //         Id = address.Id,
        //         Street = address.Street,
        //         City = address.City
        //     }).ToList()

        // });

        return Ok(customersForReturn); // O customerToReturn não precisa de .ToList() pq o Ok() serializa
    }

    [HttpGet("{id}/with-address", Name = "GetCustomerByIdWithAddresses")]
    public ActionResult<CustomerWithAddressesDto> GetCustomerWithAddresses(int id){

        var customerFromDatabase = _context.Customers.Include(c => c.Addresses).FirstOrDefault(n => n.Id == id);//parece errado pra mim, pedir explicação depois pro professor
        if(customerFromDatabase == null){return NotFound();}

        // var customerForReturn = new CustomerWithAddressesDto{
        //     Id = customerFromDatabase.Id,
        //     Name = customerFromDatabase.Name, 
        //     Cpf = customerFromDatabase.Cpf,
        //     Addresses = customerFromDatabase.Addresses.Select(a => new AddressDto{
        //         Id = a.Id,
        //         Street = a.Street,
        //         City = a.City
        //     }).ToList()
        // };

        var customerForReturn = _mapper.Map<CustomerWithAddressesDto>(customerFromDatabase);

        return Ok(customerForReturn);
    }

    [HttpPost("with-address")]
    public ActionResult<CustomerWithAddressesDto> CreatCustomerWithAddresses(CustomerWithAddressesForCreationDto customerWithAddressesForCreationDto){
            // int n = 0;
            // int x = n++ + ++n;
        
        // var customerEntity = new Customer{
        //     Id = _data.Customers.Max(n => n.Id) + 1,
        //     Name = customerWithAddressesForCreationDto.Name,
        //     Cpf = customerWithAddressesForCreationDto.Cpf,
        //     Addresses = customerWithAddressesForCreationDto.Addresses.Select(a => new Address{
        //         Id = genUniqueAddrId(), // Desse jeito não funciona para esse caso pois ele não tem acesso 
        //         Street = a.Street,      //a ele mesmo para colocar um id diferente no próximo da lista
        //         City = a.City
        //     }).ToList()
        // };

        // _data.Customers.Add(customerEntity);

        // var customerForReturn = new CustomerWithAddressesDto{
        //     Id = customerEntity.Id,
        //     Name = customerWithAddressesForCreationDto.Name,
        //     Cpf = customerWithAddressesForCreationDto.Cpf,
        //     Addresses = customerEntity.Addresses.Select(a => new AddressDto{
        //         Id = a.Id,
        //         Street = a.Street,
        //         City = a.City
        //     }).ToList()
        // };

        var customerEntity = _mapper.Map<Customer>(customerWithAddressesForCreationDto);
        // customerEntity.Id = _data.Customers.Max(n => n.Id) + 1;
        // foreach(Address a in customerEntity.Addresses){
        //     a.Id = genUniqueAddrId();
        // }
        _context.Customers.Add(customerEntity);
        _context.SaveChanges();
        var customerForReturn = _mapper.Map<CustomerWithAddressesDto>(customerEntity);

        return CreatedAtRoute(
            "GetCustomerByIdWithAddresses",
            new {id = customerForReturn.Id},
            customerForReturn
        );
    }

    [HttpPut("{id}/with-address")]
    public ActionResult UpdateCustomerWithAddresses(int id, CustomerWithAddressesForUpdateDto customerWithAddressesForUpdateDto){
        if(id != customerWithAddressesForUpdateDto.Id){ return BadRequest();}

        var rightCustomer = _context.Customers.FirstOrDefault(n => n.Id == id);//teste/////////
        // var rightCustomer = _context.Customers.Include(c => c.Addresses).FirstOrDefault(n => n.Id == id);//o que "funciona"////////
        if(rightCustomer == null){ return NotFound();}

        // var updatedCustomer = new Customer{
        //     Id = customerWithAddressesForUpdateDto.Id,
        //     Name = customerWithAddressesForUpdateDto.Name,
        //     Cpf = customerWithAddressesForUpdateDto.Cpf,
        //     Addresses = customerWithAddressesForUpdateDto.Addresses.Select(a => new Address{
        //         Id = genUniqueAddrId(),
        //         Street = a.Street,
        //         City = a.City
        //     }).ToList()
        // };

        _mapper.Map(customerWithAddressesForUpdateDto, rightCustomer);//teste/////

        rightCustomer.Addresses = customerWithAddressesForUpdateDto.Addresses.Select(a => _mapper.Map<Address>(a)).ToList();//teste//talvez deixe como final(ficou legal)////


        // rightCustomer.Name = updatedCustomer.Name;
        // rightCustomer.Cpf = updatedCustomer.Cpf;
        // rightCustomer.Addresses = updatedCustomer.Addresses;

        // var updatedCustomer = _mapper.Map<Customer>(customerWithAddressesForUpdateDto);//o que "funciona"////////


        // foreach(Address a in updatedCustomer.Addresses){//isso é retardado e deve ter outro modo de fazer isso
        //     a.Id = genUniqueAddrId();
        // }


        // _mapper.Map(updatedCustomer, rightCustomer);//o feito que "funciona"//////
        _context.SaveChanges();



        return NoContent();

    }

}