namespace Curso.Api.Models;

public class CustomerForUpdateDto{
    public int Id {get;set;}
    public string Name {get;set;} = "";
    public string Cpf {get;set;} = "";
    public List<Endereco> Enderecos {get;set;}
}