using FormBuilder.Application.DTOs;

namespace FormBuilder.Application.Interfaces;

/// <summary>Application-level use cases for form templates, exposed to the API layer.</summary>
public interface IFormTemplateService
{
    /// <summary>Creates a new form template together with its fields and its full approval route.</summary>
    Task<FormTemplateDto> CreateAsync(CreateFormTemplateRequest request, CancellationToken cancellationToken = default);

    /// <summary>Returns a lightweight summary of every existing form template.</summary>
    Task<List<FormTemplateSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Returns the full detail of one form template, or null if it does not exist.</summary>
    Task<FormTemplateDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
