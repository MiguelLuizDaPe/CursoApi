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
    [HttpPost]
    [Route("api/customers/post")]
    public IActionResult Post ([FromBody] Customer customer){
        Data.getData().Customers.Add(customer);
        return Ok(customer);
    }
}