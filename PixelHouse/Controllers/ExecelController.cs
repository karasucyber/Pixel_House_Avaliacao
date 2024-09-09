using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PixelHouse.Data;
using System;
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
               var headers = new[]
        {
            "ID", "Nome", "Descrição", "Preço", "Quantidade em Estoque", "Estoque Mínimo",
            "Unidade", "ICMS", "CFOP" // Removidos DataDeEntrada, DataDeSaida, DataDeAlteracao, Fornecedor Preferencial
        };
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = worksheet.Cells[1, i + 1];
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        }

        // Dados
        for (int i = 0; i < produtos.Count; i++)
        {
            var produto = produtos[i];
            var row = i + 2;
            worksheet.Cells[row, 1].Value = produto.Id;
            worksheet.Cells[row, 2].Value = produto.Nome;
            worksheet.Cells[row, 3].Value = produto.Descricao;
            worksheet.Cells[row, 4].Value = produto.Preco;
            worksheet.Cells[row, 5].Value = produto.QuantidadeEmEstoque;
            worksheet.Cells[row, 6].Value = produto.EstoqueMinimo;
            worksheet.Cells[row, 7].Value = produto.Unidade;
            worksheet.Cells[row, 8].Value = produto.ICMS;
            worksheet.Cells[row, 9].Value = produto.CFOP;
            // Removido DataDeEntrada, DataDeSaida, DataDeAlteracao, Fornecedor Preferencial
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
                p.Preco,
                TotalVendido = v.TotalVendido
            })
            .ToList();

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Produtos Mais Vendidos");

            // Cabeçalhos
            var headers = new[]
            {
                "ID", "Nome", "Descrição", "Preço", "Total Vendido"
            };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = worksheet.Cells[1, i + 1];
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }

            // Dados
            for (int i = 0; i < produtosMaisVendidos.Count; i++)
            {
                var produto = produtosMaisVendidos[i];
                var row = i + 2;
                worksheet.Cells[row, 1].Value = produto.Id;
                worksheet.Cells[row, 2].Value = produto.Nome;
                worksheet.Cells[row, 3].Value = produto.Descricao;
                worksheet.Cells[row, 4].Value = produto.Preco;
                worksheet.Cells[row, 5].Value = produto.TotalVendido;
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
                p.Preco,
                TotalComprado = c.TotalComprado
            })
            .ToList();

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Produtos Mais Comprados");

            // Cabeçalhos
            var headers = new[]
            {
                "ID", "Nome", "Descrição", "Preço", "Total Comprado"
            };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = worksheet.Cells[1, i + 1];
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }

            // Dados
            for (int i = 0; i < produtosMaisComprados.Count; i++)
            {
                var produto = produtosMaisComprados[i];
                var row = i + 2;
                worksheet.Cells[row, 1].Value = produto.Id;
                worksheet.Cells[row, 2].Value = produto.Nome;
                worksheet.Cells[row, 3].Value = produto.Descricao;
                worksheet.Cells[row, 4].Value = produto.Preco;
                worksheet.Cells[row, 5].Value = produto.TotalComprado;
            }

            // Ajustar largura das colunas
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProdutosMaisComprados.xlsx");
        }
    }

    public IActionResult ProdutosComPoucasVendas()
{
    // Buscar as vendas e contar o total vendido para cada produto
    var vendas = _context.Vendas
        .GroupBy(v => v.ProdutoId)
        .Select(g => new
        {
            ProdutoId = g.Key,
            TotalVendido = g.Sum(v => v.Quantidade)
        })
        .OrderBy(v => v.TotalVendido) // Ordenar pelo total vendido, do menor para o maior
        .ToList();

    // Buscar os produtos
    var produtos = _context.Produtos.ToList();

    // Juntar as vendas com os produtos e pegar os produtos com menos vendas
    var produtosComPoucasVendas = vendas
        .Join(produtos, v => v.ProdutoId, p => p.Id, (v, p) => new
        {
            p.Id,
            p.Nome,
            p.Descricao,
            p.Preco,
            TotalVendido = v.TotalVendido
        })
        .ToList();

    // Criar e configurar a planilha Excel
    using (var package = new ExcelPackage())
    {
        var worksheet = package.Workbook.Worksheets.Add("Produtos com Poucas Vendas");

        // Cabeçalhos
        var headers = new[]
        {
            "ID", "Nome", "Descrição", "Preço", "Total Vendido"
        };
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = worksheet.Cells[1, i + 1];
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        }

        // Dados
        for (int i = 0; i < produtosComPoucasVendas.Count; i++)
        {
            var produto = produtosComPoucasVendas[i];
            var row = i + 2;
            worksheet.Cells[row, 1].Value = produto.Id;
            worksheet.Cells[row, 2].Value = produto.Nome;
            worksheet.Cells[row, 3].Value = produto.Descricao;
            worksheet.Cells[row, 4].Value = produto.Preco;
            worksheet.Cells[row, 5].Value = produto.TotalVendido;
        }

        // Ajustar largura das colunas
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProdutosComPoucasVendas.xlsx");
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
            var headers = new[]
            {
                "ID", "Nome", "Descrição", "Preço", "Quantidade em Estoque", "Estoque Mínimo"
            };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = worksheet.Cells[1, i + 1];
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            }

            // Dados
            for (int i = 0; i < produtos.Count; i++)
            {
                var produto = produtos[i];
                var row = i + 2;
                worksheet.Cells[row, 1].Value = produto.Id;
                worksheet.Cells[row, 2].Value = produto.Nome;
                worksheet.Cells[row, 3].Value = produto.Descricao;
                worksheet.Cells[row, 4].Value = produto.Preco;
                worksheet.Cells[row, 5].Value = produto.QuantidadeEmEstoque;
                worksheet.Cells[row, 6].Value = produto.EstoqueMinimo;
            }

            // Ajustar largura das colunas
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProdutosComEstoqueBaixo.xlsx");
            
        }
        
    }

         public IActionResult Excel()
    {
        return View();
    }
}
