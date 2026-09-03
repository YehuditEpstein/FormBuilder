import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  CreateFormTemplateRequest,
  FormTemplateDto,
  FormTemplateSummaryDto,
} from '../models/form-template.model';

/**
 * Thin HTTP client for the FormTemplates API. Kept free of any UI/state concerns
 * so it can be reused (and mocked) independently of the components that use it.
 */
@Injectable({ providedIn: 'root' })
export class FormTemplatesService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/form-templates`;

  /** Saves a new form template in its entirety (fields + full approval route). */
  create(request: CreateFormTemplateRequest): Observable<FormTemplateDto> {
    return this.http.post<FormTemplateDto>(this.baseUrl, request);
  }

  /** Lightweight list of all existing form templates. */
  getAll(): Observable<FormTemplateSummaryDto[]> {
    return this.http.get<FormTemplateSummaryDto[]>(this.baseUrl);
  }

  /** Full detail of a single form template. */
  getById(id: number): Observable<FormTemplateDto> {
    return this.http.get<FormTemplateDto>(`${this.baseUrl}/${id}`);
  }
}
