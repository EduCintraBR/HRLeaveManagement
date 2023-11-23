using AutoMapper;
using HRLeaveManagement.Application.Contracts.Email;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Models.Email;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest
{
    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, Unit>
    {
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IAppLogger<CreateLeaveRequestCommandHandler> _appLogger;

        public CreateLeaveRequestCommandHandler(IEmailSender emailSender, IMapper mapper, ILeaveTypeRepository leaveTypeRepository, ILeaveRequestRepository leaveRequestRepository, IAppLogger<CreateLeaveRequestCommandHandler> appLogger)
        {
            this._emailSender = emailSender;
            this._mapper = mapper;
            this._leaveTypeRepository = leaveTypeRepository;
            this._leaveRequestRepository = leaveRequestRepository;
            this._appLogger = appLogger;
        }

        public async Task<Unit> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateLeaveRequestCommandValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.Errors.Any())
                throw new BadRequestException("Invalid Leave Request", validationResult);

            // Get requesting employee`s id

            // Check on employee`s allocation

            // If allocations aren`t enough, return validation error with message

            // Create leave request
            var leaveRequest = _mapper.Map<Domain.LeaveRequest>(request);
            await _leaveRequestRepository.CreateAsync(leaveRequest);

            // Send confirmation email
            try
            {
                var email = new EmailMessage
                {
                    To = string.Empty, /* Get Email from Employee record */
                    Body = $"Your leave request for {request.StartDate:D} to {request.EndDate:D} " +
                           $"has been created successfully.",
                    Subject = "Leave Request Submitted"
                };

                await _emailSender.SendEmailAsync(email);
            }
            catch (Exception ex)
            {
                _appLogger.LogInformation(ex.Message);
            }

            return Unit.Value;
        }
    }
}
