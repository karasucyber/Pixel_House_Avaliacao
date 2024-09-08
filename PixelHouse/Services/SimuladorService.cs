using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PixelHouse.Data;
using PixelHouse.Models;

namespace PixelHouse.Services
{
    public class SimuladorService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer? _vendaTimer;
        private Timer? _compraTimer;
        private readonly Random _random = new Random();

        public SimuladorService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Inicializa o timer para simular vendas a cada 30 segundos
            _vendaTimer = new Timer(ExecuteVenda, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            // Inicializa o timer para simular compras a cada 1 minuto
            _compraTimer = new Timer(ExecuteCompra, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

            return Task.CompletedTask;
        }

        private async void ExecuteVenda(object? state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Simular vendas para todos os produtos
                var produtos = await dbContext.Produtos.ToListAsync();
                foreach (var produto in produtos)
                {
                    if (produto.QuantidadeEmEstoque > produto.EstoqueMinimo)
                    {
                        // Gera um valor aleatório para a quantidade de venda entre 1 e 10
                        var quantidade = _random.Next(1, 11);

                        var venda = new Venda
                        {
                            ProdutoId = produto.Id,
                            Quantidade = quantidade,
                            DataSaida = DateTime.Now
                        };

                        dbContext.Vendas.Add(venda);
                        produto.QuantidadeEmEstoque -= venda.Quantidade;
                    }
                }

                await dbContext.SaveChangesAsync();
            }
        }

        private async void ExecuteCompra(object? state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Simular compras para todos os produtos
                var produtos = await dbContext.Produtos.ToListAsync();
                foreach (var produto in produtos)
                {
                    if (produto.QuantidadeEmEstoque < produto.EstoqueMinimo)
                    {
                        var quantidadeParaComprar = (produto.EstoqueMinimo - produto.QuantidadeEmEstoque) * 2;
                        
                        var compra = new Compra
                        {
                            ProdutoId = produto.Id,
                            Quantidade = quantidadeParaComprar,
                            DataEntrada = DateTime.Now
                        };

                        dbContext.Compras.Add(compra);
                        produto.QuantidadeEmEstoque += compra.Quantidade;
                    }
                }

                await dbContext.SaveChangesAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _vendaTimer?.Change(Timeout.Infinite, 0);
            _compraTimer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _vendaTimer?.Dispose();
            _compraTimer?.Dispose();
        }
    }
}
