using Jukeboxmpa.Data;
using Microsoft.EntityFrameworkCore;

// Program.cs configures services and the HTTP request pipeline.
// - Adds DbContext configured for SQLite using the connection string in appsettings.json.
// - Adds MVC controllers with views and authorization.
// - Configures routing and middleware (static files, HTTPS redirection, authentication/authorization).
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // if authentication is configured
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Song}/{action=Index}/{id?}");

app.Run();
