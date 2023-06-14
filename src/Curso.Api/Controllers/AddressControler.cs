using AutoMapper;
using Curso.Api.DbContexts;
using Curso.Api.Entities;
using Curso.Api.Models;
using Curso.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Curso.Api.Controllers;

[ApiController]
[Route("api/customers/{customerId}/addresses")]
public class AddressesController : MainController{

    // private readonly Data _data;
    private readonly IMapper _mapper;
    private readonly CustomerContext _context;
    private readonly ICustomerRepository _customerRepository;

    public AddressesController(/*Data data,*/ IMapper mapper, CustomerContext context, ICustomerRepository customerRepository){//essa porra é uma injeção de dependência
        // _data = data ?? throw new ArgumentNullException(nameof(data));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }

    // private int addrIdMax = 0;
    // private int genUniqueAddrId(){
    //     if(addrIdMax == 0){
    //         foreach(Customer c in _data.Customers){
    //             if(!c.Addresses.Any()){continue;}
    //             int idMax = c.Addresses.Max(n => n.Id);
    //             if(idMax > addrIdMax){addrIdMax = idMax;}
    //         }
    //     }
    //     addrIdMax += 1;
    //     return addrIdMax;
    // }


    [HttpGet] //copiar essa porra de alguém depois também
    public async Task<ActionResult<IEnumerable<AddressDto>>> GetAdressesFromCustomer(int customerId){
        var addressesFromDatabase = await _customerRepository.GetAddressesFromCustomerAsync(customerId);

        var addressesToReturn = _mapper.Map<IEnumerable<AddressDto>>(addressesFromDatabase);

        if (!addressesToReturn.Any()){ return NotFound();}
        return Ok(addressesToReturn);
    }

    [HttpGet("{addressId}", Name = "GetAddressById")]
    public async Task<ActionResult<AddressDto>> GetAddressFromCustomer( int customerId, int addressId){
        var addressFromCustomer = await _customerRepository.GetAddressFromCustomerAsync(customerId, addressId);

        var addressToReturn = _mapper.Map<AddressDto>(addressFromCustomer);
        if(addressToReturn == null) return NotFound();

        return Ok(addressToReturn);


        //var addressToReturn = _data.Customers.FirstOrDefault(c => c.Id == customerId).Addresses.FirstOrDefault(a => a.Id == addressId);

    }

    [HttpPost]
    public async Task<ActionResult<AddressDto>> CreatAddress(int customerId, AddressForCreationDto addressForCreationDto){

        var customerFromDatabase = await _customerRepository.GetCustomerWithAddressesAsync(customerId);
        if(customerFromDatabase == null){return NotFound();}

        var addressEntity = _mapper.Map<Address>(addressForCreationDto);

        _customerRepository.AddAddressInCustomer(customerId, addressEntity);
        _customerRepository.SaveChanges();

        var addressToReturn = _mapper.Map<AddressDto>(addressEntity);

        return CreatedAtRoute(
            "GetAddressById",
            new {customerId = customerFromDatabase.Id, addressId = addressToReturn.Id},
            addressToReturn
        );
    }

    [HttpPut("{addressId}")]
    public async Task<ActionResult> UpdateCustomer(int customerId, int addressId, AddressForUpdateDto addressForUpdateDto){
        if(addressId != addressForUpdateDto.Id){return BadRequest();}

        var customerFromDatabase = await _customerRepository.GetCustomerWithAddressesAsync(customerId);
        if(customerFromDatabase == null){return NotFound();}

        var rightAddressFromCustomer = customerFromDatabase.Addresses.FirstOrDefault(a => a.Id == addressId);
        if(rightAddressFromCustomer == null){return NotFound();}

        _customerRepository.UpdateAddressInCustomer(addressForUpdateDto, rightAddressFromCustomer);
        _customerRepository.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{addressId}")]
    public async Task<ActionResult> DeleteAddress(int customerId, int addressId){
        var customerFromDatabase = await _customerRepository.GetCustomerWithAddressesAsync(customerId);
        if(customerFromDatabase == null){return NotFound();}

        var addressFromCustomer = customerFromDatabase.Addresses.FirstOrDefault(a => a.Id == addressId);
        if(addressFromCustomer == null){return NotFound();}

        _customerRepository.RemoveAddressFromCustomer(addressFromCustomer);
        _customerRepository.SaveChanges();

        return NoContent();

    }
}       