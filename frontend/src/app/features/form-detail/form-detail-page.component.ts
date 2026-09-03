import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormTemplatesService } from '../../core/services/form-templates.service';
import { FormTemplateDto } from '../../core/models/form-template.model';
import { FieldType, FIELD_TYPE_LABELS } from '../../core/models/field-type';
import { ApprovalActionType, APPROVAL_ACTION_LABELS } from '../../core/models/approval-action-type';

@Component({
  selector: 'app-form-detail-page',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './form-detail-page.component.html',
  styleUrl: './form-detail-page.component.css',
})
export class FormDetailPageComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly formTemplatesService = inject(FormTemplatesService);

  protected readonly form = signal<FormTemplateDto | null>(null);
  protected readonly isLoading = signal(true);
  protected readonly errorMessage = signal<string | null>(null);

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (!Number.isInteger(id)) {
      this.errorMessage.set('מזהה טופס לא תקין.');
      this.isLoading.set(false);
      return;
    }

    this.formTemplatesService.getById(id).subscribe({
      next: (form) => {
        this.form.set(form);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error(err);
        this.errorMessage.set('הטופס לא נמצא.');
        this.isLoading.set(false);
      },
    });
  }

  protected fieldTypeLabel(type: FieldType): string {
    return FIELD_TYPE_LABELS[type];
  }

  protected approvalActionLabel(actionType: ApprovalActionType): string {
    return APPROVAL_ACTION_LABELS[actionType];
  }
}
