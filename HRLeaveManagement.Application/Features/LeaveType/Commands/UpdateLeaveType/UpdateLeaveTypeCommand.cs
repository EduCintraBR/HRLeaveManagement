using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.Commands.UpdateLeafType
{
    public record UpdateLeaveTypeCommand() : IRequest<Unit>
    {
        public string Name { get; set; } = string.Empty;
        public int DefaultDays { get; set; }
    }
}
