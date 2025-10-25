using Microsoft.EntityFrameworkCore;
using ELKOOD.ToDo.Core.Entities;
using ELKOOD.ToDo.Core.Enums;

namespace ELKOOD.ToDo.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure relationships
            modelBuilder.Entity<ToDoItem>()
                .HasOne(t => t.User)
                .WithMany(u => u.ToDoItems)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure enums as strings for better readability
            modelBuilder.Entity<ToDoItem>()
                .Property(t => t.Priority)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            // Seed data for In-Memory database
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Create a temporary instance of PasswordService for seeding
            var passwordService = new Services.PasswordService();
            
            var ownerUser = new User
            {
                Id = 1,
                Username = "owner",
                Email = "owner@elkood.com",
                PasswordHash = passwordService.HashPassword("owner123"),
                Role = UserRole.Owner,
                CreatedAt = DateTime.UtcNow
            };

            var guestUser = new User
            {
                Id = 2,
                Username = "guest",
                Email = "guest@elkood.com", 
                PasswordHash = passwordService.HashPassword("guest123"),
                Role = UserRole.Guest,
                CreatedAt = DateTime.UtcNow
            };

            modelBuilder.Entity<User>().HasData(ownerUser, guestUser);

            modelBuilder.Entity<ToDoItem>().HasData(
                new ToDoItem
                {
                    Id = 1,
                    Title = "Complete ELKOOD Task",
                    Description = "Finish the To-Do API project for ELKOOD",
                    IsCompleted = false,
                    Priority = PriorityLevel.High,
                    Category = "Work",
                    CreatedAt = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow.AddDays(5),
                    UserId = 1
                },
                new ToDoItem
                {
                    Id = 2,
                    Title = "Learn ASP.NET Core",
                    Description = "Study ASP.NET Core fundamentals",
                    IsCompleted = true,
                    Priority = PriorityLevel.Medium,
                    Category = "Learning",
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    DueDate = DateTime.UtcNow.AddDays(10),
                    UserId = 1
                },
                new ToDoItem
                {
                    Id = 3,
                    Title = "Review Documentation",
                    Description = "Read through project requirements",
                    IsCompleted = false,
                    Priority = PriorityLevel.Low,
                    Category = "Work",
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    UserId = 2
                }
            );
        }
    }
}