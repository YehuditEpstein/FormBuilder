/**
 * Kinds of dynamic fields a form can contain.
 * Values must match the backend's FieldType enum names exactly (serialized as strings).
 */
export enum FieldType {
  Text = 'Text',
  TextArea = 'TextArea',
  Number = 'Number',
  Date = 'Date',
  Checkbox = 'Checkbox',
  Dropdown = 'Dropdown',
}

export const FIELD_TYPE_LABELS: Record<FieldType, string> = {
  [FieldType.Text]: 'טקסט',
  [FieldType.TextArea]: 'טקסט חופשי',
  [FieldType.Number]: 'מספר',
  [FieldType.Date]: 'תאריך',
  [FieldType.Checkbox]: 'תיבת סימון',
  [FieldType.Dropdown]: 'רשימה נפתחת',
};
