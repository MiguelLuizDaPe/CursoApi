namespace Curso.Api.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerDto{
    public int Id {get;set;}
    public string Name {get; set;} = "";
    public string Cpf {get; set;} = "";
}
