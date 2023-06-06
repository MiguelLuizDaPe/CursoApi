using AutoMapper;

namespace Curso.Api.Profiles;

public class AddressProfile : Profile{
    public AddressProfile(){
        CreateMap<Entities.Address, Models.AddressDto>();
        CreateMap<Models.AddressForCreationDto, Entities.Address>();
        CreateMap<Models.AddressForUpdateDto, Entities.Address>();
    }
}