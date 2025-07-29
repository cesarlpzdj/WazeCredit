using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using WazeCredit.Data;
using WazeCredit.Models;
using WazeCredit.Service;
using WazeCredit.Utilities.DI;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/log-.txt")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.
// Register dependency injection for the WazeForecastSettings and IMarketForecaster
// Configure WazeForecastSettings from the appsettings.json

builder.Services
    .AddAppSettingsConfig(builder.Configuration)
    .AddAllServices()
    .AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services
    .AddDbContext<AppDbContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ))
    .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<CustomMiddleware>();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
