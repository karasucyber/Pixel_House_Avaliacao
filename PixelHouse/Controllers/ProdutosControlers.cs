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


       /* Ação Index */
    public async Task<IActionResult> Index()
    {
        var produtos = await _context.Produtos.ToListAsync();
        return View(produtos);
    }    

        /* Ação Create (GET) */
        public IActionResult Create()
        {
            return View();
        }

        /* Ação Create (POST) */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao,Preco,QuantidadeEmEstoque,EstoqueMinimo,Unidade,ICMS,CFOP,FornecedorPreferencial")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                produto.DataEntrada = DateTime.Now; // Define a data de entrada como a data atual
                produto.DataSaida = default(DateTime); // Data de saída ainda não está definida
                produto.DataAlteracao = DateTime.Now; // Define a data de alteração como a data atual

                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        /* Ação Edit (GET) */
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        /* Ação Edit (POST) */
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
                    var produtoExistente = await _context.Produtos.FindAsync(id);
                    if (produtoExistente == null)
                    {
                        return NotFound();
                    }

                    // Atualiza as propriedades do produto existente com as novas informações
                    produtoExistente.Nome = produto.Nome;
                    produtoExistente.Descricao = produto.Descricao;
                    produtoExistente.Preco = produto.Preco;
                    produtoExistente.QuantidadeEmEstoque = produto.QuantidadeEmEstoque;
                    produtoExistente.EstoqueMinimo = produto.EstoqueMinimo;
                    produtoExistente.Unidade = produto.Unidade;
                    produtoExistente.ICMS = produto.ICMS;
                    produtoExistente.CFOP = produto.CFOP;
                    produtoExistente.FornecedorPreferencial = produto.FornecedorPreferencial;
                    produtoExistente.DataSaida = produto.DataSaida; // Atualiza a data de saída se necessário
                    produtoExistente.DataEntrada = produto.DataEntrada; // Atualiza a data de entrada se necessário
                    produtoExistente.DataAlteracao = DateTime.Now; // Atualiza a data de alteração

                    _context.Update(produtoExistente);
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

        /* Verifica se o produto existe */
        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(k => k.Id == id);
        }
    }
}
