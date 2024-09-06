using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PixelHouse.Data;
using PixelHouse.Models;

namespace PixelHouse.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProdutosController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*ação index*/
        public async Task<IActionResult> Index()
        {
            return View(await _context.Produtos.ToListAsync());
        }

        /*ação create(Get)*/
        public IActionResult create()
        {
            return View();
        }

        [HttpPost]

        /*Anti CSRF*/
        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao,Preco,QuantidadeEmEstoque,EstoqueMinimo,Unidade,ICMS,CFOP,DataEntrada,DataSaida,DataAlteracao,FornecedorPreferencial")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }









    }
}