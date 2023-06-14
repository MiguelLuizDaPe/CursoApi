namespace Curso.Api.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerDto{
    public int Id {get; set;}
    public string Name {get; set;} = "";
    public string Cpf {get; set;} = "";
}