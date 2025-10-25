using ELKOOD.ToDo.Core.Entities;
using ELKOOD.ToDo.Core.Common;

namespace ELKOOD.ToDo.Application.Interfaces
{
    public interface IToDoService
    {
        Task<ToDoItem?> GetToDoItemByIdAsync(int id);
        Task<IEnumerable<ToDoItem>> GetAllToDoItemsAsync();
        Task<IEnumerable<ToDoItem>> GetToDoItemsByUserAsync(int userId);
        Task<ToDoItem> CreateToDoItemAsync(ToDoItem toDoItem);
        Task UpdateToDoItemAsync(ToDoItem toDoItem);
        Task DeleteToDoItemAsync(int id);
        Task MarkAsCompletedAsync(int id);
        Task<IEnumerable<ToDoItem>> GetFilteredToDoItemsAsync(int? userId, string? category, string? priority, bool? isCompleted);
        Task<IEnumerable<ToDoItem>> SearchToDoItemsAsync(string searchTerm);
        Task<PagedResult<ToDoItem>> GetPagedToDoItemsAsync(int pageNumber, int pageSize);
    }
}   