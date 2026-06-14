using FluentAssertions;
using Moq;
using TaskManagement.Domain.Commands;
using TaskManagement.Domain.Models;
using TaskManagement.Domain.Models.Enums;
using TaskManagement.Service.Services.Abstractions;
using TaskManagement.Service.Services.Implementations;

namespace TaskManagement.Service.Tests
{
    public class DomainTaskServiceTests
    {
        private readonly Mock<IDomainTaskRepository> _repository;
        private readonly Mock<IUserLookupService> _userLookupService;
        public DomainTaskServiceTests()
        {
            _repository = new Mock<IDomainTaskRepository>();
            _userLookupService = new Mock<IUserLookupService>();
        }
        [Fact]
        public async Task UpdateTask()
        {
            const int taskId = 1;

            var domainTask = new DomainTask
            {
                Id = taskId,
                Title = "Test title",
                Description = "Test Description",
                ProjectId = 1,
                Project = new Project("Test Name", "Test Project description", "1"),
                Priority = Priority.Low,
                DeadLine = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)),
                CreatedByUserId = "1"
            };

            var updateTaskCommand = new UpdateTaskCommand
            {
                Id = taskId,
                Title = "Updated title",
                Description = "Updated Description",
                Status = Status.InProgress,
                Priority = Priority.High,
                DeadLine = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10))
            };

            _repository
                .Setup(x => x.GetByIdAsync(taskId))
                .ReturnsAsync(domainTask);

            var service = new DomainTaskService(
                _repository.Object,
                _userLookupService.Object);

            await service.UpdateAsync(updateTaskCommand);

            domainTask.Title.Should().Be(updateTaskCommand.Title);
            domainTask.Description.Should().Be(updateTaskCommand.Description);
            domainTask.Status.Should().Be(updateTaskCommand.Status);
            domainTask.Priority.Should().Be(updateTaskCommand.Priority);
            domainTask.DeadLine.Should().Be(updateTaskCommand.DeadLine);

            _repository.Verify(x => x.GetByIdAsync(taskId), Times.Once);
            _repository.Verify(x => x.UpdateAsync(domainTask), Times.Once);
        }

    }
    }

