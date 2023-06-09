using System.Reflection;
using System.Text;
using Curso.Api;
using Curso.Api.Configuration;
using Curso.Api.DbContexts;
using Curso.Api.Extensions;
using Curso.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => {
    options.ListenLocalhost(5000);
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());//injetamo o auto mapper (eu acho, não entendi onde o bagulho ta injetando)
builder.Services.AddSingleton<Data>();//injetamo o Data como singleton (eu acho, não entendi onde o bagulho ta injetando)
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddAuthentication("Bearer").AddJwtBearer( options => {
    options.TokenValidationParameters = new(){
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]!)
            )
        };
    }
);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddDbContext<CustomerContext>(options => options.UseNpgsql("Host=localhost;Database=Curso;Username=postgres;Password=123456"));//pro curso o username é postgres e em casa é miguel

// Add services to the container.
//aqui foi configurado pra transformar em .json(eu acho)
builder.Services.AddControllers(options => options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter())
//coloca essa porra pq o creatCustomer tava retornando 400 e não 422 pq a apicontroller tava resolvendo o problema sozinho ao envés de deixar dar merda
).ConfigureApiBehaviorOptions(setupAction =>
       {
           setupAction.InvalidModelStateResponseFactory = context =>
           {
               // Cria a fábrica de um objeto de detalhes de problema de validação
               var problemDetailsFactory = context.HttpContext.RequestServices
                   .GetRequiredService<ProblemDetailsFactory>();


               // Cria um objeto de detalhes de problema de validação
               var validationProblemDetails = problemDetailsFactory
                   .CreateValidationProblemDetails(
                       context.HttpContext,
                       context.ModelState);


               // Adiciona informações adicionais não adicionadas por padrão
               validationProblemDetails.Detail =
                   "See the errors field for details.";
               validationProblemDetails.Instance =
                   context.HttpContext.Request.Path;


               // Relata respostas do estado de modelo inválido como problemas de validação
               validationProblemDetails.Type =
                   "https://courseunivali.com/modelvalidationproblem";
               validationProblemDetails.Status =
                   StatusCodes.Status422UnprocessableEntity;
               validationProblemDetails.Title =
                   "One or more validation errors occurred.";


               return new UnprocessableEntityObjectResult(
                   validationProblemDetails)
               {
                   ContentTypes = { "application/problem+json" }
               };
           };
       });




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(/*setupAction =>
{
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly.}";
}*/
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();//Mediwers (a ordem desses puto importa)
app.UseAuthorization();

app.MapControllers();

await app.ResetDatabaseAsync();

app.Run();