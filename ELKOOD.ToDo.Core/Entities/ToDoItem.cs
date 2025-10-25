using ELKOOD.ToDo.Core.Enums;

namespace ELKOOD.ToDo.Core.Entities
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public PriorityLevel Priority { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        
        // Foreign key
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;
    }
}