using FormBuilder.Application.DTOs;
using FormBuilder.Application.Exceptions;
using FormBuilder.Application.Interfaces;
using FormBuilder.Application.Mapping;
using FormBuilder.Domain.Entities;

namespace FormBuilder.Application.Services;

/// <summary>
/// Implements the form-template use cases on top of <see cref="IFormTemplateRepository"/>.
/// Holds no persistence or transport concerns — those belong to Infrastructure and Api.
/// </summary>
public class FormTemplateService : IFormTemplateService
{
    private readonly IFormTemplateRepository _repository;

    public FormTemplateService(IFormTemplateRepository repository)
    {
        _repository = repository;
    }

    public async Task<FormTemplateDto> CreateAsync(CreateFormTemplateRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Fields.Count == 0)
        {
            throw new ValidationException("A form template must contain at least one field.");
        }

        if (request.ApprovalSteps.Count == 0)
        {
            throw new ValidationException("A form template must define at least one approval step.");
        }

        var template = new FormTemplate(request.Name, request.CreatedBy);

        foreach (var field in request.Fields)
        {
            template.AddField(field.Label, field.Type, field.IsRequired);
        }

        foreach (var step in request.ApprovalSteps)
        {
            template.AddApprovalStep(step.StepName, step.ApproverIdentity, step.ActionType);
        }

        var saved = await _repository.AddAsync(template, cancellationToken);
        return saved.ToDto();
    }

    public async Task<List<FormTemplateSummaryDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var templates = await _repository.GetAllAsync(cancellationToken);
        return templates
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => t.ToSummaryDto())
            .ToList();
    }

    public async Task<FormTemplateDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var template = await _repository.GetByIdAsync(id, cancellationToken);
        return template?.ToDto();
    }
}
