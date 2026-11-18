using Microsoft.EntityFrameworkCore;
using TimeTracker.DAL;
using TimeTracker.BLL.Services;
using TimeTracker.DAL.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<ITrackedAppRepository, TrackedAppRepository>();
builder.Services.AddScoped<IActivitySessionRepository, ActivitySessionRepository>();
builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ITrackedAppService, TrackedAppService>();

var app = builder.Build();
// יצירה או עדכון אוטומטי של מסד הנתונים בהפעלה
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// בדיקת "האם השרת חי"
app.MapGet("/api/health", () => Results.Ok());


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseAuthorization();

app.MapControllers();

app.Run();
