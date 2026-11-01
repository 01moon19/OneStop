
using ECommerce.UserService.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ECommerce.UserService.Data;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.UserEmail)
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasIndex(u => u.UserEmail)
                .IsUnique();

            entity.Property(u => u.UserRole)
                .HasConversion<int>();
        });
    }
}
