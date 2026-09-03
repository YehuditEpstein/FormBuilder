import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import {
  FormArray,
  FormControl,
  FormGroup,
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { FormTemplatesService } from '../../core/services/form-templates.service';
import { ApprovalActionType, APPROVAL_ACTION_LABELS } from '../../core/models/approval-action-type';
import { FieldType, FIELD_TYPE_LABELS } from '../../core/models/field-type';
import { CreateFormTemplateRequest } from '../../core/models/form-template.model';

type FieldGroup = FormGroup<{
  label: FormControl<string>;
  type: FormControl<FieldType>;
  isRequired: FormControl<boolean>;
}>;

type ApprovalStepGroup = FormGroup<{
  stepName: FormControl<string>;
  approverIdentity: FormControl<string>;
  actionType: FormControl<ApprovalActionType>;
}>;

/**
 * "Create new form" screen: form envelope + dynamic form builder + dynamic
 * approval-route builder, all backed by one typed Reactive Form.
 */
@Component({
  selector: 'app-form-builder-page',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './form-builder-page.component.html',
  styleUrl: './form-builder-page.component.css',
})
export class FormBuilderPageComponent {
  private readonly fb = inject(NonNullableFormBuilder);
  private readonly formTemplatesService = inject(FormTemplatesService);
  private readonly router = inject(Router);

  protected readonly fieldTypeLabels = FIELD_TYPE_LABELS;
  protected readonly approvalActionOptions = Object.values(ApprovalActionType);
  protected readonly approvalActionLabels = APPROVAL_ACTION_LABELS;

  // Exposed individually so the template's "add field" buttons can bind to them directly.
  protected readonly fieldTypeText = FieldType.Text;
  protected readonly fieldTypeDate = FieldType.Date;
  protected readonly fieldTypeNumber = FieldType.Number;
  protected readonly fieldTypeCheckbox = FieldType.Checkbox;

  protected readonly isSubmitting = signal(false);
  protected readonly errorMessage = signal<string | null>(null);

  protected readonly form = this.fb.group({
    name: this.fb.control('', [Validators.required, Validators.maxLength(200)]),
    createdBy: this.fb.control('hr-admin', [Validators.required, Validators.maxLength(200)]),
    fields: this.fb.array<FieldGroup>([]),
    approvalSteps: this.fb.array<ApprovalStepGroup>([]),
  });

  protected get fields(): FormArray<FieldGroup> {
    return this.form.controls.fields;
  }

  protected get approvalSteps(): FormArray<ApprovalStepGroup> {
    return this.form.controls.approvalSteps;
  }

  protected addField(type: FieldType): void {
    const group: FieldGroup = this.fb.group({
      label: this.fb.control('', [Validators.required, Validators.maxLength(200)]),
      type: this.fb.control(type),
      isRequired: this.fb.control(false),
    });
    this.fields.push(group);
  }

  protected removeField(index: number): void {
    this.fields.removeAt(index);
  }

  protected addApprovalStep(): void {
    const group: ApprovalStepGroup = this.fb.group({
      stepName: this.fb.control('', [Validators.required, Validators.maxLength(200)]),
      approverIdentity: this.fb.control('', [Validators.required, Validators.maxLength(200)]),
      actionType: this.fb.control(ApprovalActionType.ApproveOrReject),
    });
    this.approvalSteps.push(group);
  }

  protected removeApprovalStep(index: number): void {
    this.approvalSteps.removeAt(index);
  }

  protected submit(): void {
    this.errorMessage.set(null);

    if (this.fields.length === 0) {
      this.errorMessage.set('יש להוסיף לפחות שדה אחד לטופס.');
      return;
    }

    if (this.approvalSteps.length === 0) {
      this.errorMessage.set('יש להוסיף לפחות שלב אישור אחד למסלול.');
      return;
    }

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.errorMessage.set('יש להשלים את כל השדות המסומנים כחובה.');
      return;
    }

    const value = this.form.getRawValue();
    const request: CreateFormTemplateRequest = {
      name: value.name,
      createdBy: value.createdBy,
      fields: value.fields.map((f) => ({ label: f.label, type: f.type, isRequired: f.isRequired })),
      approvalSteps: value.approvalSteps.map((s) => ({
        stepName: s.stepName,
        approverIdentity: s.approverIdentity,
        actionType: s.actionType,
      })),
    };

    this.isSubmitting.set(true);

    this.formTemplatesService.create(request).subscribe({
      next: (created) => {
        this.isSubmitting.set(false);
        this.router.navigate(['/forms', created.id]);
      },
      error: (err) => {
        console.error(err);
        this.isSubmitting.set(false);
        this.errorMessage.set('שמירת הטופס נכשלה. נסו שוב.');
      },
    });
  }
}
