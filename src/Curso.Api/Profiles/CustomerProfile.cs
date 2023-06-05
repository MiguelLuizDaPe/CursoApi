using AutoMapper;

namespace Curso.Api.Profiles;

public class CustomerProfile : Profile{
    public CustomerProfile(){
        //Primeiro argumento é o objeto de origem e o segundo o objeto de origem
        CreateMap<Entities.Customer, Models.CustomerDto>();
    }
}