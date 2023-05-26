using Curso.Api;
using Microsoft.AspNetCore.Mvc;
using Curso.Api.Entities;

namespace Curso.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase{
    [HttpGet]
    public ActionResult<IEnumerable<Customer>> GetCustomers(){
       var result = Data.getData();
       return Ok(result);
    }

    [HttpGet("{id}", Name = "GetCustomerById")]
    public ActionResult<Customer> GetCustomerById(int id){
        //O n dentro da lambda é o objeto customer pq o FirstOrDefault() retorna elemento de mesma tipagem, por isso é possível interagi com o Id e compara-lo
        var customer = Data.getData().Customers.FirstOrDefault(n => n.Id == id);
        if(customer == null){return NotFound("Deu merda");}
        return Ok(customer);

        //testa customer diferente de null, se for efetua a opção 1 se não efetua a segunda opção
        // return customer != null ? Ok(customer) : NotFound("Deu merda");

    }

    [HttpGet("cpf/{cpf}")]
    public ActionResult<Customer> GetCustomerCpf(string cpf){
        //O n dentro da lambda é o objeto customer pq o FirstOrDefault() retorna elemento de mesma tipagem, por isso é possível interagi com o Id e compara-lo
        var customer = Data.getData().Customers.FirstOrDefault(n => n.Cpf == cpf);
        if(customer == null){return NotFound("Deu merda");}
        return Ok(customer);

        //testa customer diferente de null, se for efetua a opção 1 se não efetua a segunda opção
        // return customer != null ? Ok(customer) : NotFound("Deu merda");

    }

    //adiciona em elemento,masi pra frente boto pra adicionar em lista
    [HttpPost]
    public ActionResult<Customer> CreateCustomer (Customer customer){
        var newCustomer = new Customer(){
            Id = Data.getData().Customers.Max(n => n.Id) + 1,
            Name = customer.Name,
            Cpf = customer.Cpf
        };

        var rightCustomer = Data.getData().Customers.FirstOrDefault(n => n.Cpf == newCustomer.Cpf);
        if(rightCustomer != null){return Conflict();}
        else{Data.getData().Customers.Add(newCustomer);}
        
        return CreatedAtRoute(
            "GetCustomerById",
            new {id = newCustomer.Id},
            newCustomer
        );
    }
}