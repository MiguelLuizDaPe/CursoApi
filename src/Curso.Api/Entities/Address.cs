using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Curso.Api.Entities;
public class Address{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get; set;}

    [Required]
    [MaxLength(50)]
    public string Street {get; set;} = "";

    [Required]
    [MaxLength(50)]
    public string City {get; set;} = "";

    [ForeignKey("CustomerId")]
    //public Customer? Customer {get;set;}//colocar somente quando necessário pos pode induzir erros no código
    public int CustomerId {get;set;}
}