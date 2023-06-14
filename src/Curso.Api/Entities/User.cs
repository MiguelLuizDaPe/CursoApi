namespace Curso.Api.Entities;

public class User{
    //classe que represnta um user na database
    public int Id {get;set;}
    public string Name {get;set;} = "";
    public string Username {get;set;} = "";
    public string Password {get;set;} = "";
}