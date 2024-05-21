using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PROG_POE_2.Areas.Identity.Data;
using PROG_POE_2.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Login_RegContextConnection") ?? throw new InvalidOperationException("Connection string 'Login_RegContextConnection' not found.");

builder.Services.AddDbContext<Login_RegContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false ).AddRoles<IdentityRole>().AddEntityFrameworkStores<Login_RegContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// To add Login and Register Razor Pages
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = true;
});

builder.Services.AddSession(options =>
{
    // Set a longer timeout for testing.
    options.IdleTimeout = TimeSpan.FromMinutes(60); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

// Use the session middleware
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// The MapRazorPages method is used to map Razor pages to the application request pipeline.
app.MapRazorPages();


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Employee", "Farmer" };
    foreach (var roleName in roleNames)
    {
        var roleExist = roleManager.RoleExistsAsync(roleName).Result;
        if (!roleExist)
        {
            roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
        }
    }
}

app.Run();
