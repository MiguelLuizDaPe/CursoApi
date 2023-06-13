namespace Curso.Api.Features.Customers.Queries.GetCustomerByCpf;


public class GetCustomerByCpfDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
}