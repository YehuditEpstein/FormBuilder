namespace FormBuilder.Domain.Enums;

/// <summary>
/// The action an approver is allowed to take at a given step of an approval route.
/// </summary>
public enum ApprovalActionType
{
    Approve = 0,
    Reject = 1,
    ApproveOrReject = 2,
    ViewOnly = 3
}
