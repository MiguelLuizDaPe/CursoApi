namespace Curso.Api.Entities;
public class Address{
    public int Id {get; set;}
    public string Street {get; set;} = "";
    public string City {get; set;} = "";
    public Address(int Id, string Street, string City){
        this.Id = Id;
        this.Street = Street;
        this.City = City;
    }
    public Address(){}
}