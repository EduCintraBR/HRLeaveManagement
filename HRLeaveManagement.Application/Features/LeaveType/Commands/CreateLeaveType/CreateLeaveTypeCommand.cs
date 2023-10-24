using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.Commands.CreateLeafType
{
    public record CreateLeaveTypeCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public int DefaultDays { get; set; }
    }
}
