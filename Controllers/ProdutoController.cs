using Imagens.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Imagens.Controllers
{
    public class ProdutoController : Controller
    {

        private readonly Imagens.Data.AppContext _appContext;

        public ProdutoController(Imagens.Data.AppContext appContext)
        {
            _appContext = appContext;
        }

        // GET: ProdutoController
        public async Task<ActionResult> Index()
        {
            var allProducts = await _appContext.Produtos.ToListAsync();
            return View(allProducts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto, IList<IFormFile> Img)
        {            
            IFormFile uploadedImage = Img.FirstOrDefault();
            MemoryStream ms = new MemoryStream();
            if (Img.Count > 0)
            {
                uploadedImage.OpenReadStream().CopyTo(ms);
                produto.Foto = ms.ToArray();
            }

            _appContext.Produtos.Add(produto);
            _appContext.SaveChanges();
            return RedirectToAction("Index");        
        }


        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var produto = await _appContext.Produtos.FindAsync(id);
            if (produto == null)
            {
                return BadRequest();
            }
            return View(produto);
        }
        
        [HttpGet]
        public async Task<IActionResult>Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var produto = await _appContext.Produtos.FindAsync(id);
            if (produto == null)
            {
                return BadRequest();
            }
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, Produto produto, IList<IFormFile> Img)
        {
            if (id == null)
            {
                return NotFound();
            }
            var dadosAntigos = _appContext.Produtos.AsNoTracking().FirstOrDefault(p=>p.Id==id);
            
            IFormFile uploadedImage = Img.FirstOrDefault();
            MemoryStream ms = new MemoryStream();
            if (Img.Count > 0)
            {
                uploadedImage.OpenReadStream().CopyTo(ms);
                produto.Foto = ms.ToArray();
            }
            else
            {
                produto.Foto = dadosAntigos.Foto;
            }
            if (ModelState.IsValid)
            {                
                _appContext.Update(produto);
                await _appContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete (Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var produto = await _appContext.Produtos.FindAsync(id);
            if (produto == null)
            {
                return BadRequest();
            }
            return View(produto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            var produto = await _appContext.Produtos.FindAsync(id);
            _appContext.Produtos.Remove(produto);
            await _appContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}

