using Curso.Api.Entities;
using Curso.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Curso.Api.Controllers;

[ApiController]
[Route("api/customers/{customerId}/addresses")]
public class AddressController : ControllerBase{

    [HttpGet] //copiar essa porra de alguém depois também
    public ActionResult<IEnumerable<AddressDto>> GetAdresses(int customerId){
        var customerFromDatabase = Data.getInstance().Customers.FirstOrDefault(
            customer => customer.Id == customerId
        );

        if (customerFromDatabase == null){ return NotFound();}

        var addressesToReturn = customerFromDatabase.Addresses.Select(
            a => new AddressDto{Id = a.Id, Street = a.Street, City = a.City}
        );

        return Ok(addressesToReturn);
    }

    [HttpGet("{addressId}", Name = "GetAddressById")]
    public ActionResult<AddressDto> GetAddress( int customerId, int addressId){
        var customerFromDatabase = Data.getInstance().Customers.FirstOrDefault(c => c.Id == customerId);
        if(customerFromDatabase == null){return NotFound();}

        var addressFromCustomer = customerFromDatabase.Addresses.FirstOrDefault(a => a.Id == addressId);
        if(addressFromCustomer == null){return NotFound();}

        var addressToReturn = new AddressDto{Id = addressFromCustomer.Id, Street = addressFromCustomer.Street, City = addressFromCustomer.City};

        return Ok(addressToReturn);


        //var addressToReturn = Data.getInstance().Customers.FirstOrDefault(c => c.Id == customerId).Addresses.FirstOrDefault(a => a.Id == addressId);

    }

    [HttpPost]
    public ActionResult<AddressDto> CreatAddress(int customerId, AddressForCreationDto addressForCreationDto){

        var customerFromDatabase = Data.getInstance().Customers.FirstOrDefault(c => c.Id == customerId);
        if(customerFromDatabase == null){return NotFound();}

        var genUniqueAddrId = () => {
            int addrIdMax = 0;

            foreach(Customer c in Data.getInstance().Customers){
                int idMax = c.Addresses.Max(n => n.Id);
                if(idMax > addrIdMax){addrIdMax = idMax;}
            }
            return addrIdMax + 1;
        };

        var addressEntity = new Address{
            Id = genUniqueAddrId(),
            Street = addressForCreationDto.Street,
            City = addressForCreationDto.City
        };

        customerFromDatabase.Addresses.Add(addressEntity);

        var addressToReturn = new AddressDto{
            Id = addressEntity.Id,
            Street = addressForCreationDto.Street,
            City = addressForCreationDto.City
        };

        // return NoContent();

        return CreatedAtRoute(
            "GetAddressById",
            new {customerId, addressId = addressToReturn.Id},
            addressToReturn
        );
    }

    [HttpPut("{addressId}")]
    public ActionResult UpdateCustomer(int customerId, int addressId, AddressForUpdateDto addressForUpdateDto){
        if(addressId != addressForUpdateDto.Id){return BadRequest();}

        var customerFromDatabase = Data.getInstance().Customers.FirstOrDefault(c => c.Id == customerId);
        if(customerFromDatabase == null){return NotFound();}

        var rightAddressFromCustomer = customerFromDatabase.Addresses.FirstOrDefault(a => a.Id == addressId);
        if(rightAddressFromCustomer == null){return NotFound();}

        var updatedAddress = new Address{
            Id = addressForUpdateDto.Id,
            Street = addressForUpdateDto.Street,
            City = addressForUpdateDto.City
        };

        rightAddressFromCustomer.Street = updatedAddress.Street;
        rightAddressFromCustomer.City = updatedAddress.City;

        return NoContent();
    }

    [HttpDelete("{addressId}")]
    public ActionResult DeleteAddress(int customerId, int addressId){
        var customerFromDatabase = Data.getInstance().Customers.FirstOrDefault(c => c.Id == customerId);
        if(customerFromDatabase == null){return NotFound();}

        var addressFromCustomer = customerFromDatabase.Addresses.FirstOrDefault(a => a.Id == addressId);
        if(addressFromCustomer == null){return NotFound();}

        customerFromDatabase.Addresses.Remove(addressFromCustomer);
        return NoContent();

    }
}       