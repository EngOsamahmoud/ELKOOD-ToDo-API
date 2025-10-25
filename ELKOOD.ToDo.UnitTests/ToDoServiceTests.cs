using Moq;
using FluentAssertions;
using ELKOOD.ToDo.Core.Entities;
using ELKOOD.ToDo.Core.Interfaces;
using ELKOOD.ToDo.Application.Services;
using ELKOOD.ToDo.Core.Enums;
using Microsoft.Extensions.Logging;

namespace ELKOOD.ToDo.UnitTests.Services
{
    public class ToDoServiceTests
    {
        private readonly Mock<IToDoRepository> _toDoRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ILogger<ToDoService>> _loggerMock;
        private readonly ToDoService _toDoService;

        public ToDoServiceTests()
        {
            _toDoRepositoryMock = new Mock<IToDoRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<ILogger<ToDoService>>();
            _toDoService = new ToDoService(_toDoRepositoryMock.Object, _userRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetToDoItemByIdAsync_WhenItemExists_ReturnsItem()
        {
            // Arrange
            var expectedItem = new ToDoItem { Id = 1, Title = "Test Item" };
            _toDoRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(expectedItem);

            // Act
            var result = await _toDoService.GetToDoItemByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Title.Should().Be("Test Item");
        }

        [Fact]
        public async Task CreateToDoItemAsync_WithValidData_CreatesItem()
        {
            // Arrange
            var user = new User { Id = 1, Username = "testuser" };
            var newItem = new ToDoItem 
            { 
                Title = "New Task", 
                Description = "Description", 
                UserId = 1 
            };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(user);
            _toDoRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<ToDoItem>()))
                .Returns(Task.CompletedTask)
                .Callback<ToDoItem>(item => item.Id = 1);

            // Act
            var result = await _toDoService.CreateToDoItemAsync(newItem);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("New Task");
            result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task MarkAsCompletedAsync_WhenItemExists_MarksAsCompleted()
        {
            // Arrange
            var item = new ToDoItem { Id = 1, Title = "Test", IsCompleted = false };
            _toDoRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(item);

            // Act
            await _toDoService.MarkAsCompletedAsync(1);

            // Assert
            item.IsCompleted.Should().BeTrue();
            _toDoRepositoryMock.Verify(repo => repo.Update(item), Times.Once);
        }
    }
}