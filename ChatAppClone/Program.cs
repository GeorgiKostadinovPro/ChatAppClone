using Serilog;
using Microsoft.EntityFrameworkCore;

using ChatAppClone.Data;
using ChatAppClone.Data.Models;
using ChatAppClone.Data.Seeding;

using ChatAppClone.Extensions;

using ChatAppClone.Hubs;
using ChatAppClone.Middlewares;
using ChatAppClone.Common.Constants;

var builder = WebApplication.CreateBuilder(args);

// Register Host Logger Serilog to be the main logging provider
string currentDirectory = Environment.CurrentDirectory;
string logFolderPath = Path.Combine(currentDirectory, GeneralConstants.LogsFolder);
Directory.CreateDirectory(logFolderPath);

// configure Serilog Globally
Log.Logger = new LoggerConfiguration()
    .WriteTo.File(Path.Combine(logFolderPath, GeneralConstants.LogFile), rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

// Register Serilog as the app logger
builder.Host.UseSerilog();

// Register DB Context and SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ChatAppCloneDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Register Microsoft Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ChatAppCloneDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddAppServices(builder.Configuration);

// Register SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// Migrate DB => Seed data if empty
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ChatAppCloneDbContext>();
    dbContext.Database.Migrate();

    new ChatAppCloneDbContextSeeder().SeedAsync(dbContext, scope.ServiceProvider).GetAwaiter().GetResult();
}

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error/500");
    app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");
    app.UseHsts();
}

// Middlewares
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GeneralExceptionHandlerMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "api",
    pattern: "api/{controller}/{action}/{id?}");

app.MapRazorPages();

app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<ChatHub>("/chatHub");

app.Run();
