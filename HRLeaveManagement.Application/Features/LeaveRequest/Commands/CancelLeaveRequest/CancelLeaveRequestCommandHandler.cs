using HRLeaveManagement.Application.Contracts.Email;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Models.Email;
using MediatR;

namespace HRLeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest
{
    public class CancelLeaveRequestCommandHandler : IRequestHandler<CancelLeaveRequestCommand, Unit>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IEmailSender _emailSender;
        private readonly IAppLogger<CancelLeaveRequestCommandHandler> _appLogger;

        public CancelLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository, IEmailSender emailSender, IAppLogger<CancelLeaveRequestCommandHandler> appLogger)
        {
            this._leaveRequestRepository = leaveRequestRepository;
            this._emailSender = emailSender;
            this._appLogger = appLogger;
        }

        public async Task<Unit> Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(request.Id);

            if (leaveRequest is null)
                throw new NotFoundException(nameof(leaveRequest), request.Id);

            leaveRequest.Cancelled = true;

            // If already approve, re-evaluate the employee`s allocations for the leave type

            // Send Confirmation Email
            try
            {
                var email = new EmailMessage
                {
                    To = string.Empty, /* Get Email from Employee record */
                    Body = $"Your leave request for {leaveRequest.StartDate:D} to {leaveRequest.EndDate:D} " +
                           $"has been cancelled successfully.",
                    Subject = "Leave Request Cancelled"
                };

                await _emailSender.SendEmailAsync(email);
            }
            catch (Exception ex)
            {
                _appLogger.LogWarning(ex.Message);
            }
        }
    }
}
