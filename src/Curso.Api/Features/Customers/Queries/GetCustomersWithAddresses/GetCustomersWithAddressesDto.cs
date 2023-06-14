using Curso.Api.Entities;
using Curso.Api.Models;

namespace Curso.Api.Features.Customers.Queries.GetCustomersWithAddresses;

public class GetCustomersWithAddressesDto{
    public int Id {get; set;}
    public string Name {get; set;} = "";
    public string Cpf {get; set;} = "";
    public ICollection<AddressDto> Addresses {get; set;} = new List<AddressDto>();
}