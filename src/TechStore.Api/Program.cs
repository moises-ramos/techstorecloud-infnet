using Microsoft.EntityFrameworkCore;
using TechStore.Application.Interfaces;
using TechStore.Application.Services;
using TechStore.Domain.Interfaces;
using TechStore.Infrastructure.Data;
using TechStore.Infrastructure.Repositories;

// ---------------------------------------------------------
// Builder — Configuração de Serviços
// ---------------------------------------------------------

var builder = WebApplication.CreateBuilder(args);

// Logging estruturado
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Application Insights (Observabilidade)
if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
{
    builder.Services.AddApplicationInsightsTelemetry();
}

// Entity Framework Core — Azure SQL Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TechStoreDb"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        }));

// Dependency Injection — Repository Pattern
builder.Services.AddScoped<IRepositorioDeProduto, RepositorioDeProduto>();

// Dependency Injection — Service Layer
builder.Services.AddScoped<IServicoDeProduto, ServicoDeProduto>();

// Controllers com validação automática
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "TechStore API",
        Version = "v1",
        Description = "API de Cadastro de Produtos — TechStore Cloud MVP"
    });
});

// CORS — permite comunicação do Azure Static Web Apps
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>("sqlserver");

// ---------------------------------------------------------
// App — Pipeline de Middleware
// ---------------------------------------------------------

var app = builder.Build();

// Swagger disponível em todos os ambientes para o MVP
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "TechStore API v1");
    options.RoutePrefix = "swagger";
});

// CORS
app.UseCors("AllowFrontend");

// HTTPS Redirection
app.UseHttpsRedirection();

// Autorização (preparado para expansão futura)
app.UseAuthorization();

// Mapear Controllers
app.MapControllers();

// Health Check endpoint
app.MapHealthChecks("/health");

// ---------------------------------------------------------
// Aplicar Migrations automaticamente (apenas para MVP)
// ---------------------------------------------------------

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
        logger.LogInformation("Migrations aplicadas com sucesso ao Azure SQL Database.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro ao aplicar migrations no Azure SQL Database.");
    }
}

// ---------------------------------------------------------
// Iniciar a API
// ---------------------------------------------------------

app.Run();
