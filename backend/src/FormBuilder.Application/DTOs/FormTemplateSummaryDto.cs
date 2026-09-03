namespace FormBuilder.Application.DTOs;

/// <summary>Lightweight representation of a form template for list views.</summary>
public record FormTemplateSummaryDto(
    int Id,
    string Name,
    DateTime CreatedAt,
    string CreatedBy,
    int FieldsCount,
    int ApprovalStepsCount);
