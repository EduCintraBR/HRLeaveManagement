using HRLeaveManagement.Application.Features.LeaveRequest.Commands.CancelLeaveRequest;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands.ChangeLeaveRequestApproval;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands.CreateLeaveRequest;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands.DeleteLeaveRequest;
using HRLeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;
using HRLeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetail;
using HRLeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestList;
using HRLeaveManagement.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRLeaveManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveRequestsController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<LeaveRequestListDto>>> Get(bool isLoggedInUser = false)
        {
            var leaveRequests = _mediator.Send(new GetLeaveRequestListQuery());
            return Ok(leaveRequests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveRequestDetailsDto>> Get(int id)
        {
            var leaveRequestDetail = _mediator.Send(new GetLeaveRequestDetailQuery { Id = id });
            return Ok(leaveRequestDetail);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Post([FromBody] CreateLeaveRequestCommand createLeaveRequest)
        {
            var response = await _mediator.Send(createLeaveRequest);
            return CreatedAtAction(nameof(Get), new { id = response });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Put([FromBody] UpdateLeaveRequestCommand updateLeaveRequest)
        {
            await _mediator.Send(updateLeaveRequest);
            return NoContent();
        }

        [HttpPut("CancelRequest")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> CancelRequest(CancelLeaveRequestCommand cancelLeaveRequest)
        {
            await _mediator.Send(cancelLeaveRequest);
            return NoContent();
        }

        [HttpPut("UpdateApproval")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UpdateApproval(ChangeLeaveRequestApprovalCommand updateApprovalRequest)
        {
            await _mediator.Send(updateApprovalRequest);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteLeaveRequestCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
