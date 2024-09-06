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



  
        /*ação create(Post)*/
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

         /*Ação edit(Get)*/
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null){
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);
            if(produto == null){
                return NotFound();
            }
            return View(produto);
        }

      [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,Preco,QuantidadeEmEstoque,EstoqueMinimo,Unidade,ICMS,CFOP,DataEntrada,DataSaida,DataAlteracao,FornecedorPreferencial")] Produto produto)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoExists(produto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(k => k.Id == id);
        }














 
   

    }
}