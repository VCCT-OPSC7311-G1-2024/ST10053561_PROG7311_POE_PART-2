using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PROG_POE_2.Areas.Identity.Data;
using PROG_POE_2.Models;

namespace PROG_POE_2.Data;

public class Login_RegContext : IdentityDbContext<ApplicationUser>
{
    // Adding DB context for Farmers
    public DbSet<Farmer> Farmers { get; set; }

    // Adding DB context for Products
    public DbSet<Product> Products { get; set; }

    public Login_RegContext(DbContextOptions<Login_RegContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
