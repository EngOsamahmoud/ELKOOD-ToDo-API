using ELKOOD.ToDo.Infrastructure.Data;
using ELKOOD.ToDo.Infrastructure;
using ELKOOD.ToDo.Application;
using ELKOOD.ToDo.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add infrastructure and application layers
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

// Initialize In-Memory database with seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        // For In-Memory database, we just need to ensure it's created
        context.Database.EnsureCreated();

        // Seed data is automatically inserted via OnModelCreating
        // You can verify by counting records
        var userCount = context.Users.Count();
        var todoCount = context.ToDoItems.Count();

        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation($"In-Memory database seeded with {userCount} users and {todoCount} todo items.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing the In-Memory database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();