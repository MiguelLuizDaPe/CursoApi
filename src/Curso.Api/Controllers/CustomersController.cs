using Curso.Api;
using Microsoft.AspNetCore.Mvc;
using Curso.Api.Entities;

namespace Curso.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase{
    internal class CustomerDto : Customer{
        public CustomerDto(string Name, string Cpf, int Id = -1){
            if(Id != -1){this.Id = Id;}
            else{this.Id = Data.getInstance().Customers.Max(n => n.Id) + 1;}
            this.Name = Name;
            this.Cpf = Cpf;
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<Customer>> GetCustomers(){
       var listCustomer = Data.getInstance().Customers;
       var result = new List<CustomerDto>();
       
       foreach(Customer c in listCustomer){
            var newC = new CustomerDto(c.Name, c.Cpf, c.Id);
            result.Add(newC);  
       }

       return Ok(result);
    }

    [HttpGet("{id}", Name = "GetCustomerById")]
    public ActionResult<Customer> GetCustomerById(int id){
        //O n dentro da lambda é o objeto customer pq o FirstOrDefault() retorna elemento de mesma tipagem, por isso é possível interagi com o Id e compara-lo
        var customer = Data.getInstance().Customers.FirstOrDefault(n => n.Id == id);
        if(customer == null){return NotFound();}
        var dtoCustomer = new CustomerDto(customer.Name, customer.Cpf, customer.Id);

        return Ok(dtoCustomer);

        //testa customer diferente de null, se for efetua a opção 1 se não efetua a segunda opção
        // return customer != null ? Ok(customer) : NotFound();

    }

    [HttpGet("cpf/{cpf}")]
    public ActionResult<Customer> GetCustomerCpf(string cpf){
        //O n dentro da lambda é o objeto customer pq o FirstOrDefault() retorna elemento de mesma tipagem, por isso é possível interagi com o Id e compara-lo
        var customer = Data.getInstance().Customers.FirstOrDefault(n => n.Cpf == cpf);
        if(customer == null){return NotFound();}
        var dtoCustomer = new CustomerDto(customer.Name, customer.Cpf, customer.Id);

        return Ok(customer);

        //testa customer diferente de null, se for efetua a opção 1 se não efetua a segunda opção
        // return customer != null ? Ok(customer) : NotFound();

    }

    //adiciona em elemento,masi pra frente boto pra adicionar em lista
    [HttpPost]
    public ActionResult<Customer> CreateCustomer(Customer customer){
        var newCustomer = new CustomerDto(customer.Name, customer.Cpf);

        var rightCustomer = Data.getInstance().Customers.FirstOrDefault(n => n.Cpf == newCustomer.Cpf);
        if(rightCustomer != null){return Conflict();}
        Data.getInstance().Customers.Add(newCustomer);

        return CreatedAtRoute(
            "GetCustomerById",
            new {id = newCustomer.Id},
            newCustomer
        );
    }

    [HttpPut("{id}")]
    public ActionResult<Customer> PutCustomer(Customer customer, int Id){
        var updatedCustomer = new CustomerDto(customer.Name, customer.Cpf, Id);

        var rightCustomer = Data.getInstance().Customers.FirstOrDefault(n => n.Id == updatedCustomer.Id);
        if(rightCustomer == null){return NotFound();}
        Data.getInstance().Customers.Remove(rightCustomer);
        Data.getInstance().Customers.Add(updatedCustomer);
    
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult<Customer> DeleteCustomer(int id){
        var customer = Data.getInstance().Customers.FirstOrDefault(n => n.Id == id);
        if(customer == null){return NotFound();}
        Data.getInstance().Customers.Remove(customer);
        return NoContent();
    }
}