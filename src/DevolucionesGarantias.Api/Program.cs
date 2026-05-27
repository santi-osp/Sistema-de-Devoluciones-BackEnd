using DevolucionesGarantias.Api.Extensions;
using DevolucionesGarantias.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

await app.UseDevelopmentDatabaseSeederAsync();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseApiPipeline();

app.Run();
