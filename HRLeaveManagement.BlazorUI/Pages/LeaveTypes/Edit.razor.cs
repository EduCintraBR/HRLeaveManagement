using HRLeaveManagement.BlazorUI.Contracts;
using HRLeaveManagement.BlazorUI.Models.LeaveTypes;
using Microsoft.AspNetCore.Components;

namespace HRLeaveManagement.BlazorUI.Pages.LeaveTypes
{
    public partial class Edit
    {
        [Inject]
        ILeaveTypeService _client { get; set; }

        [Inject]
        NavigationManager _navManager { get; set; }

        [Parameter]
        public int Id { get; set; }
        public string Message { get; set; }

        LeaveTypeVM leaveType = new LeaveTypeVM();

        protected override async Task OnParametersSetAsync()
        {
            leaveType = await _client.GetLeaveTypeDetails(Id);
        }

        async Task EditLeaveType()
        {
            var response = await _client.UpdateLeaveType(Id, leaveType);

            if (response.Success)
            {
                _navManager.NavigateTo("/leavetypes/");
            }

            Message = response.Message;
        }
    }
}
