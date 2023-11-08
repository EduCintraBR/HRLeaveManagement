using AutoMapper;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveType.Commands.DeleteLeafType;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.Commands.DeleteLeaveType
{
    public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand, Unit>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public DeleteLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository)
        {
            this._leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<Unit> Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            var leaveTypeToDelete = await _leaveTypeRepository.GetByIdAsync(request.Id);

            if(leaveTypeToDelete == null) 
                throw new NotFoundException(nameof(LeaveType), request.Id);

            if (leaveTypeToDelete != null) 
            {
                await _leaveTypeRepository.DeleteAsync(leaveTypeToDelete);
            }

            return Unit.Value;
        }
    }
}
