using HackathonHealthMed.Application.Interfaces;
using HackathonHealthMed.Application.Service;
using HackathonHealthMed.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<IHorarioDisponivelRepository, HorarioDisponivelService>();

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
