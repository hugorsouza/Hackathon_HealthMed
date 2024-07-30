using HackathonHealthMed.Application.Interfaces;
using HackathonHealthMed.Application.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

// Configuração de logging
//builder.Logging.ClearProviders(); // Limpa provedores padrão, se necessário
//builder.Logging.AddConsole(); // Adiciona provedor de log para console
//builder.Logging.AddDebug(); // Adiciona provedor de log para o depurador

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>{});
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
