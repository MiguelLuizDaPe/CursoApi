namespace Curso.Api.Entities;

public class Customer{
    public int Id {get; set;}
    public string Name {get; set;} = "";
    public string Cpf {get; set;} = "";
    public ICollection<Address> Addresses {get; set;} = new List<Address>();
    public Customer(int Id, string Name, string Cpf){
        this.Id = Id;
        this.Cpf = Cpf;
        this.Name = Name;
    }
    public Customer(){}
}