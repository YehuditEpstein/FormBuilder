using FormBuilder.Domain.Enums;

namespace FormBuilder.Domain.Entities;

/// <summary>
/// An organizational form template: its envelope (name, creator, creation date),
/// its dynamic list of fields and its dynamic approval route (milestones).
/// Fields/steps are only added through the entity's own methods so the
/// object always keeps its invariants (ordering, at least one required value, etc.).
/// </summary>
public class FormTemplate
{
    private readonly List<FormField> _fields = new();
    private readonly List<ApprovalStep> _approvalSteps = new();

    /// <summary>Required by EF Core for materialization; never used directly by application code.</summary>
    private FormTemplate()
    {
    }

    public FormTemplate(string name, string createdBy)
    {
        SetName(name);

        if (string.IsNullOrWhiteSpace(createdBy))
        {
            throw new ArgumentException("Creator is required.", nameof(createdBy));
        }

        CreatedBy = createdBy.Trim();
        CreatedAt = DateTime.UtcNow;
    }

    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public string CreatedBy { get; private set; } = string.Empty;

    public IReadOnlyCollection<FormField> Fields => _fields.AsReadOnly();

    public IReadOnlyCollection<ApprovalStep> ApprovalSteps => _approvalSteps.AsReadOnly();

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Form name is required.", nameof(name));
        }

        Name = name.Trim();
    }

    /// <summary>Adds a dynamic field to the form; the field's position is derived from insertion order.</summary>
    public FormField AddField(string label, FieldType type, bool isRequired)
    {
        var field = new FormField(label, type, orderIndex: _fields.Count, isRequired);
        _fields.Add(field);
        return field;
    }

    /// <summary>Adds the next step of the approval route; step order is derived from insertion order (1-based).</summary>
    public ApprovalStep AddApprovalStep(string stepName, string approverIdentity, ApprovalActionType actionType)
    {
        var step = new ApprovalStep(stepOrder: _approvalSteps.Count + 1, stepName, approverIdentity, actionType);
        _approvalSteps.Add(step);
        return step;
    }
}
