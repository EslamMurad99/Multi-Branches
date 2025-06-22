using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiBranches.Data;
using MultiBranches.Models;

namespace MultiBranches.Controllers
{
    public class ProductController : Controller
    {
        ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.TbProducts.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductModel product)
        {
            if (ModelState.IsValid)
            {  // Add a new Product
                _context.TbProducts.Add(product); // into TbProducts
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public IActionResult Edit(int id)
        {
            var product = _context.TbProducts.Find(id);
            
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ProductModel product)
        {
            if (id != product.ProductId) 
                return NotFound();

            _context.TbProducts.Update(product); // into TbProducts
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var product = _context.TbProducts.Find(id);
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.TbProducts.Find(id);
            _context.TbProducts.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
