using Curso.Api.Entities;

namespace Curso.Api.Models;

public class CustomerWithAddressesDto{
    public int Id {get; set;}
    public string Name {get; set;} = "";
    public string Cpf {get; set;} = "";
    public ICollection<Address> Addresses {get; set;} = new List<Address>();
    public CustomerWithAddressesDto(int Id, string Name, string Cpf, List<Address> Addresses){
        this.Id = Id;
        this.Cpf = Cpf;
        this.Name = Name;
        this.Addresses = Addresses;
    }
    public CustomerWithAddressesDto(){}
}