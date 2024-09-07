using Microsoft.EntityFrameworkCore;
using PixelHouse.Models;

namespace PixelHouse.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
    }
}
