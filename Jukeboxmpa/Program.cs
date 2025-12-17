using Jukeboxmpa.Data;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;

// Program.cs configures services and the HTTP request pipeline.
// - Adds DbContext configured for SQLite using the connection string in appsettings.json.
// - Adds MVC controllers with views and authorization.
// - Configures routing and middleware (static files, HTTPS redirection, authentication/authorization).
var builder = WebApplication.CreateBuilder(args);

//Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // the time before play list dissappears
    options.Cookie.HttpOnly = true; // make the cookie accessible only via HTTP
    options.Cookie.IsEssential = true; // make the cookie essential for the application to function
});

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity to use ApplicationDbContext for storage.
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add Razor Pages support, which is required for the Identity UI pages (Login, Register).
builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();


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
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

// apply pending EF Core migrations at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Mapped Razor Pages will automatically be available now.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Song}/{action=Index}/{id?}");

app.MapRazorPages(); // Map Razor Pages for Identity

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Log the connection string used by EF Core
    var conn = db.Database.GetDbConnection().ConnectionString;
    Console.WriteLine($"EF Core connection string: {conn}");
}

app.Run();