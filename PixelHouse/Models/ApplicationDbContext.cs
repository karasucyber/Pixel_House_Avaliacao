using Microsoft.EntityFrameworkCore;
using PixelHouse.Models; 

namespace PixelHouse.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Construtor que recebe as opções de contexto e passa para a classe base
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet para a entidade Produto. Isso representa uma tabela no banco de dados.
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Venda> Vendas { get; set; }
    }
}
