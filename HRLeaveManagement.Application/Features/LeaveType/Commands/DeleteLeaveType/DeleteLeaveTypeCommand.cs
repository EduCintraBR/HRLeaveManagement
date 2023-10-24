using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.Commands.DeleteLeafType
{
    public record DeleteLeaveTypeCommand() : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
