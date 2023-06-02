using Curso.Api.Entities;

namespace Curso.Api.Models;

// public class CustomerDto{
//     public string FirstName {private get; set;} = "";
//     public string LastName {private get; set;} = "";
//     public string FullName{
//         get{
//             return FirstName + " " + LastName;
//         }
//     }
//     public string Cpf {get; set;} = "";
// }
//isso é só um exemplo pra lembrar de algo, se fode arrumando depois

public class CustomerDto{
    public int Id {get; set;}
    public string Name {get; set;} = "";
    public string Cpf {get; set;} = "";
    public List<Address> Enderecos {get; set;}
    public CustomerDto(int Id, string Name, string Cpf){
        this.Id = Id;
        this.Cpf = Cpf;
        this.Name = Name;
        this.Enderecos = Enderecos;
    }
}