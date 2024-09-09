using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using PixelHouse.Data;
using System.IO;
using System.Linq;

public class ExcelController : Controller
{
    private readonly ApplicationDbContext _context;

    public ExcelController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult ProdutosEmEstoque()
    {
        var produtos = _context.Produtos.ToList();

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Produtos em Estoque");

            // Cabeçalhos
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Nome";
            worksheet.Cells[1, 3].Value = "Descrição";
            worksheet.Cells[1, 4].Value = "Quantidade em Estoque";

            // Dados
            for (int i = 0; i < produtos.Count; i++)
            {
                var produto = produtos[i];
                worksheet.Cells[i + 2, 1].Value = produto.Id;
                worksheet.Cells[i + 2, 2].Value = produto.Nome;
                worksheet.Cells[i + 2, 3].Value = produto.Descricao;
                worksheet.Cells[i + 2, 4].Value = produto.QuantidadeEmEstoque;
            }

            // Ajustar largura das colunas
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProdutosEmEstoque.xlsx");
        }
    }

    public IActionResult ProdutosMaisVendidos()
    {
        var vendas = _context.Vendas
            .GroupBy(v => v.ProdutoId)
            .Select(g => new
            {
                ProdutoId = g.Key,
                TotalVendido = g.Sum(v => v.Quantidade)
            })
            .OrderByDescending(p => p.TotalVendido)
            .ToList();

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

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Produtos Mais Vendidos");

            // Cabeçalhos
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Nome";
            worksheet.Cells[1, 3].Value = "Descrição";
            worksheet.Cells[1, 4].Value = "Total Vendido";

            // Dados
            for (int i = 0; i < produtosMaisVendidos.Count; i++)
            {
                var produto = produtosMaisVendidos[i];
                worksheet.Cells[i + 2, 1].Value = produto.Id;
                worksheet.Cells[i + 2, 2].Value = produto.Nome;
                worksheet.Cells[i + 2, 3].Value = produto.Descricao;
                worksheet.Cells[i + 2, 4].Value = produto.TotalVendido;
            }

            // Ajustar largura das colunas
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProdutosMaisVendidos.xlsx");
        }
    }

    public IActionResult ProdutosMaisComprados()
    {
        var compras = _context.Compras
            .GroupBy(c => c.ProdutoId)
            .Select(g => new
            {
                ProdutoId = g.Key,
                TotalComprado = g.Sum(c => c.Quantidade)
            })
            .OrderByDescending(c => c.TotalComprado)
            .ToList();

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

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Produtos Mais Comprados");

            // Cabeçalhos
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Nome";
            worksheet.Cells[1, 3].Value = "Descrição";
            worksheet.Cells[1, 4].Value = "Total Comprado";

            // Dados
            for (int i = 0; i < produtosMaisComprados.Count; i++)
            {
                var produto = produtosMaisComprados[i];
                worksheet.Cells[i + 2, 1].Value = produto.Id;
                worksheet.Cells[i + 2, 2].Value = produto.Nome;
                worksheet.Cells[i + 2, 3].Value = produto.Descricao;
                worksheet.Cells[i + 2, 4].Value = produto.TotalComprado;
            }

            // Ajustar largura das colunas
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProdutosMaisComprados.xlsx");
        }
    }

    public IActionResult ProdutosComEstoqueBaixo()
    {
        var produtos = _context.Produtos
            .Where(p => p.QuantidadeEmEstoque < p.EstoqueMinimo)
            .ToList();

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Produtos com Estoque Baixo");

            // Cabeçalhos
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Nome";
            worksheet.Cells[1, 3].Value = "Descrição";
            worksheet.Cells[1, 4].Value = "Quantidade em Estoque";
            worksheet.Cells[1, 5].Value = "Estoque Mínimo";

            // Dados
            for (int i = 0; i < produtos.Count; i++)
            {
                var produto = produtos[i];
                worksheet.Cells[i + 2, 1].Value = produto.Id;
                worksheet.Cells[i + 2, 2].Value = produto.Nome;
                worksheet.Cells[i + 2, 3].Value = produto.Descricao;
                worksheet.Cells[i + 2, 4].Value = produto.QuantidadeEmEstoque;
                worksheet.Cells[i + 2, 5].Value = produto.EstoqueMinimo;
            }

            // Ajustar largura das colunas
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProdutosComEstoqueBaixo.xlsx");
        }
    }

    public IActionResult ProdutosComPoucasVendas()
    {
        var vendas = _context.Vendas
            .GroupBy(v => v.ProdutoId)
            .Select(g => new
            {
                ProdutoId = g.Key,
                TotalVendido = g.Sum(v => v.Quantidade)
            })
            .OrderBy(p => p.TotalVendido)
            .ToList();

        var produtos = _context.Produtos.ToList();
        var produtosComPoucasVendas = vendas
            .Join(produtos, v => v.ProdutoId, p => p.Id, (v, p) => new
            {
                p.Id,
                p.Nome,
                p.Descricao,
                TotalVendido = v.TotalVendido
            })
            .Where(p => p.TotalVendido > 0)
            .ToList();

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Produtos com Poucas Vendas");

            // Cabeçalhos
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Nome";
            worksheet.Cells[1, 3].Value = "Descrição";
            worksheet.Cells[1, 4].Value = "Total Vendido";

            // Dados
            for (int i = 0; i < produtosComPoucasVendas.Count; i++)
            {
                var produto = produtosComPoucasVendas[i];
                worksheet.Cells[i + 2, 1].Value = produto.Id;
                worksheet.Cells[i + 2, 2].Value = produto.Nome;
                worksheet.Cells[i + 2, 3].Value = produto.Descricao;
                worksheet.Cells[i + 2, 4].Value = produto.TotalVendido;
            }

            // Ajustar largura das colunas
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProdutosComPoucasVendas.xlsx");
        }
    }

        public IActionResult Excel()
    {
        return View();
    }
}




          