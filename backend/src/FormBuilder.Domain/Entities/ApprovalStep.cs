using FormBuilder.Domain.Enums;

namespace FormBuilder.Domain.Entities;

/// <summary>
/// A single step ("milestone") of a form's dynamic approval route.
/// </summary>
public class ApprovalStep
{
    private ApprovalStep()
    {
    }

    internal ApprovalStep(int stepOrder, string stepName, string approverIdentity, ApprovalActionType actionType)
    {
        if (string.IsNullOrWhiteSpace(stepName))
        {
            throw new ArgumentException("Step name is required.", nameof(stepName));
        }

        if (string.IsNullOrWhiteSpace(approverIdentity))
        {
            throw new ArgumentException("Approver identity is required.", nameof(approverIdentity));
        }

        StepOrder = stepOrder;
        StepName = stepName.Trim();
        ApproverIdentity = approverIdentity.Trim();
        ActionType = actionType;
    }

    public int Id { get; private set; }

    public int FormTemplateId { get; private set; }

    public FormTemplate? FormTemplate { get; private set; }

    /// <summary>1-based position of this step along the approval route.</summary>
    public int StepOrder { get; private set; }

    public string StepName { get; private set; } = string.Empty;

    /// <summary>Identity of the approver for this step (name, email or role — kept as free text for the PoC).</summary>
    public string ApproverIdentity { get; private set; } = string.Empty;

    public ApprovalActionType ActionType { get; private set; }
}
