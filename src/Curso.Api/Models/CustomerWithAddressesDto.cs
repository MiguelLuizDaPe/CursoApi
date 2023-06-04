using Curso.Api.Entities;

namespace Curso.Api.Models;

public class CustomerWithAddressesDto{
    public int Id {get; set;}
    public string Name {get; set;} = "";
    public string Cpf {get; set;} = "";
    public ICollection<AddressDto> Addresses {get; set;} = new List<AddressDto>();
}