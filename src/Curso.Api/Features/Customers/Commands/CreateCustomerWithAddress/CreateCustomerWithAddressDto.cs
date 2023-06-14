using Curso.Api.Models;

namespace Curso.Api.Features.Customers.Commands.CreateCustomerWithAddress;

public class CreateCustomerWithAddressDto{
    public int Id {get; set;}
    public string Name {get; set;} = "";
    public string Cpf {get; set;} = "";
    public ICollection<AddressDto> Addresses {get; set;} = new List<AddressDto>();
}
