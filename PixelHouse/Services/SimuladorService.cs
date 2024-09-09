using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

        // Método executado ao iniciar o serviço
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Inicializa o timer para simular vendas a cada 30 segundos
            _vendaTimer = new Timer(ExecuteVenda, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));

            // Inicializa o timer para simular compras a cada 1 minuto
            _compraTimer = new Timer(ExecuteCompra, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

private async void ExecuteVenda(object? state)
{
    try
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var produtos = await dbContext.Produtos.ToListAsync();
            
            if (produtos.Any())
            {
                // Escolhe um produto aleatório
                var produto = produtos[_random.Next(produtos.Count)];
                
                // Simula uma venda se o estoque for maior que o mínimo
                if (produto.QuantidadeEmEstoque > produto.EstoqueMinimo)
                {
                    // Ajusta a quantidade para um valor maior (por exemplo, até 50 unidades)
                    var quantidade = _random.Next(10, Math.Min(51, produto.QuantidadeEmEstoque + 1));
                    
                    var venda = new Venda
                    {
                        ProdutoId = produto.Id,
                        Quantidade = quantidade,
                        DataSaida = DateTime.Now
                    };
                    
                    dbContext.Vendas.Add(venda);
                    produto.QuantidadeEmEstoque -= venda.Quantidade;

                    // Garante que o estoque não fique negativo
                    if (produto.QuantidadeEmEstoque < 0)
                        produto.QuantidadeEmEstoque = 0;

                    produto.DataSaida = venda.DataSaida; // Atualiza a DataSaida no produto
                }
                
                await dbContext.SaveChangesAsync();
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao executar vendas: {ex.Message}");
    }
}
private async void ExecuteCompra(object? state)
{
    try
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var produtos = await dbContext.Produtos.ToListAsync();

            foreach (var produto in produtos)
            {
                // Verifica se o estoque está abaixo do estoque mínimo ou 10 unidades acima do estoque mínimo
                if (produto.QuantidadeEmEstoque < produto.EstoqueMinimo || 
                    produto.QuantidadeEmEstoque > produto.EstoqueMinimo + 10)
                {
                    var quantidadeParaComprar = _random.Next(5, 21);
                    var compra = new Compra
                    {
                        ProdutoId = produto.Id,
                        Quantidade = quantidadeParaComprar,
                        DataEntrada = DateTime.Now
                    };
                    dbContext.Compras.Add(compra);
                    produto.QuantidadeEmEstoque += compra.Quantidade;
                    produto.DataEntrada = compra.DataEntrada; // Atualiza a DataEntrada no produto
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao executar compras: {ex.Message}");
    }
}



        // Método executado ao parar o serviço
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _vendaTimer?.Change(Timeout.Infinite, 0);
            _compraTimer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        // Método para liberar recursos do timer
        public void Dispose()
        {
            _vendaTimer?.Dispose();
            _compraTimer?.Dispose();
        }
    }
}
