using HRLeaveManagement.Application.Models.Identity;

namespace HRLeaveManagement.Application.Contracts.Identity
{
    public interface IUserService
    {
        Task<List<Employee>> GetEmployeesAsync();
        Task<Employee> GetEmployesAsync(string userId);
    }
}
