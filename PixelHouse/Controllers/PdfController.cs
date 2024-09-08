using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using PixelHouse.Data;
using PixelHouse.Models;
using System.IO;
using System.Linq;

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
                                columns.RelativeColumn(200); // Descrição
                                columns.ConstantColumn(100); // Quantidade
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("ID").Bold();
                                header.Cell().Text("Nome").Bold();
                                header.Cell().Text("Descrição").Bold();
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

    public IActionResult Relatorios()
    {
        return View();
    }
}
