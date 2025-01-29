using FilmkveldAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5010");


// 🔹 Configure database connection (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Enable CORS (Cross-Origin Resource Sharing) - Useful if frontend is separate
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// 🔹 Enable OpenAPI (Swagger) integration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 Build the application
var app = builder.Build();

// 🔹 Use Swagger for API documentation in development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔹 Enable CORS policy
app.UseCors("AllowAll");

// 🔹 Global Exception Handling - Handles unexpected errors
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An unexpected error occurred.");
    });
});

// 🔹 Enforce HTTPS redirection and authorization
//app.UseHttpsRedirection();
app.UseAuthorization();

// Root endpoint tanımı
app.MapGet("/", () => "Filmkveld API is running. Go to /swagger to explore the API.");

// 🔹 Map API controllers
app.MapControllers();

// 🔹 Start the application
app.Run();
