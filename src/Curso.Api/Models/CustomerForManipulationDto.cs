using System.ComponentModel.DataAnnotations;
using Univali.Api.ValidationAttributes;

namespace Curso.Api.Models;

public abstract class CustomerForManipulationDto{
    //o [Required] faz com que todos os catributos sejam preenchidos para serem válidos
    [Required(ErrorMessage = "You should fill out the name")]
    [MaxLength(100, ErrorMessage = "The name shouldn't have more than 100 characters")]
    public string Name {get;set;} = "";
    [Required(ErrorMessage = "You have to put the cpf")]
    // o primeiro 11 é o máximo, por isso fica estranho
    // [RegularExpression] da pra meter um regex
    // [StringLength(11, MinimumLength = 11,  ErrorMessage = "The cpf should have 11 numbers")]
    //disponibilizou para dar override por um filho
    [CpfMustBeValid(ErrorMessage = "the {0} must be right")]//provavelmente ta errado
    public string Cpf {get;set;} = "";
    // public virtual string Cpf {get;set;} = "";
}