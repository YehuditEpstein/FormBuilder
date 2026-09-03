using FormBuilder.Domain.Enums;

namespace FormBuilder.Domain.Entities;

/// <summary>
/// A single dynamic field that belongs to a <see cref="FormTemplate"/>.
/// </summary>
public class FormField
{
    private FormField()
    {
    }

    internal FormField(string label, FieldType type, int orderIndex, bool isRequired)
    {
        if (string.IsNullOrWhiteSpace(label))
        {
            throw new ArgumentException("Field label is required.", nameof(label));
        }

        Label = label.Trim();
        Type = type;
        OrderIndex = orderIndex;
        IsRequired = isRequired;
    }

    public int Id { get; private set; }

    public int FormTemplateId { get; private set; }

    public FormTemplate? FormTemplate { get; private set; }

    public string Label { get; private set; } = string.Empty;

    public FieldType Type { get; private set; }

    public int OrderIndex { get; private set; }

    public bool IsRequired { get; private set; }
}
