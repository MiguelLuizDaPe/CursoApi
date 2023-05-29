namespace Curso.Api.Entities;

public class Customer{
    public int Id {get; set;}
    public string Name {get; set;} = "";
    public string Cpf {get; set;} = "";
    public Customer(int Id, string Name, string Cpf){
        this.Id = Id;
        this.Cpf = Cpf;
        this.Name = Name;
    }
}
// public class CustomerDto{
//     public int Id {get; set;}
//     public string Name {get; set;} = "";
//     public string Cpf {get; set;} = "";
//     public CustomerDto(int Id,string Name, string Cpf, string cpf)
//     {
//         this.Id = Id;
//         this.Cpf = Cpf;
//         this.Name = Name;
//     }
// }

public class Endereco{
    public string logradouro {get; set;} = "";
    public int numero {get; set;}
    public string cidade {get; set;} = "";
    public string bairro {get; set;} = "";
}