using Curso.Api.Entities;
using Curso.Api.Models;

namespace Curso.Api.Repositories;

public interface ICustomerRepository{
    Task<IEnumerable<Customer>> GetCustomersAsync();
    Task<Customer?> GetCustomerByIdAsync(int customerId);
    Task<Customer?> GetCustomerByCpfAsync(string customerCpf);
    void AddCustomer(Customer customerEntity);
    void SaveChanges();
    void RemoveCustomer(Customer customer);
    void UpdateCustomer(CustomerForUpdateDto customerForUpdateDto, Customer customer);
    void PatchCustomer(CustomerForPatchDto customerForPatchDto, Customer customer);
    Task<IEnumerable<Customer>> GetCustomersWithAddressesAsync();
    Task<Customer?> GetCustomerWithAddressesAsync(int customerId);
    void AddCustomerWithAddresses(Customer customer);
    void UpdateCustomerWithAddresses(CustomerWithAddressesForUpdateDto customerWithAddressesForUpdateDto, Customer customer);
}