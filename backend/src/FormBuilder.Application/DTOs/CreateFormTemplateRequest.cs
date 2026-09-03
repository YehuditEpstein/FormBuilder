using System.ComponentModel.DataAnnotations;

namespace FormBuilder.Application.DTOs;

/// <summary>
/// Payload to create a new form template in one call: its envelope, its dynamic
/// fields and its full dynamic approval route (milestones).
/// </summary>
public class CreateFormTemplateRequest
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(200)]
    public string Name { get; init; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [MaxLength(200)]
    public string CreatedBy { get; init; } = string.Empty;

    /// <summary>Fields in display order. May be empty, though a form with no fields is of little use.</summary>
    public List<CreateFormFieldRequest> Fields { get; init; } = new();

    /// <summary>Approval steps in route order (first item is step 1, and so on).</summary>
    public List<CreateApprovalStepRequest> ApprovalSteps { get; init; } = new();
}
