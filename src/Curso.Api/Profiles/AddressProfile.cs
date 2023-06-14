using AutoMapper;
using Curso.Api.Entities;
using Curso.Api.Features.Addresses.Command.CreateAddressOfCustomer;
using Curso.Api.Features.Addresses.Command.RemoveAddressOfCustomer;
using Curso.Api.Features.Addresses.Queries.GetAddressesOfCustomer;
using Curso.Api.Features.Addresses.Queries.GetAddressOfCustomer;

namespace Curso.Api.Profiles;

public class AddressProfile : Profile{
    public AddressProfile(){
        CreateMap<Entities.Address, Models.AddressDto>();
        CreateMap<Models.AddressForCreationDto, Entities.Address>();
        CreateMap<Models.AddressForUpdateDto, Entities.Address>();

        //novo
        CreateMap<Address, GetAddressOfCustomerDto>();
        CreateMap<Address, GetAddressesOfCustomerDto>();
        CreateMap<Address, CreateAddressOfCustomerDto>();
        CreateMap<CreateAddressOfCustomerCommand, Address>();
        CreateMap<Address, RemoveAddressOfCustomerDto>();
        CreateMap<RemoveAddressOfCustomerCommand, Address>();
    }
}