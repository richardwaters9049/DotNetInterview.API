using DotNetInterview.API;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container ==============================================

// Controllers and API Explorer
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger/OpenAPI documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "DotNetInterview.API", 
        Version = "1.0",
        Description = "Interview Management API",
        Contact = new OpenApiContact { Name = "Richy", Email = "richardwaters866@gmail.com" }
    });
    
    // Include XML comments for enhanced documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Configure Database ========================================================
var connection = new SqliteConnection("Data Source=DotNetInterview;Mode=Memory;Cache=Shared");
connection.Open();

builder.Services.AddDbContext<DataContext>(options => 
    options.UseSqlite(connection)
           .EnableSensitiveDataLogging() // Only for development
           .EnableDetailedErrors());     // Only for development

var app = builder.Build();

// Configure the HTTP request pipeline =======================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNetInterview.API v1");
        c.RoutePrefix = "swagger";
        c.DisplayRequestDuration(); // Show request timing in UI
    });
}

app.MapControllers();

// Database Initialization ==================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try 
    {
        var context = services.GetRequiredService<DataContext>();
        
        // Ensure database is created and migrated
        context.Database.EnsureCreated();
        
        // Seed initial test data
        SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing the database.");
    }
}

app.Run();