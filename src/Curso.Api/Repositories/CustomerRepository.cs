using Curso.Api.DbContexts;
using Curso.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Curso.Api.Repositories;

public class CustomerRepository : ICustomerRepository{
    private readonly CustomerContext _context;

    public CustomerRepository(CustomerContext customerContext){
        _context = customerContext;
    }

    public Customer? GetCustomerById(int customerId)
    {
        return _context.Customers.FirstOrDefault(n => n.Id == customerId);
    }

    public async Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        return await _context.Customers.OrderBy(c => c.Name).ToListAsync();
    }
}