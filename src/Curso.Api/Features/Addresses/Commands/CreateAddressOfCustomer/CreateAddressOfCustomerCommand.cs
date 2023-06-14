using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Curso.Api.Features.Addresses.Command.CreateAddressOfCustomer;

public class CreateAddressOfCustomerCommand : IRequest<CreateAddressOfCustomerDto>{
    [Required(ErrorMessage = "You should fill out a Street")]
    [MaxLength(100, ErrorMessage = "The Street shouldn't have more than 100 characters")]
    public string Street {get; set;} = string.Empty;

    [Required(ErrorMessage = "You should fill out a City")]
    [MaxLength(100, ErrorMessage = "The City shouldn't have more than 100 characters")]
    public string City {get; set;} = string.Empty;

    public int CustomerId {get; set;}

}