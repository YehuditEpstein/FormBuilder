using System.ComponentModel.DataAnnotations;
using FormBuilder.Domain.Enums;

namespace FormBuilder.Application.DTOs;

/// <summary>Payload for a single approval-route step when creating a new form template.</summary>
public class CreateApprovalStepRequest
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(200)]
    public string StepName { get; init; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [MaxLength(200)]
    public string ApproverIdentity { get; init; } = string.Empty;

    public ApprovalActionType ActionType { get; init; }
}
