using Microsoft.EntityFrameworkCore;
using ELKOOD.ToDo.Core.Entities;
using ELKOOD.ToDo.Core.Interfaces;
using ELKOOD.ToDo.Infrastructure.Data;
using ELKOOD.ToDo.Core.Common;
using ELKOOD.ToDo.Core.Enums;

namespace ELKOOD.ToDo.Infrastructure.Repositories
{
    public class ToDoRepository : Repository<ToDoItem>, IToDoRepository
    {
        public ToDoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ToDoItem>> GetToDoItemsByUserAsync(int userId)
        {
            return await _dbSet
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ToDoItem>> GetToDoItemsWithFiltersAsync(int? userId, string? category, string? priority, bool? isCompleted)
        {
            var query = _dbSet.AsQueryable();

            if (userId.HasValue)
                query = query.Where(t => t.UserId == userId.Value);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(t => t.Category == category);

            if (!string.IsNullOrEmpty(priority) && Enum.TryParse<PriorityLevel>(priority, out var priorityLevel))
                query = query.Where(t => t.Priority == priorityLevel);

            if (isCompleted.HasValue)
                query = query.Where(t => t.IsCompleted == isCompleted.Value);

            return await query
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ToDoItem>> SearchToDoItemsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            return await _dbSet
                .Where(t => t.Title.Contains(searchTerm) || t.Description.Contains(searchTerm))
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<PagedResult<ToDoItem>> GetPagedToDoItemsAsync(int pageNumber, int pageSize)
        {
            var query = _dbSet.OrderByDescending(t => t.CreatedAt);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ToDoItem>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
