using FormBuilder.Domain.Entities;

namespace FormBuilder.Application.Interfaces;

/// <summary>
/// Persistence abstraction for <see cref="FormTemplate"/> aggregates. The Application
/// layer depends only on this interface — Infrastructure provides the EF Core implementation.
/// </summary>
public interface IFormTemplateRepository
{
    Task<FormTemplate> AddAsync(FormTemplate formTemplate, CancellationToken cancellationToken = default);

    Task<List<FormTemplate>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<FormTemplate?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
