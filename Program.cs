using Microsoft.EntityFrameworkCore;
using CourseManagement.Data;
using CourseManagement.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CourseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CourseDbContext") ?? throw new InvalidOperationException("Connection string 'CourseDbContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

//automapper config
builder.Services.AddAutoMapper(typeof(Program));

//DI configuration
builder.Services.AddTransient<ICoursesService, CoursesService>();
builder.Services.AddTransient<IDAndIService, DAndIService>();

//File Storage
builder.Services.AddTransient<IStorageService, FileStorageService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Courses}/{action=Index}/{id?}");

app.Run();
