using Curso.Api.Entities;

namespace Curso.Api.Repositories;

public interface ICustomerRepository{
    Task<IEnumerable<Customer>> GetCustomersAsync();
    Customer? GetCustomerById(int customerId);
    Customer? GetCustomerByCpf(string customerCpf);
    void AddCustomer(Customer customerEntity);
    void SaveChanges();
    void RemoveCustomer(Customer customer);
}