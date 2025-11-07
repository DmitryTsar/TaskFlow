using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TaskFlow.Application.Commands.TaskCommand;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Handlers;
using TaskFlow.Application.Queries.TaskQueries;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Interfaces;
using Xunit;
using TaskStatus = TaskFlow.Domain.Enums.TaskStatus;

namespace TaskFlow.Tests.Handlers
{
    public class TaskHandlersTests
    {
        private readonly Mock<ITaskRepository> _taskRepo = new();
        private readonly Mock<ITaskHistoryRepository> _historyRepo = new();
        private readonly Mock<IProjectRepository> _projectRepo = new();
        private readonly Mock<IUserRepository> _userRepo = new();

        private readonly TaskHandlers _handler;

        public TaskHandlersTests()
        {
            _handler = new TaskHandlers(_taskRepo.Object, _historyRepo.Object, _projectRepo.Object, _userRepo.Object);
        }

        // --- CREATE ---
        [Fact]
        public async Task Handle_CreateTaskCommand_ShouldCreateTask()
        {
            // Arrange
            var command = new CreateTaskCommand
            {
                Title = "Test Task",
                Description = "Desc",
                ProjectId = Guid.NewGuid(),
                CreatedById = Guid.NewGuid(),
                Status = TaskStatus.New,
                Priority = TaskPriority.Medium
            };

            _projectRepo.Setup(x => x.GetByIdAsync(command.ProjectId))
                .ReturnsAsync(new Project { Id = command.ProjectId, Name = "Project A" });
            _userRepo.Setup(x => x.GetByIdAsync(command.CreatedById))
                .ReturnsAsync(new User { Id = command.CreatedById, UserName = "Alice" });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Title.Should().Be("Test Task");
            result.ProjectName.Should().Be("Project A");
            result.CreatedByName.Should().Be("Alice");
            _taskRepo.Verify(r => r.AddAsync(It.IsAny<TaskItem>()), Times.Once);
        }

        // --- UPDATE ---
        [Fact]
        public async Task Handle_UpdateTaskCommand_ShouldUpdateAndCreateHistory()
        {
            // Arrange
            var existing = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Old Title",
                Description = "Old Desc",
                Status = TaskStatus.New,
                ProjectId = Guid.NewGuid(),
                CreatedById = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            _taskRepo.Setup(r => r.GetByIdAsync(existing.Id)).ReturnsAsync(existing);

            var command = new UpdateTaskCommand
            {
                Id = existing.Id,
                Title = "New Title",
                Description = "New Desc",
                Status = TaskStatus.Done,
                UpdatedById = Guid.NewGuid()
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Title.Should().Be("New Title");
            _taskRepo.Verify(r => r.UpdateAsync(It.IsAny<TaskItem>()), Times.Once);
            _historyRepo.Verify(r => r.AddAsync(It.IsAny<TaskHistory>()), Times.AtLeastOnce);
        }

        // --- DELETE ---
        [Fact]
        public async Task Handle_DeleteTaskCommand_ShouldReturnTrue()
        {
            // Arrange
            var id = Guid.NewGuid();
            _taskRepo.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            var command = new DeleteTaskCommand { Id = id };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            _taskRepo.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        // --- GET BY ID ---
        [Fact]
        public async Task Handle_GetTaskByIdQuery_ShouldReturnDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var task = new TaskItem
            {
                Id = id,
                Title = "T1",
                ProjectId = Guid.NewGuid(),
                CreatedById = Guid.NewGuid(),
                Project = new Project { Id = Guid.NewGuid(), Name = "Proj" },
                CreatedBy = new User { Id = Guid.NewGuid(), UserName = "Bob" },
                Status = TaskStatus.InProgress,
                CreatedAt = DateTime.UtcNow
            };

            _taskRepo.Setup(r => r.GetFullTaskAsync(id)).ReturnsAsync(task);

            var query = new GetTaskByIdQuery { Id = id };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.ProjectName.Should().Be("Proj");
            result.CreatedByName.Should().Be("Bob");
        }

        // --- GET BY PROJECT ---
        [Fact]
        public async Task Handle_GetTasksByProject_ShouldReturnList()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var tasks = new List<TaskItem>
            {
                new() { Id = Guid.NewGuid(), Title = "Task1", ProjectId = projectId, CreatedById = Guid.NewGuid() },
                new() { Id = Guid.NewGuid(), Title = "Task2", ProjectId = projectId, CreatedById = Guid.NewGuid() }
            };
            _taskRepo.Setup(r => r.GetByProjectAsync(projectId)).ReturnsAsync(tasks);

            var query = new GetTasksByProjectQuery { ProjectId = projectId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
        }
    }
}
