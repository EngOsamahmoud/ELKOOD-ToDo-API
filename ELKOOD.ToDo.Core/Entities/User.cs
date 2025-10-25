using ELKOOD.ToDo.Core.Enums;

namespace ELKOOD.ToDo.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<ToDoItem> ToDoItems { get; set; } = new List<ToDoItem>();
    }
}