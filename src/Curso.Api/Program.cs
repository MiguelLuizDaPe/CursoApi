using Curso.Api;
using Curso.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options => {
    options.ListenLocalhost(5000);
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());//injetamo o auto mapper (eu acho, não entendi onde o bagulho ta injetando)
builder.Services.AddSingleton<Data>();//injetamo o Data como singleton (eu acho, não entendi onde o bagulho ta injetando)

// Add services to the container.
//aqui foi configurado pra transformar em .json(eu acho)
builder.Services.AddControllers(options => options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter())
//coloca essa porra pq o creatCustomer tava retornando 400 e não 422 pq a apicontroller tava resolvendo o problema sozinho ao envés de deixar dar merda
).ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
