/**
 * Actions an approver may take at a step of a form's approval route.
 * Values must match the backend's ApprovalActionType enum names exactly (serialized as strings).
 */
export enum ApprovalActionType {
  Approve = 'Approve',
  Reject = 'Reject',
  ApproveOrReject = 'ApproveOrReject',
  ViewOnly = 'ViewOnly',
}

export const APPROVAL_ACTION_LABELS: Record<ApprovalActionType, string> = {
  [ApprovalActionType.Approve]: 'אישור בלבד',
  [ApprovalActionType.Reject]: 'דחייה בלבד',
  [ApprovalActionType.ApproveOrReject]: 'אישור או דחייה',
  [ApprovalActionType.ViewOnly]: 'צפייה בלבד',
};
