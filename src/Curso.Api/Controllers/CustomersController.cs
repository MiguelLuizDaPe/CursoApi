using Curso.Api;
using Microsoft.AspNetCore.Mvc;
using Curso.Api.Entities;
using Curso.Api.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace Curso.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase{

    [HttpGet]
    public ActionResult<IEnumerable<CustomerDto>> GetCustomers(){
       var customersForReturn = Data.getInstance().Customers.Select(c => new CustomerDto (c.Id, c.Name, c.Cpf));
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
        var customerForReturn = new CustomerDto(
            customerFromDatabase.Id,
            customerFromDatabase.Name, 
            customerFromDatabase.Cpf
        );

        return Ok(customerForReturn);

        //testa customer diferente de null, se for efetua a opção 1 se não efetua a segunda opção
        // return customer != null ? Ok(customer) : NotFound();

    }

    [HttpGet("cpf/{cpf}")]
    public ActionResult<CustomerDto> GetCustomerCpf(string cpf){
        //O n dentro da lambda é o objeto customer pq o FirstOrDefault() retorna elemento de mesma tipagem, por isso é possível interagi com o Id e compara-lo
        var customerFromDatabase = Data.getInstance().Customers.FirstOrDefault(n => n.Cpf == cpf);
        if(customerFromDatabase == null){return NotFound();}
        var dtoCustomer = new CustomerDto(
            customerFromDatabase.Id,
            customerFromDatabase.Name, 
            customerFromDatabase.Cpf
        );

        return Ok(dtoCustomer);

        //testa customer diferente de null, se for efetua a opção 1 se não efetua a segunda opção
        // return customer != null ? Ok(customer) : NotFound();

    }

    //adiciona em elemento,masi pra frente boto pra adicionar em lista
    [HttpPost]
    public ActionResult<CustomerDto> CreateCustomer(CustomerForCreationDto customerForCreationDto){

        var rightCustomer = Data.getInstance().Customers.FirstOrDefault(n => n.Cpf == customerForCreationDto.Cpf);
        if(rightCustomer != null){return Conflict();}

        var customerEntity = new Customer(
            Data.getInstance().Customers.Max(n => n.Id) + 1, 
            customerForCreationDto.Name, 
            customerForCreationDto.Cpf
        );

        Data.getInstance().Customers.Add(customerEntity);

        var customerForReturn = new CustomerDto(
            customerEntity.Id, 
            customerForCreationDto.Name, 
            customerForCreationDto.Cpf
        );


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
            Cpf = customerFromDatabase.Cpf
        };

        patchDocument.ApplyTo(customerToPatch);

        customerFromDatabase.Name = customerToPatch.Name;
        customerFromDatabase.Cpf = customerToPatch.Cpf;

        return NoContent();

    }
}