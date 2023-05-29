using Curso.Api;
using Microsoft.AspNetCore.Mvc;
using Curso.Api.Entities;
using Cursp.Api.Models;

namespace Curso.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase{

    [HttpGet]
    public ActionResult<IEnumerable<CustomerDto>> GetCustomers(){
       var listCustomer = Data.getInstance().Customers; //.Select(c => new CustomerDto {Id = c.Id, });
       var result = new List<CustomerDto>();
       
       foreach(Customer c in listCustomer){
            var newC = new CustomerDto{Id = c.Id, Name = c.Name, Cpf = c.Cpf};
            result.Add(newC);  
       }

       return Ok(result);
    }

    [HttpGet("{id}", Name = "GetCustomerById")]
    public ActionResult<CustomerDto> GetCustomerById(int id){
        //O n dentro da lambda é o objeto customer pq o FirstOrDefault() retorna elemento de mesma tipagem, por isso é possível interagi com o Id e compara-lo
        var customer = Data.getInstance().Customers.FirstOrDefault(n => n.Id == id);
        if(customer == null){return NotFound();}
        var customerForReturn = new CustomerDto{
            Id = customer.Id,
            Name = customer.Name, 
            Cpf = customer.Cpf
        };

        return Ok(customerForReturn);

        //testa customer diferente de null, se for efetua a opção 1 se não efetua a segunda opção
        // return customer != null ? Ok(customer) : NotFound();

    }

    [HttpGet("cpf/{cpf}")]
    public ActionResult<CustomerDto> GetCustomerCpf(string cpf){
        //O n dentro da lambda é o objeto customer pq o FirstOrDefault() retorna elemento de mesma tipagem, por isso é possível interagi com o Id e compara-lo
        var customer = Data.getInstance().Customers.FirstOrDefault(n => n.Cpf == cpf);
        if(customer == null){return NotFound();}
        var dtoCustomer = new CustomerDto{
            Id = customer.Id,
            Name = customer.Name, 
            Cpf = customer.Cpf
        };;

        return Ok(dtoCustomer);

        //testa customer diferente de null, se for efetua a opção 1 se não efetua a segunda opção
        // return customer != null ? Ok(customer) : NotFound();

    }

    //adiciona em elemento,masi pra frente boto pra adicionar em lista
    [HttpPost]
    public ActionResult<CustomerDto> CreateCustomer(CustomerDto customer){

        var rightCustomer = Data.getInstance().Customers.FirstOrDefault(n => n.Cpf == customer.Cpf);
        if(rightCustomer != null){return Conflict();}

        var newCustomer = new Customer(
            Data.getInstance().Customers.Max(n => n.Id) + 1, 
            customer.Name, 
            customer.Cpf
        );

        Data.getInstance().Customers.Add(newCustomer);

        return CreatedAtRoute(
            "GetCustomerById",
            new {id = newCustomer.Id},
            newCustomer
        );
    }

    [HttpPut("{id}")]
    public ActionResult<CustomerDto> PutCustomer(CustomerDto customer, int Id){

        var rightCustomer = Data.getInstance().Customers.FirstOrDefault(n => n.Id == Id);
        if(rightCustomer == null){return NotFound();}

        var updatedCustomer = new Customer(
            Id,
            customer.Name,
            customer.Cpf
        );

        Data.getInstance().Customers.Remove(rightCustomer);
        Data.getInstance().Customers.Add(updatedCustomer);
    
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult<CustomerDto> DeleteCustomer(int id){
        var customer = Data.getInstance().Customers.FirstOrDefault(n => n.Id == id);
        if(customer == null){return NotFound();}
        Data.getInstance().Customers.Remove(customer);
        return NoContent();
    }
}