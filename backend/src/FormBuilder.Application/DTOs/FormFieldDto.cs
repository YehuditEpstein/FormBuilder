using FormBuilder.Domain.Enums;

namespace FormBuilder.Application.DTOs;

/// <summary>A single dynamic field as returned to API clients.</summary>
public record FormFieldDto(
    int Id,
    string Label,
    FieldType Type,
    int OrderIndex,
    bool IsRequired);
