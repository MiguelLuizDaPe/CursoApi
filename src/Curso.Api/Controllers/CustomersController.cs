using Curso.Api;
using Microsoft.AspNetCore.Mvc;
using Curso.Api.Entities;
using Curso.Api.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Curso.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase{

    private int addrIdMax = 0;
    private int genUniqueAddrId(){
        if(addrIdMax == 0){
            foreach(Customer c in Data.getInstance().Customers){
                int idMax = c.Addresses.Max(n => n.Id);
                if(idMax > addrIdMax){addrIdMax = idMax;}
            }
        }
        addrIdMax += 1;
        return addrIdMax;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CustomerDto>> GetCustomers(){
       var customersForReturn = Data.getInstance().Customers.Select(c => new CustomerDto{Id = c.Id, Name = c.Name, Cpf = c.Cpf});
       // meu código a baixo, é a mesma coisa que a putaria do select, mas é mais usado o select
    //    var customersForReturn = new List<CustomerDto>();
       
    //    foreach(Customer c in listCustomer){
    //         var newC = new CustomerDto{Id = c.Id, Name = c.Name, Cpf = c.Cpf};
    //         result.Add(newC);  
    //    }

       return Ok(customersForReturn);
    }

    [HttpGet("{id}", Name = "GetCustomerById")]
    public ActionResult<CustomerDto> GetCustomerById(int id){
        //O n dentro da lambda é o objeto customer pq o FirstOrDefault() retorna elemento de mesma tipagem, por isso é possível interagi com o Id e compara-lo
        var customerFromDatabase = Data.getInstance().Customers.FirstOrDefault(n => n.Id == id);
        if(customerFromDatabase == null){return NotFound();}
        var customerForReturn = new CustomerDto{
            Id = customerFromDatabase.Id,
            Name = customerFromDatabase.Name, 
            Cpf = customerFromDatabase.Cpf
        };

        return Ok(customerForReturn);

        //testa customer diferente de null, se for efetua a opção 1 se não efetua a segunda opção
        // return customer != null ? Ok(customer) : NotFound();

    }

    [HttpGet("cpf/{cpf}")]
    public ActionResult<CustomerDto> GetCustomerCpf(string cpf){
        //O n dentro da lambda é o objeto customer pq o FirstOrDefault() retorna elemento de mesma tipagem, por isso é possível interagi com o Id e compara-lo
        var customerFromDatabase = Data.getInstance().Customers.FirstOrDefault(n => n.Cpf == cpf);
        if(customerFromDatabase == null){return NotFound();}
        var dtoCustomer = new CustomerDto{
            Id = customerFromDatabase.Id,
            Name = customerFromDatabase.Name, 
            Cpf = customerFromDatabase.Cpf
        };

        return Ok(dtoCustomer);

        //testa customer diferente de null, se for efetua a opção 1 se não efetua a segunda opção
        // return customer != null ? Ok(customer) : NotFound();

    }

    [HttpPost]
    public ActionResult<CustomerDto> CreateCustomer(CustomerForCreationDto customerForCreationDto){

        if(!ModelState.IsValid){
            //Cria a fábrica de um objeto de detalhes de um problema de validação
            var problemDetailsFactory = HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            //Cria um objeto de detalhes de um problema de validação
            var validationProblemDetails = problemDetailsFactory.CreateValidationProblemDetails(HttpContext, ModelState);
            //Aribui o statis code 422 no corpo do response
            validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;

            return UnprocessableEntity(validationProblemDetails)/*que é um erro 422*/;
        }

        var customerEntity = new Customer{
            Id = Data.getInstance().Customers.Max(n => n.Id) + 1, 
            Name = customerForCreationDto.Name, 
            Cpf = customerForCreationDto.Cpf
        };

        Data.getInstance().Customers.Add(customerEntity);

        var customerForReturn = new CustomerDto{
            Id = customerEntity.Id, 
            Name = customerForCreationDto.Name, 
            Cpf = customerForCreationDto.Cpf
        };


        return CreatedAtRoute(
            "GetCustomerById",
            new {id = customerForReturn.Id},
            customerForReturn
        );
    }

    [HttpPut("{id}")]
    public ActionResult UpdateCustomer( int Id, CustomerForUpdateDto customerForUpdateDto){
        //verificação de Id pq pode acontecer um ataque malicioso com intenções maléficas de pura maldade
        if(Id != customerForUpdateDto.Id){return BadRequest();}

        var rightCustomer = Data.getInstance().Customers.FirstOrDefault(n => n.Id == Id);
        if(rightCustomer == null){return NotFound();}

        var updatedCustomer = new Customer(
            customerForUpdateDto.Id,
            customerForUpdateDto.Name,
            customerForUpdateDto.Cpf
        );

        rightCustomer.Name = customerForUpdateDto.Name;
        rightCustomer.Cpf = customerForUpdateDto.Cpf;
    
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult<CustomerDto> DeleteCustomer(int id){
        var customer = Data.getInstance().Customers.FirstOrDefault(n => n.Id == id);
        if(customer == null){return NotFound();}
        Data.getInstance().Customers.Remove(customer);
        return NoContent();
    }

    //quando possível pesquisar sobre isso (talvez)
    [HttpPatch("{id}")]
    public ActionResult PartiallyUpdateCustomer([FromBody] JsonPatchDocument<CustomerForPatchDto> patchDocument, [FromRoute] int id){
        var customerFromDatabase = Data.getInstance().Customers.FirstOrDefault(n => n.Id == id);
        if(customerFromDatabase == null){return NotFound();}

        var customerToPatch = new CustomerForPatchDto{
            Name = customerFromDatabase.Name,
            Cpf = customerFromDatabase.Cpf,
        };

        patchDocument.ApplyTo(customerToPatch);

        customerFromDatabase.Name = customerToPatch.Name;
        customerFromDatabase.Cpf = customerToPatch.Cpf;

        return NoContent();

    }

    [HttpGet("with-address")]
    public ActionResult<IEnumerable<CustomerWithAddressesDto>> GetCustomersWithAddresses(){
        
        var customersFromDatabase = Data.getInstance().Customers;
        var customersToReturn = customersFromDatabase.Select(customer => new CustomerWithAddressesDto{
            Id = customer.Id,
            Name = customer.Name,
            Cpf = customer.Cpf,
            Addresses = customer.Addresses.Select(address => new AddressDto{
                Id = address.Id,
                Street = address.Street,
                City = address.City
            }).ToList()

        });

        return Ok(customersToReturn); // O customerToReturn não precisa de .ToList() pq o Ok() serializa
    }

    [HttpGet("{id}/with-address")]
    public ActionResult<CustomerWithAddressesDto> GetCustomerWithAddresses(int id){

        var customerFromDatabase = Data.getInstance().Customers.FirstOrDefault(n => n.Id == id);
        if(customerFromDatabase == null){return NotFound();}

        var customerForReturn = new CustomerWithAddressesDto{
            Id = customerFromDatabase.Id,
            Name = customerFromDatabase.Name, 
            Cpf = customerFromDatabase.Cpf,
            Addresses = customerFromDatabase.Addresses.Select(a => new AddressDto{
                Id = a.Id,
                Street = a.Street,
                City = a.City
            }).ToList()
        };

        return Ok(customerForReturn);
    }

    [HttpPost("with-address")]
    public ActionResult<CustomerWithAddressesDto> CreatCustomerWithAddresses(int id, CustomerWithAddressesForCreationDto customerWithAddressesForCreationDto){
            // int n = 0;
            // int x = n++ + ++n;
        
        var customerEntity = new Customer{
            Id = Data.getInstance().Customers.Max(n => n.Id) + 1,
            Name = customerWithAddressesForCreationDto.Name,
            Cpf = customerWithAddressesForCreationDto.Cpf,
            Addresses = customerWithAddressesForCreationDto.Addresses.Select(a => new Address{
                Id = genUniqueAddrId(), // Desse jeito não funciona para esse caso pois ele não tem acesso 
                Street = a.Street,      //a ele mesmo para colocar um id diferente no próximo da lista
                City = a.City
            }).ToList()
        };

        Data.getInstance().Customers.Add(customerEntity);

        var customerForReturn = new CustomerWithAddressesDto{
            Id = customerEntity.Id,
            Name = customerWithAddressesForCreationDto.Name,
            Cpf = customerWithAddressesForCreationDto.Cpf,
            Addresses = customerEntity.Addresses.Select(a => new AddressDto{
                Id = a.Id,
                Street = a.Street,
                City = a.City
            }).ToList()
        };

        return CreatedAtRoute(
            "GetCustomerById",
            new {id = customerForReturn.Id},
            customerForReturn
        );
    }

    [HttpPut("{id}/with-address")]
    public ActionResult UpdateCustomerWithAddresses(int id, CustomerWithAddressesForUpdateDto customerWithAddressesForUpdateDto){
        if(id != customerWithAddressesForUpdateDto.Id){ return BadRequest();}

        var rightCustomer = Data.getInstance().Customers.FirstOrDefault(n => n.Id == id);
        if(rightCustomer == null){ return BadRequest();}

        var updatedCustomer = new Customer{
            Id = customerWithAddressesForUpdateDto.Id,
            Name = customerWithAddressesForUpdateDto.Name,
            Cpf = customerWithAddressesForUpdateDto.Cpf,
            Addresses = customerWithAddressesForUpdateDto.Addresses.Select(a => new Address{
                Id = genUniqueAddrId(),
                Street = a.Street,
                City = a.City
            }).ToList()
        };

        rightCustomer.Name = updatedCustomer.Name;
        rightCustomer.Cpf = updatedCustomer.Cpf;
        rightCustomer.Addresses = updatedCustomer.Addresses;

        return NoContent();

    }

}