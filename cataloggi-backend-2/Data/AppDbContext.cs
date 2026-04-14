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
}
