using DotNetInterview.API;
using DotNetInterview.API.Controllers;
using DotNetInterview.API.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers()
    .AddApplicationPart(typeof(ItemsController).Assembly)
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Shop Inventory API", 
        Version = "v1",
        Description = "API for managing shop inventory with dynamic pricing",
        Contact = new OpenApiContact { Name = "API Support", Email = "support@shopapi.com" }
    });
    
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddScoped<PricingService>();

// SQLite Configuration
var connection = new SqliteConnection("Data Source=ShopInventory;Mode=Memory;Cache=Shared");
connection.Open();
builder.Services.AddDbContext<DataContext>(options => 
    options.UseSqlite(connection)
           .EnableSensitiveDataLogging()
           .EnableDetailedErrors());

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop Inventory v1");
        c.RoutePrefix = "swagger";
        c.ConfigObject.AdditionalItems["tryItOutEnabled"] = true;
    });
}

app.MapControllers();

// Database initialization
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try 
    {
        var context = services.GetRequiredService<DataContext>();
        context.Database.EnsureCreated();
        SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Database initialization failed");
    }
}

app.Run();