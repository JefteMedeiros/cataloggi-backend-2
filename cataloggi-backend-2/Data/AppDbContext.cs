using cataloggi_backend_2.Models;
using Microsoft.EntityFrameworkCore;

namespace cataloggi_backend_2.AppDbContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Item> Items => Set<Item>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasIndex(category => category.Name)
            .IsUnique()
            .HasFilter("\"DeletedAt\" IS NULL");

        modelBuilder.Entity<Category>()
            .HasIndex(category => category.Slug)
            .IsUnique()
            .HasFilter("\"DeletedAt\" IS NULL");

        modelBuilder.Entity<Category>()
            .HasQueryFilter(c => c.DeletedAt == null);

        modelBuilder.Entity<Item>()
            .HasQueryFilter(i => i.DeletedAt == null);
    }
}
