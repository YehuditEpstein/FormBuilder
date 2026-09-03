namespace FormBuilder.Application.DTOs;

/// <summary>Full representation of a form template, including its fields and approval route.</summary>
public record FormTemplateDto(
    int Id,
    string Name,
    DateTime CreatedAt,
    string CreatedBy,
    IReadOnlyCollection<FormFieldDto> Fields,
    IReadOnlyCollection<ApprovalStepDto> ApprovalSteps);
