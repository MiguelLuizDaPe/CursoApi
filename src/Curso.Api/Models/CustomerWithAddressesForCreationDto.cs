using Curso.Api.Entities;

namespace Curso.Api.Models;

public class CustomerWithAddressesForCreationDto{
    public string Name {get; set;} = "";
    public string Cpf {get; set;} = "";
    public ICollection<AddressForCreationDto> Addresses {get; set;} = new List<AddressForCreationDto>();
}