using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using PixelHouse.Data;
using System.IO;
using System.Linq;

public class ExcelController : Controller
{

    public IActionResult Index()
    {
        return View(); // Retorna a view 'excel.cshtml'
    }
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

    public IActionResult Excel()
    {
        return View();
    }
}
