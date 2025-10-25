using ELKOOD.ToDo.Core.Entities;
using ELKOOD.ToDo.Core.Interfaces;
using ELKOOD.ToDo.Application.Interfaces;
// using ELKOOD.ToDo.Application.Common;
using ELKOOD.ToDo.Core.Common;
using Microsoft.Extensions.Logging;

namespace ELKOOD.ToDo.Application.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ToDoService> _logger;

        public ToDoService(IToDoRepository toDoRepository, IUserRepository userRepository, ILogger<ToDoService> logger)
        {
            _toDoRepository = toDoRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<ToDoItem?> GetToDoItemByIdAsync(int id)
        {
            _logger.LogInformation("Getting ToDo item with ID: {Id}", id);
            return await _toDoRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<ToDoItem>> GetAllToDoItemsAsync()
        {
            _logger.LogInformation("Getting all ToDo items");
            return await _toDoRepository.GetAllAsync();
        }

        public async Task<IEnumerable<ToDoItem>> GetToDoItemsByUserAsync(int userId)
        {
            _logger.LogInformation("Getting ToDo items for user ID: {UserId}", userId);
            return await _toDoRepository.GetToDoItemsByUserAsync(userId);
        }

        public async Task<ToDoItem> CreateToDoItemAsync(ToDoItem toDoItem)
        {
            var user = await _userRepository.GetByIdAsync(toDoItem.UserId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            toDoItem.CreatedAt = DateTime.UtcNow;
            
            _logger.LogInformation("Creating new ToDo item for user ID: {UserId}", toDoItem.UserId);
            await _toDoRepository.AddAsync(toDoItem);
            
            return toDoItem;
        }

        public async Task UpdateToDoItemAsync(ToDoItem toDoItem)
        {
            var existingItem = await _toDoRepository.GetByIdAsync(toDoItem.Id);
            if (existingItem == null)
            {
                throw new ArgumentException("ToDo item not found");
            }

            existingItem.Title = toDoItem.Title;
            existingItem.Description = toDoItem.Description;
            existingItem.Priority = toDoItem.Priority;
            existingItem.Category = toDoItem.Category;
            existingItem.DueDate = toDoItem.DueDate;

            _logger.LogInformation("Updating ToDo item with ID: {Id}", toDoItem.Id);
            _toDoRepository.Update(existingItem);
        }

        public async Task DeleteToDoItemAsync(int id)
        {
            var item = await _toDoRepository.GetByIdAsync(id);
            if (item == null)
            {
                throw new ArgumentException("ToDo item not found");
            }

            _logger.LogInformation("Deleting ToDo item with ID: {Id}", id);
            _toDoRepository.Remove(item);
        }

        public async Task MarkAsCompletedAsync(int id)
        {
            var item = await _toDoRepository.GetByIdAsync(id);
            if (item == null)
            {
                throw new ArgumentException("ToDo item not found");
            }

            item.IsCompleted = true;
            _logger.LogInformation("Marking ToDo item with ID: {Id} as completed", id);
            _toDoRepository.Update(item);
        }

        public async Task<IEnumerable<ToDoItem>> GetFilteredToDoItemsAsync(int? userId, string? category, string? priority, bool? isCompleted)
        {
            _logger.LogInformation("Filtering ToDo items - User: {UserId}, Category: {Category}, Priority: {Priority}, Completed: {IsCompleted}", 
                userId, category, priority, isCompleted);
            
            return await _toDoRepository.GetToDoItemsWithFiltersAsync(userId, category, priority, isCompleted);
        }

        public async Task<IEnumerable<ToDoItem>> SearchToDoItemsAsync(string searchTerm)
        {
            _logger.LogInformation("Searching ToDo items with term: {SearchTerm}", searchTerm);
            return await _toDoRepository.SearchToDoItemsAsync(searchTerm);
        }

        public async Task<PagedResult<ToDoItem>> GetPagedToDoItemsAsync(int pageNumber, int pageSize)
        {
            _logger.LogInformation("Getting paged ToDo items - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);
            return await _toDoRepository.GetPagedToDoItemsAsync(pageNumber, pageSize);
        }
    }
}