using Core.Service;
using Service;
using Core.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==============================================================================
// 1. INJEÇÕES DE DEPENDÊNCIA (Arquitetura de Referência - Regra 5)
// ==============================================================================

// Configuração do Banco de Dados
builder.Services.AddDbContext<CondosmartContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Ligando a Interface ao Serviço (A mágica acontece aqui)
builder.Services.AddScoped<IPagamentoService, PagamentoService>();

// ==============================================================================

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ==============================================================================
// 2. SEGURANÇA
// A Autenticação 
// ==============================================================================
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();