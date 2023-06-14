using Curso.Api.Entities;

namespace Curso.Api.Features.Customers.Queries.GetCustomersWithAddresses;

public class GetCustomersWithAddressesDto{
    public int Id {get; set;}
    public string Name {get; set;} = "";
    public string Cpf {get; set;} = "";
    public ICollection<Address> Addresses {get; set;} = new List<Address>();
}