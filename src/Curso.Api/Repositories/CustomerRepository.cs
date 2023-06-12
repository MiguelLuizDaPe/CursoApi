using Curso.Api.DbContexts;
using Curso.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Curso.Api.Repositories;

public class CustomerRepository : ICustomerRepository{
    private readonly CustomerContext _context;

    public CustomerRepository(CustomerContext customerContext){
        _context = customerContext;
    }

    public async Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        return await _context.Customers.OrderBy(c => c.Id).ToListAsync();
    }

    public Customer? GetCustomerById(int customerId)
    {
        return _context.Customers.FirstOrDefault(n => n.Id == customerId);
    }

    public Customer? GetCustomerByCpf(string customerCpf)
    {
        return _context.Customers.FirstOrDefault(n => n.Cpf == customerCpf);
    }

    public void AddCustomer(Customer customerEntity)
    {
        _context.Customers.Add(customerEntity);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();

    }

    public void RemoveCustomer(Customer customer)
    {
        _context.Customers.Remove(customer);
    }
}