using System.ComponentModel.DataAnnotations;

namespace Curso.Api.Models;

public class CustomerForUpdateDto: CustomerForManipulationDto{
    [Required(ErrorMessage = "You should fill the id")]
    public int Id {get;set;}
}