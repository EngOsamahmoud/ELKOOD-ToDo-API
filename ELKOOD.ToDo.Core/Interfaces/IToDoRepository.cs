using ELKOOD.ToDo.Core.Entities;
using ELKOOD.ToDo.Core.Common;

namespace ELKOOD.ToDo.Core.Interfaces
{
    public interface IToDoRepository : IRepository<ToDoItem>
    {
        Task<IEnumerable<ToDoItem>> GetToDoItemsByUserAsync(int userId);
        Task<IEnumerable<ToDoItem>> GetToDoItemsWithFiltersAsync(int? userId, string? category, string? priority, bool? isCompleted);
        Task<IEnumerable<ToDoItem>> SearchToDoItemsAsync(string searchTerm);
        Task<PagedResult<ToDoItem>> GetPagedToDoItemsAsync(int pageNumber, int pageSize);
    }
}