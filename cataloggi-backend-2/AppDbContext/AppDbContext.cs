using Microsoft.EntityFrameworkCore;

namespace cataloggi_backend_2.AppDbContext;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}