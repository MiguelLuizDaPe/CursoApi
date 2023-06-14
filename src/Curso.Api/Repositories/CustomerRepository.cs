using AutoMapper;
using Curso.Api.DbContexts;
using Curso.Api.Entities;
using Curso.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Curso.Api.Repositories;

public class CustomerRepository : ICustomerRepository{
    private readonly CustomerContext _context;
    private readonly IMapper _mapper;

    public CustomerRepository(CustomerContext customerContext, IMapper mapper){
        _context = customerContext;
        _mapper = mapper;
    }

    //CUSTOMER//

    public async Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        return await _context.Customers.OrderBy(c => c.Id).ToListAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(int customerId)
    {
        return await _context.Customers.FirstOrDefaultAsync(n => n.Id == customerId);
    }

    public async Task<Customer?> GetCustomerByCpfAsync(string customerCpf)
    {
        return await _context.Customers.FirstOrDefaultAsync(n => n.Cpf == customerCpf);
    }

    public void AddCustomer(Customer customerEntity)
    {
        _context.Customers.Add(customerEntity);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() > 0);

    }

    public void RemoveCustomer(Customer customer)
    {
        _context.Customers.Remove(customer);
    }

    public void UpdateCustomer(CustomerForUpdateDto customerForUpdateDto, Customer customer)
    {
        _mapper.Map(customerForUpdateDto, customer);
    }

    public void PatchCustomer(CustomerForPatchDto customerForPatchDto, Customer customer)
    {
        _mapper.Map(customerForPatchDto, customer);

    }

    public async Task<IEnumerable<Customer>> GetCustomersWithAddressesAsync()
    {
        return await _context.Customers.Include(c => c.Addresses).ToListAsync();
    }

    public async Task<Customer?> GetCustomerWithAddressesAsync(int customerId)
    {
        return await _context.Customers.Include(c => c.Addresses).FirstOrDefaultAsync(n => n.Id == customerId);
    }

    public void AddCustomerWithAddresses(Customer customer)
    {
        _context.Customers.Add(customer);

    }

    public void UpdateCustomerWithAddresses(CustomerWithAddressesForUpdateDto customerWithAddressesForUpdateDto, Customer customer)
    {
        _mapper.Map(customerWithAddressesForUpdateDto, customer);
        customer.Addresses = customerWithAddressesForUpdateDto.Addresses.Select(a => _mapper.Map<Address>(a)).ToList();
    }


    //ADDRESS//

    public async Task<IEnumerable<Address>> GetAddressesFromCustomerAsync(int customerId)
    {
        IEnumerable<Address> addressList = await _context.Addresses.ToListAsync();
        return addressList.ToList().FindAll(a => a.CustomerId == customerId).OrderBy(c => c.Id);
    }

    public async Task<Address?> GetAddressFromCustomerAsync(int customerId, int addressId)
    {
        return await _context.Addresses.FirstOrDefaultAsync(a => a.CustomerId == customerId && a.Id == addressId);
    }

    public void AddAddressInCustomer(int customerId, Address address)
    {
        _context.Customers.FirstOrDefault(c => c.Id == customerId)?.Addresses.Add(address);
    }

    public void UpdateAddressInCustomer(AddressForUpdateDto addressForUpdateDto, Address address)
    {
        _mapper.Map(addressForUpdateDto, address);
    }

    public void RemoveAddressFromCustomer(Address address)
    {
        _context.Addresses.Remove(address);
    }
}