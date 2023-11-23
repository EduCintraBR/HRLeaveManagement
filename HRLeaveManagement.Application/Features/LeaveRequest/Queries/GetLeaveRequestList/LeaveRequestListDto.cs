using HRLeaveManagement.Application.Features.LeaveRequest.Shared;
using HRLeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList
{
    public class LeaveRequestListDto : BaseLeaveRequest
    {
        public string RequestingEmployeeId { get; set; }
        public LeaveTypeDto LeaveType { get; set; }
        public DateTime DateRequested { get; set; }
        public bool? Approved { get; set; }
    }
}
