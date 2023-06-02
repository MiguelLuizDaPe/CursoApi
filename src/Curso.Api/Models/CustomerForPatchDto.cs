namespace Curso.Api.Models;

public class CustomerForPatchDto{
    public string Name {get;set;} = "";
    public string Cpf {get;set;} = "";
    public List<Endereco> Enderecos {get;set;}
}