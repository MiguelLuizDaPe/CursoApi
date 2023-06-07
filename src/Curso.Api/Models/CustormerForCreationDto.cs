using System.ComponentModel.DataAnnotations;

namespace Curso.Api.Models;

public class CustomerForCreationDto : CustomerForManipulationDto{
    //esse ta mexendo com o pai(base) e pegando o cpf dando override
    // public override string Cpf {get => base.Cpf;set => base.Cpf = Cpf;}
}