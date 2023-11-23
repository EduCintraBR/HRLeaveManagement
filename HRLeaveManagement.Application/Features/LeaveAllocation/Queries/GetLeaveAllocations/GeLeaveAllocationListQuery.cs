using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveAllocation.Queries.GetLeaveAllocations
{
    public class GeLeaveAllocationListQuery : IRequest<List<LeaveAllocationDto>>
    {

    }
}
