namespace Curso.Api.Models;

public class AddressDto{
    public int Id {get; set;}
    public string Street {get; set;} = "";
    public string City {get; set;} = "";
    public AddressDto(int Id, string Street, string City){
        this.Id = Id;
        this.Street = Street;
        this.City = City;
    }
    public AddressDto(){}
}