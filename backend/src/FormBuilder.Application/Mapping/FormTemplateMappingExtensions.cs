using FormBuilder.Application.DTOs;
using FormBuilder.Domain.Entities;

namespace FormBuilder.Application.Mapping;

/// <summary>Plain, dependency-free mapping between domain entities and DTOs.</summary>
public static class FormTemplateMappingExtensions
{
    public static FormFieldDto ToDto(this FormField field) =>
        new(field.Id, field.Label, field.Type, field.OrderIndex, field.IsRequired);

    public static ApprovalStepDto ToDto(this ApprovalStep step) =>
        new(step.Id, step.StepOrder, step.StepName, step.ApproverIdentity, step.ActionType);

    public static FormTemplateDto ToDto(this FormTemplate template) =>
        new(
            template.Id,
            template.Name,
            template.CreatedAt,
            template.CreatedBy,
            template.Fields.OrderBy(f => f.OrderIndex).Select(f => f.ToDto()).ToList(),
            template.ApprovalSteps.OrderBy(s => s.StepOrder).Select(s => s.ToDto()).ToList());

    public static FormTemplateSummaryDto ToSummaryDto(this FormTemplate template) =>
        new(
            template.Id,
            template.Name,
            template.CreatedAt,
            template.CreatedBy,
            template.Fields.Count,
            template.ApprovalSteps.Count);
}
