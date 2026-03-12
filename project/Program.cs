using System.Security.Claims;
using MediaLibraryWebApp.Data;
using MediaLibraryWebApp.Models;
using MediaLibraryWebApp.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add MVC services
builder.Services.AddControllersWithViews();

// Add Authentication / Authorization
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.Cookie.Name = "MediaLibraryAuth";
    });

builder.Services.AddAuthorization(options =>
{
    // Require authentication by default
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Add Database (MySQL)
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    );
});

// Register repositories
builder.Services.AddScoped<IMediaRepository, MediaRepository>();
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<IPlaybackRepository, PlaybackRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
// Favorites repository (for the "Favorites" view and toggle)
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();

var app = builder.Build();

// Ensure database is created and seeded with a default user
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Users.Any())
    {
        db.Users.Add(new User
        {
            Username = "admin",
            Email = "admin@example.com",
        });
        db.SaveChanges();
    }
}

// Error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Static files (wwwroot)
app.UseStaticFiles();

app.UseRouting();

// Enable authentication before authorization so the user can be identified.
app.UseAuthentication();
app.UseAuthorization();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Run application
app.Run();