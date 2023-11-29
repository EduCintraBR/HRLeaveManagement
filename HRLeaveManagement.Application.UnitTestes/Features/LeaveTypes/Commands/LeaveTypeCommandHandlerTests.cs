using AutoMapper;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Contracts.Persistence;
using HRLeaveManagement.Application.Exceptions;
using HRLeaveManagement.Application.Features.LeaveType.Commands.CreateLeafType;
using HRLeaveManagement.Application.Features.LeaveType.Commands.UpdateLeafType;
using HRLeaveManagement.Application.Features.LeaveType.Commands.UpdateLeaveType;
using HRLeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HRLeaveManagement.Application.MappingProfiles;
using HRLeaveManagement.Application.UnitTestes.Mocks;
using HRLeaveManagement.Domain;
using MediatR;
using Moq;
using Shouldly;

namespace HRLeaveManagement.Application.UnitTestes.Features.LeaveTypes.Commands
{
    public class LeaveTypeCommandHandlerTests
    {
        private readonly Mock<ILeaveTypeRepository> _mockRepo;
        private IMapper _mapper;
        private Mock<IAppLogger<UpdateLeaveTypeCommandHandler>> _mockAppLogger;

        public LeaveTypeCommandHandlerTests()
        {
            _mockRepo = MockLeaveTypeRepository.GetMockLeaveTypeRepository();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<LeaveTypeProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            _mockAppLogger = new Mock<IAppLogger<UpdateLeaveTypeCommandHandler>>();
        }

        [Fact]
        public async Task CreateLeaveTypeAndReturnIdCreated()
        {
            //Arrange
            var leaveTypeToCreate = new CreateLeaveTypeCommand
            {
                Name = "Holiday test",
                DefaultDays = 1
            };
            var handler = new CreateLeaveTypeCommandHandler(_mapper, _mockRepo.Object);

            //Act
            var result = await handler.Handle(leaveTypeToCreate, CancellationToken.None);

            //Assert
            result.ShouldBeGreaterThan(0);
            result.ShouldBeOfType<int>();
        }

        [Fact]
        public async Task ValidRequestUpdatesLeaveType()
        {
            //Arrange
            var leaveTypeToUpdate = new UpdateLeaveTypeCommand
            {
                Id = 3,
                DefaultDays = 15,
                Name = "Test Update Success"
            };
            var handler = new UpdateLeaveTypeCommandHandler(_mapper, _mockRepo.Object, _mockAppLogger.Object);

            //Act
            var result = await handler.Handle(leaveTypeToUpdate, CancellationToken.None);

            //Assert
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task InvalidRequestUpdatesLeaveType()
        {
            //Arrange
            var leaveTypeToUpdate = new UpdateLeaveTypeCommand
            {
                Id = 3,
                DefaultDays = 0,
                Name = "Test Update Fail"
            };
            var handler = new UpdateLeaveTypeCommandHandler(_mapper, _mockRepo.Object, _mockAppLogger.Object);

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(async () => await handler.Handle(leaveTypeToUpdate, CancellationToken.None));
        }
    }
}
