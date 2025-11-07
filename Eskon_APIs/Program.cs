using Eskon_APIs;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDependencies(builder.Configuration);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine("---");
Console.WriteLine("--- [DEBUG] Verifying Connection String ---");
Console.WriteLine($"--- > {connectionString}");
Console.WriteLine("---");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

#region MyRegion
// ===== PASTE THIS CODE BLOCK BEFORE app.Run() =====

// This block will automatically apply pending EF Core migrations on startup
try
{
    // Create a new dependency injection scope to get the DbContext
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>(); // <-- IMPORTANT: Replace 'YourDbContext' with your actual DbContext class name

        // Apply pending migrations. This will also create the database if it doesn't exist.
        await dbContext.Database.MigrateAsync();
    }
}
catch (Exception ex)
{
    // Log the error if migrations fail
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while migrating the database.");
    // Optionally, you might want to stop the application if migrations fail
    // For now, we'll just log it.
}

// =======================================================
#endregion

app.Run();
