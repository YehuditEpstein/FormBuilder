import { ApprovalActionType } from './approval-action-type';
import { FieldType } from './field-type';

export interface FormFieldDto {
  id: number;
  label: string;
  type: FieldType;
  orderIndex: number;
  isRequired: boolean;
}

export interface ApprovalStepDto {
  id: number;
  stepOrder: number;
  stepName: string;
  approverIdentity: string;
  actionType: ApprovalActionType;
}

/** Full form template, as returned for a single-form read. */
export interface FormTemplateDto {
  id: number;
  name: string;
  createdAt: string;
  createdBy: string;
  fields: FormFieldDto[];
  approvalSteps: ApprovalStepDto[];
}

/** Lightweight form template, as returned for list views. */
export interface FormTemplateSummaryDto {
  id: number;
  name: string;
  createdAt: string;
  createdBy: string;
  fieldsCount: number;
  approvalStepsCount: number;
}

export interface CreateFormFieldRequest {
  label: string;
  type: FieldType;
  isRequired: boolean;
}

export interface CreateApprovalStepRequest {
  stepName: string;
  approverIdentity: string;
  actionType: ApprovalActionType;
}

/** Payload to create a form template in one call: envelope, fields and approval route. */
export interface CreateFormTemplateRequest {
  name: string;
  createdBy: string;
  fields: CreateFormFieldRequest[];
  approvalSteps: CreateApprovalStepRequest[];
}
