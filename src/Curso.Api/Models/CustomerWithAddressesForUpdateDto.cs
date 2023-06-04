namespace Curso.Api.Models;

public class CustomerWithAddressesForUpdateDto{
    public int Id {get;set;}
    public string Name {get;set;} = "";
    public string Cpf {get;set;} = "";
    public ICollection<AddressForUpdateDto> Addresses {get; set;} = new List<AddressForUpdateDto>();

}