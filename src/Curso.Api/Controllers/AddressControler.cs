using Microsoft.AspNetCore.Mvc;

namespace Curso.Api.Controllers;

[ApiController]
[Route("api/customers/{customerId}/addresses")]
public class AddressController : ControllerBase{
    [HttpGet] //copiar essa porra de alguém depois também
    public Action<IEnumerable<AddressesDto>> GetAdresses(int){

    }
}