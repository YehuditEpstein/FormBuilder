using FormBuilder.Application.Interfaces;
using FormBuilder.Domain.Entities;
using FormBuilder.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FormBuilder.Infrastructure.Repositories;

/// <summary>EF Core implementation of <see cref="IFormTemplateRepository"/>.</summary>
public class FormTemplateRepository : IFormTemplateRepository
{
    private readonly FormBuilderDbContext _dbContext;

    public FormTemplateRepository(FormBuilderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FormTemplate> AddAsync(FormTemplate formTemplate, CancellationToken cancellationToken = default)
    {
        _dbContext.FormTemplates.Add(formTemplate);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return formTemplate;
    }

    public Task<List<FormTemplate>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _dbContext.FormTemplates
            .AsNoTracking()
            .Include(t => t.Fields)
            .Include(t => t.ApprovalSteps)
            .ToListAsync(cancellationToken);

    public Task<FormTemplate?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        _dbContext.FormTemplates
            .AsNoTracking()
            .Include(t => t.Fields)
            .Include(t => t.ApprovalSteps)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
}
