using Jukeboxmpa.Data;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity; // <-- NEW USING (for Identity setup)

// Program.cs configures services and the HTTP request pipeline.
// - Adds DbContext configured for SQLite using the connection string in appsettings.json.
// - Adds MVC controllers with views and authorization.
// - Configures routing and middleware (static files, HTTPS redirection, authentication/authorization).
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// *****************************************************************
// ADDED IDENTITY SERVICES
// *****************************************************************
// 1. Configure Identity to use ApplicationDbContext for storage.
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// 2. Add Razor Pages support, which is required for the Identity UI pages (Login, Register).
builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();
// builder.Services.AddAuthorization(); // Optional: Identity setup already includes core authorization

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
app.UseAuthentication(); // IMPORTANT: Must come before UseAuthorization
app.UseAuthorization();
// *****************************************************************

// apply pending EF Core migrations at startup (create scope first)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Mapped Razor Pages (Identity UI) will automatically be available now.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Song}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Log the connection string used by EF Core
    var conn = db.Database.GetDbConnection().ConnectionString;
    Console.WriteLine($"EF Core connection string: {conn}");
}

app.Run();