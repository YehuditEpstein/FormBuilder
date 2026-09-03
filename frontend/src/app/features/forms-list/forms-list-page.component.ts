import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormTemplatesService } from '../../core/services/form-templates.service';
import { FormTemplateSummaryDto } from '../../core/models/form-template.model';

@Component({
  selector: 'app-forms-list-page',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './forms-list-page.component.html',
  styleUrl: './forms-list-page.component.css',
})
export class FormsListPageComponent implements OnInit {
  private readonly formTemplatesService = inject(FormTemplatesService);

  protected readonly forms = signal<FormTemplateSummaryDto[]>([]);
  protected readonly isLoading = signal(true);
  protected readonly errorMessage = signal<string | null>(null);

  ngOnInit(): void {
    this.load();
  }

  private load(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.formTemplatesService.getAll().subscribe({
      next: (forms) => {
        this.forms.set(forms);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error(err);
        this.errorMessage.set('טעינת רשימת הטפסים נכשלה.');
        this.isLoading.set(false);
      },
    });
  }
}
