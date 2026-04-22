using System.Reflection;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Application.Servicos;
using GoodHamburger.Application.Validadores;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Infrastructure.Repositorios;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuração de Injeção de Dependência
builder.Services.AddSingleton<IPedidoRepositorio, PedidoRepositorio>();
builder.Services.AddSingleton<IProdutoRepositorio, ProdutoRepositorio>();
builder.Services.AddScoped<IPedidoServico, PedidoServico>();
builder.Services.AddScoped<IProdutoServico, ProdutoServico>();
builder.Services.AddScoped<CriarPedidoValidador>();

// Configuração de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Good Hamburger API", 
        Version = "v1",
        Description = "API para gerenciamento de pedidos da lanchonete Good Hamburger."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Good Hamburger API V1");
        c.RoutePrefix = string.Empty; // Define o Swagger como página inicial no desenvolvimento
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
public partial class Program { }
