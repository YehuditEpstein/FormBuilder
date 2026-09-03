using FormBuilder.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FormBuilder.Infrastructure.Persistence;

public class FormBuilderDbContext : DbContext
{
    public FormBuilderDbContext(DbContextOptions<FormBuilderDbContext> options)
        : base(options)
    {
    }

    public DbSet<FormTemplate> FormTemplates => Set<FormTemplate>();

    public DbSet<FormField> FormFields => Set<FormField>();

    public DbSet<ApprovalStep> ApprovalSteps => Set<ApprovalStep>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FormBuilderDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
