using System.Text.Json.Serialization;
using FormBuilder.Api.Middleware;
using FormBuilder.Application.Interfaces;
using FormBuilder.Application.Services;
using FormBuilder.Infrastructure;
using FormBuilder.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

const string AngularClientCorsPolicy = "AngularClient";

// ----- Services (composition root) -----------------------------------------------------

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        // Serialize enums as their names (e.g. "Date") instead of raw ints, for a readable API contract.
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(AngularClientCorsPolicy, policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Application layer: use-case services.
builder.Services.AddScoped<IFormTemplateService, FormTemplateService>();

// Infrastructure layer: EF Core + repositories. Uses SQLite so the data is modeled
// exactly like a relational database while keeping setup to a single file.
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// ----- Database bootstrap ---------------------------------------------------------------

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FormBuilderDbContext>();
    dbContext.Database.EnsureCreated();
}

// ----- HTTP pipeline ---------------------------------------------------------------------

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(AngularClientCorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();

// Exposed for WebApplicationFactory-based integration tests.
public partial class Program
{
}
