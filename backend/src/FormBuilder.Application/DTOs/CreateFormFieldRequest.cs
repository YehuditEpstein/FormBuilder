using System.ComponentModel.DataAnnotations;
using FormBuilder.Domain.Enums;

namespace FormBuilder.Application.DTOs;

/// <summary>Payload for a single field when creating a new form template.</summary>
public class CreateFormFieldRequest
{
    [Required(AllowEmptyStrings = false)]
    [MaxLength(200)]
    public string Label { get; init; } = string.Empty;

    public FieldType Type { get; init; }

    public bool IsRequired { get; init; }
}
