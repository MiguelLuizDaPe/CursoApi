using AutoMapper;
using Curso.Api.Entities;
using Curso.Api.Features.Customers.Queries.GetCustomerByCpf;
using Curso.Api.Features.Customers.Queries.GetCustomerDetail;
using Curso.Api.Features.Customers.Queries.GetCustomers;
using Curso.Api.Features.Customers.Queries.GetCustomerWithAddress;
using Curso.Api.Features.Customers.Queries.GetCustomersWithAddresses;
using Curso.Api.Models;
using Curso.Api.Features.Customers.Commands.UpdateCustomer;
using Curso.Api.Features.Customers.Commands.CreateCustomer;
using Curso.Api.Features.Customers.Commands.CreateCustomerWithAddress;

namespace Curso.Api.Profiles;

public class CustomerProfile : Profile{
    public CustomerProfile(){
        //Primeiro argumento é o objeto de origem e o segundo o objeto de destino
        CreateMap<Entities.Customer, Models.CustomerDto>();
        CreateMap<Models.CustomerForCreationDto, Entities.Customer>();
        CreateMap<Models.CustomerForUpdateDto, Entities.Customer>();
        CreateMap<Models.CustomerForPatchDto, Entities.Customer>();
        CreateMap<Entities.Customer, Models.CustomerForPatchDto>();
        CreateMap<Entities.Customer, Models.CustomerWithAddressesDto>();
        CreateMap<Models.CustomerWithAddressesForCreationDto, Entities.Customer>();
        CreateMap<Models.CustomerWithAddressesForUpdateDto, Entities.Customer>();
        CreateMap<Entities.Customer, Entities.Customer>();

        //novos
        CreateMap<Customer, GetCustomerDetailDto>();
        CreateMap<Customer, GetCustomerByCpfDto>();
        CreateMap<Customer, GetCustomerWithAddressDto>();
        CreateMap<Customer, GetCustomersWithAddressesDto>();
        CreateMap<Customer, GetCustomersDto>();
        CreateMap<CreateCustomerCommand, Customer>();
        CreateMap<Customer, CreateCustomerDto>();
        CreateMap<UpdateCustomerCommand, CustomerForUpdateDto>();
        CreateMap<CustomerForUpdateDto, UpdateCustomerDto>();

        CreateMap<CreateCustomerWithAddressCommand, Customer>();
        CreateMap<Customer, CreateCustomerWithAddressDto>();
    }
}