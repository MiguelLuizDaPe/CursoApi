namespace Curso.Api.Models;

public class AddressForUpdateDto{
    public int Id {get; set;}
    public string Street {get; set;} = "";
    public string City {get; set;} = "";
}