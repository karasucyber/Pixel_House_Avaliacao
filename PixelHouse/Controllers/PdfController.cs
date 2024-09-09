using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using PixelHouse.Data;
using PixelHouse.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public class PdfController : Controller
{
    private readonly ApplicationDbContext _context;

    public PdfController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult ProdutosEmEstoque()
    {
        var produtos = _context.Produtos.ToList();

        var document = Document.Create(container =>
        {
            container
                .Page(page =>
                {
                    page.Margin(50);
                    page.Content().Column(column =>
                    {
                        column.Item().AlignCenter().Text("Produtos em Estoque")
                            .Bold()
                            .FontSize(18);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50); // ID
                                columns.RelativeColumn(200); // Nome
                                columns.ConstantColumn(100); // Quantidade
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("ID").Bold();
                                header.Cell().Text("Nome").Bold();
                                header.Cell().Text("Quantidade em Estoque").Bold();
                            });

                            foreach (var produto in produtos)
                            {
                                table.Cell().Text(produto.Id.ToString());
                                table.Cell().Text(produto.Nome);
                                table.Cell().Text(produto.Descricao ?? string.Empty);
                                table.Cell().Text(produto.QuantidadeEmEstoque.ToString());
                            }
                        });
                    });
                });
        });

        var stream = new MemoryStream();
        document.GeneratePdf(stream);
        stream.Position = 0;

        return File(stream.ToArray(), "application/pdf", "ProdutosEmEstoque.pdf");
    }

    public async Task<IActionResult> ProdutosMaisVendidos()
    {
        var vendas = await _context.Vendas
            .GroupBy(v => v.ProdutoId)
            .Select(g => new
            {
                ProdutoId = g.Key,
                TotalVendido = g.Sum(v => v.Quantidade)
            })
            .OrderByDescending(p => p.TotalVendido)
            .ToListAsync();

        var produtos = _context.Produtos.ToList();
        var produtosMaisVendidos = vendas
            .Join(produtos, v => v.ProdutoId, p => p.Id, (v, p) => new
            {
                p.Id,
                p.Nome,
                p.Descricao,
                TotalVendido = v.TotalVendido
            })
            .ToList();

        var document = Document.Create(container =>
        {
            container
                .Page(page =>
                {
                    page.Margin(50);
                    page.Content().Column(column =>
                    {
                        column.Item().AlignCenter().Text("Produtos Mais Vendidos")
                            .Bold()
                            .FontSize(18);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50); // ID
                                columns.RelativeColumn(200); // Nome
                                columns.ConstantColumn(100); // Total Vendido
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("ID").Bold();
                                header.Cell().Text("Nome").Bold();
                                header.Cell().Text("Total Vendido").Bold();
                            });

                            foreach (var produto in produtosMaisVendidos)
                            {
                                table.Cell().Text(produto.Id.ToString());
                                table.Cell().Text(produto.Nome);
                                table.Cell().Text(produto.Descricao ?? string.Empty);
                                table.Cell().Text(produto.TotalVendido.ToString());
                            }
                        });
                    });
                });
        });

        var stream = new MemoryStream();
        document.GeneratePdf(stream);
        stream.Position = 0;

        return File(stream.ToArray(), "application/pdf", "ProdutosMaisVendidos.pdf");
    }

    public async Task<IActionResult> ProdutosMaisComprados()
    {
        var compras = await _context.Compras
            .GroupBy(c => c.ProdutoId)
            .Select(g => new
            {
                ProdutoId = g.Key,
                TotalComprado = g.Sum(c => c.Quantidade)
            })
            .OrderByDescending(p => p.TotalComprado)
            .ToListAsync();

        var produtos = _context.Produtos.ToList();
        var produtosMaisComprados = compras
            .Join(produtos, c => c.ProdutoId, p => p.Id, (c, p) => new
            {
                p.Id,
                p.Nome,
                p.Descricao,
                TotalComprado = c.TotalComprado
            })
            .ToList();

        var document = Document.Create(container =>
        {
            container
                .Page(page =>
                {
                    page.Margin(50);
                    page.Content().Column(column =>
                    {
                        column.Item().AlignCenter().Text("Produtos Mais Comprados")
                            .Bold()
                            .FontSize(18);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50); // ID
                                columns.RelativeColumn(200); // Nome
                                columns.ConstantColumn(100); // Total Comprado
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("ID").Bold();
                                header.Cell().Text("Nome").Bold();
                                header.Cell().Text("Total Comprado").Bold();
                            });

                            foreach (var produto in produtosMaisComprados)
                            {
                                table.Cell().Text(produto.Id.ToString());
                                table.Cell().Text(produto.Nome);
                                table.Cell().Text(produto.Descricao ?? string.Empty);
                                table.Cell().Text(produto.TotalComprado.ToString());
                            }
                        });
                    });
                });
        });

        var stream = new MemoryStream();
        document.GeneratePdf(stream);
        stream.Position = 0;

        return File(stream.ToArray(), "application/pdf", "ProdutosMaisComprados.pdf");
    }

    public async Task<IActionResult> ProdutosComEstoqueBaixo()
    {
        var produtos = await _context.Produtos
            .Where(p => p.QuantidadeEmEstoque < p.EstoqueMinimo)
            .ToListAsync();

        var document = Document.Create(container =>
        {
            container
                .Page(page =>
                {
                    page.Margin(50);
                    page.Content().Column(column =>
                    {
                        column.Item().AlignCenter().Text("Produtos com Estoque Baixo")
                            .Bold()
                            .FontSize(18);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50); // ID
                                columns.RelativeColumn(200); // Nome
                                columns.ConstantColumn(100); // Quantidade em Estoque
                                columns.ConstantColumn(100); // Estoque Mínimo
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("ID").Bold();
                                header.Cell().Text("Nome").Bold();
                                header.Cell().Text("Quantidade em Estoque").Bold();
                                header.Cell().Text("Estoque Mínimo").Bold();
                            });

                            foreach (var produto in produtos)
                            {
                                table.Cell().Text(produto.Id.ToString());
                                table.Cell().Text(produto.Nome);
                                table.Cell().Text(produto.Descricao ?? string.Empty);
                                table.Cell().Text(produto.QuantidadeEmEstoque.ToString());
                                table.Cell().Text(produto.EstoqueMinimo.ToString());
                            }
                        });
                    });
                });
        });

        var stream = new MemoryStream();
        document.GeneratePdf(stream);
        stream.Position = 0;

        return File(stream.ToArray(), "application/pdf", "ProdutosComEstoqueBaixo.pdf");
    }

    
    public async Task<IActionResult> ProdutosComPoucasVendas()
    {
        var vendas = await _context.Vendas
            .GroupBy(v => v.ProdutoId)
            .Select(g => new
            {
                ProdutoId = g.Key,
                TotalVendido = g.Sum(v => v.Quantidade)
            })
            .OrderBy(p => p.TotalVendido) // Ordenar de forma crescente para pegar os produtos com menos vendas
            .ToListAsync();

        var produtos = _context.Produtos.ToList();
        var produtosComPoucasVendas = vendas
            .Join(produtos, v => v.ProdutoId, p => p.Id, (v, p) => new
            {
                p.Id,
                p.Nome,
                p.Descricao,
                TotalVendido = v.TotalVendido
            })
            .Where(p => p.TotalVendido > 0) // Excluir produtos não vendidos
            .ToList();

        var document = Document.Create(container =>
        {
            container
                .Page(page =>
                {
                    page.Margin(50);
                    page.Content().Column(column =>
                    {
                        column.Item().AlignCenter().Text("Produtos com Poucas Vendas")
                            .Bold()
                            .FontSize(18);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50); // ID
                                columns.RelativeColumn(200); // Nome
                                columns.ConstantColumn(100); // Total Vendido
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("ID").Bold();
                                header.Cell().Text("Nome").Bold();
                                header.Cell().Text("Total Vendido").Bold();
                            });

                            foreach (var produto in produtosComPoucasVendas)
                            {
                                table.Cell().Text(produto.Id.ToString());
                                table.Cell().Text(produto.Nome);
                                table.Cell().Text(produto.Descricao ?? string.Empty);
                                table.Cell().Text(produto.TotalVendido.ToString());
                            }
                        });
                    });
                });
        });

        var stream = new MemoryStream();
        document.GeneratePdf(stream);
        stream.Position = 0;

        return File(stream.ToArray(), "application/pdf", "ProdutosComPoucasVendas.pdf");
    }

    public IActionResult Relatorios()
    {
        return View();
    }
}
