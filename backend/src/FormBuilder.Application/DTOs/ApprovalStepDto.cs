using FormBuilder.Domain.Enums;

namespace FormBuilder.Application.DTOs;

/// <summary>A single approval-route step ("milestone") as returned to API clients.</summary>
public record ApprovalStepDto(
    int Id,
    int StepOrder,
    string StepName,
    string ApproverIdentity,
    ApprovalActionType ActionType);
