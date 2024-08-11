using InvestmentPortfolioAPI.Data;
using InvestmentPortfolioAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using InvestmentPortfolioAPI.Models;
using Hangfire;
using Hangfire.SqlServer;
using System.Reflection;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Habilitar geração de arquivo de documentação XML
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Investment Portfolio API",
        Description = "API para gerenciamento de portfólio de investimentos"
    });

    // Caminho para os arquivos XML de documentação
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Configurar o Entity Framework Core com SQL Server
builder.Services.AddDbContext<InvestmentContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar Hangfire para usar o SQL Server
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

builder.Services.AddHangfireServer();

// Configurar injeção de dependência para serviços
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<SmtpEmailService>(); // Alterado para Scoped
builder.Services.AddScoped<InvestmentProductService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<AdministratorService>();
builder.Services.AddScoped<CustomerService>();

// Configurar serviços adicionais
builder.Services.AddControllers();

// Configurar o Swagger
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configurar o pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Investment Portfolio API V1");
        c.RoutePrefix = string.Empty; // Faz o Swagger UI ser carregado na URL raiz
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Ativa o painel do Hangfire para monitoramento de jobs
app.UseHangfireDashboard();

// Agendar a tarefa diária após a configuração do Hangfire
RecurringJob.AddOrUpdate<InvestmentProductService>(
    "send-daily-expiration-notifications",
    service => service.NotifyExpiringProductsAsync(),
    Cron.Daily(21, 0));

app.Run();
