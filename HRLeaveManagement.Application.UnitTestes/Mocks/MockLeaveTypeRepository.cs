using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Features.LeaveType.Commands.UpdateLeafType;
using HRLeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType;
using HRLeaveManagement.Domain;
using Moq;

namespace HRLeaveManagement.Application.UnitTestes.Mocks
{
    public class MockLeaveTypeRepository
    {
        public static Mock<ILeaveTypeRepository> GetMockLeaveTypeRepository()
        {
            var leaveTypes = new List<LeaveType>
            {
                new LeaveType
                {
                    Id = 1,
                    DefaultDays = 10,
                    Name = "Test Vacation"
                },
                new LeaveType
                {
                    Id = 2,
                    DefaultDays = 15,
                    Name = "Test Sick"
                },
                new LeaveType
                {
                    Id = 3,
                    DefaultDays = 15,
                    Name = "Test Maternity"
                }
            };

            var mockRepo = new Mock<ILeaveTypeRepository>();

            mockRepo.Setup(s => s.GetAllAsync()).ReturnsAsync(leaveTypes);
            mockRepo.Setup(s => s.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    var item = leaveTypes.FirstOrDefault(t => t.Id == id);
                    if (item is null)
                        throw new KeyNotFoundException($"No item found with id {id}.");

                    return item;
                });

            mockRepo.Setup(s => s.CreateAsync(It.IsAny<LeaveType>()))
                .Callback((LeaveType leaveType) =>
                {
                    var newId = leaveTypes.Max(l => l.Id) + 1;
                    leaveType.Id = newId;
                    leaveTypes.Add(leaveType);
                })
                .Returns(Task.CompletedTask);

            mockRepo.Setup(s => s.UpdateAsync(It.IsAny<LeaveType>()))
                .Callback((LeaveType leaveType) =>
                {
                    if (leaveType is null)
                        throw new ArgumentNullException(nameof(leaveType));

                    var item = leaveTypes.FirstOrDefault(p => p.Id == leaveType.Id);
                    if (item != null)
                    {
                        item.DefaultDays = leaveType.DefaultDays;
                        item.Name = leaveType.Name;
                    }
                    else
                        throw new InvalidOperationException("Item not found.");
                })
                .Returns(Task.CompletedTask);


            mockRepo.Setup(s => s.DeleteAsync(It.IsAny<LeaveType>()))
                .Callback((LeaveType leaveType) =>
                {
                    if (leaveType is null)
                        throw new ArgumentNullException(nameof(leaveType));

                    leaveTypes.Remove(leaveType);
                })
                .Returns(Task.CompletedTask);

            return mockRepo;
        }
    }
}
