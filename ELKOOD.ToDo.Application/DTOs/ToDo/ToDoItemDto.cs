using ELKOOD.ToDo.Core.Enums;

namespace ELKOOD.ToDo.Application.DTOs.ToDo
{
    public class ToDoItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public PriorityLevel Priority { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public int UserId { get; set; }
    }

    public class CreateToDoItemDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PriorityLevel Priority { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public int UserId { get; set; }
    }

    public class UpdateToDoItemDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PriorityLevel Priority { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
    }
}