using FormBuilder.Application.Interfaces;
using FormBuilder.Infrastructure.Persistence;
using FormBuilder.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FormBuilder.Infrastructure;

/// <summary>Composition entry point for everything the Infrastructure layer provides.</summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default") ?? "Data Source=formbuilder.db";

        services.AddDbContext<FormBuilderDbContext>(options => options.UseSqlite(connectionString));

        services.AddScoped<IFormTemplateRepository, FormTemplateRepository>();

        return services;
    }
}
