using AutoMapper;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveType.Commands.UpdateLeafType;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType
{
    public class UpdateLeaveTypeCommandHandler : IRequestHandler<UpdateLeaveTypeCommand ,Unit>
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public UpdateLeaveTypeCommandHandler(IMapper mapper, ILeaveTypeRepository leaveTypeRepository)
        {
            this._mapper = mapper;
            this._leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<Unit> Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
        {
            // Convert to domain entity object
            var leaveTypeToUpdate = _mapper.Map<Domain.LeaveType>(request);

            // Update the record in the database
            await _leaveTypeRepository.UpdateAsync(leaveTypeToUpdate);

            // Return unit value
            return Unit.Value;
        }
    }
}
