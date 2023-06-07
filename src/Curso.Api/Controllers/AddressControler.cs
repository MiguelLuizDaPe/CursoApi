using AutoMapper;
using Curso.Api.Entities;
using Curso.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Curso.Api.Controllers;

[ApiController]
[Route("api/customers/{customerId}/addresses")]
public class AddressesController : MainController{

    private readonly Data _data;
    private readonly IMapper _mapper;


    public AddressesController(Data data, IMapper mapper){//essa porra é uma injeção de dependência
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    private int addrIdMax = 0;
    private int genUniqueAddrId(){
        if(addrIdMax == 0){
            foreach(Customer c in _data.Customers){
                if(!c.Addresses.Any()){continue;}
                int idMax = c.Addresses.Max(n => n.Id);
                if(idMax > addrIdMax){addrIdMax = idMax;}
            }
        }
        addrIdMax += 1;
        return addrIdMax;
    }


    [HttpGet] //copiar essa porra de alguém depois também
    public ActionResult<IEnumerable<AddressDto>> GetAdressesFromCustomer(int customerId){
        var customerFromDatabase = _data.Customers.FirstOrDefault(customer => customer.Id == customerId);
        if (customerFromDatabase == null){ return NotFound();}

        var addressesToReturn = _mapper.Map<IEnumerable<AddressDto>>(customerFromDatabase.Addresses);

        // var addressesToReturn = customerFromDatabase.Addresses.Select(
        //     a => new AddressDto{Id = a.Id, Street = a.Street, City = a.City}
        // );

        return Ok(addressesToReturn);
    }

    [HttpGet("{addressId}", Name = "GetAddressById")]
    public ActionResult<AddressDto> GetAddressFromCustomer( int customerId, int addressId){
        var customerFromDatabase = _data.Customers.FirstOrDefault(c => c.Id == customerId);
        if(customerFromDatabase == null){return NotFound();}

        var addressFromCustomer = customerFromDatabase.Addresses.FirstOrDefault(a => a.Id == addressId);
        if(addressFromCustomer == null){return NotFound();}

        var addressToReturn = _mapper.Map<AddressDto>(addressFromCustomer);
        // var addressToReturn = new AddressDto{Id = addressFromCustomer.Id, Street = addressFromCustomer.Street, City = addressFromCustomer.City};

        return Ok(addressToReturn);


        //var addressToReturn = _data.Customers.FirstOrDefault(c => c.Id == customerId).Addresses.FirstOrDefault(a => a.Id == addressId);

    }

    [HttpPost]
    public ActionResult<AddressDto> CreatAddress(int customerId, AddressForCreationDto addressForCreationDto){

        var customerFromDatabase = _data.Customers.FirstOrDefault(c => c.Id == customerId);
        if(customerFromDatabase == null){return NotFound();}

        // var addressEntity = new Address{
        //     Id = genUniqueAddrId(),
        //     Street = addressForCreationDto.Street,
        //     City = addressForCreationDto.City
        // };

        // customerFromDatabase.Addresses.Add(addressEntity);

        // var addressToReturn = new AddressDto{
        //     Id = addressEntity.Id,
        //     Street = addressForCreationDto.Street,
        //     City = addressForCreationDto.City
        // };

        var addressEntity = _mapper.Map<Address>(addressForCreationDto);
        addressEntity.Id = genUniqueAddrId();
        customerFromDatabase.Addresses.Add(addressEntity);
        var addressToReturn = _mapper.Map<AddressDto>(addressEntity);

        return CreatedAtRoute(
            "GetAddressById",
            new {customerId = customerFromDatabase.Id, addressId = addressToReturn.Id},
            addressToReturn
        );
    }

    [HttpPut("{addressId}")]
    public ActionResult UpdateCustomer(int customerId, int addressId, AddressForUpdateDto addressForUpdateDto){
        if(addressId != addressForUpdateDto.Id){return BadRequest();}

        var customerFromDatabase = _data.Customers.FirstOrDefault(c => c.Id == customerId);
        if(customerFromDatabase == null){return NotFound();}

        var rightAddressFromCustomer = customerFromDatabase.Addresses.FirstOrDefault(a => a.Id == addressId);
        if(rightAddressFromCustomer == null){return NotFound();}

        // var updatedAddress = new Address{
        //     Id = addressForUpdateDto.Id,
        //     Street = addressForUpdateDto.Street,
        //     City = addressForUpdateDto.City
        // };

        // rightAddressFromCustomer.Street = updatedAddress.Street;
        // rightAddressFromCustomer.City = updatedAddress.City;

        _mapper.Map(addressForUpdateDto, rightAddressFromCustomer);

        return NoContent();
    }

    [HttpDelete("{addressId}")]
    public ActionResult DeleteAddress(int customerId, int addressId){
        var customerFromDatabase = _data.Customers.FirstOrDefault(c => c.Id == customerId);
        if(customerFromDatabase == null){return NotFound();}

        var addressFromCustomer = customerFromDatabase.Addresses.FirstOrDefault(a => a.Id == addressId);
        if(addressFromCustomer == null){return NotFound();}

        customerFromDatabase.Addresses.Remove(addressFromCustomer);
        return NoContent();

    }
}       