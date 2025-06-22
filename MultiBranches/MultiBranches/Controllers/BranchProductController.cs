using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MultiBranches.Data;
using MultiBranches.Models;

namespace MultiBranches.Controllers
{
    public class BranchProductController : Controller
    {
        ApplicationDbContext _context;

        public BranchProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var items = _context.TbBranchProducts
                .Include(bp => bp.Branch)
                .Include(bp => bp.Product)
                .ToList();
            return View(items);
        }

        /*
          public IActionResult Create()
        {
            ViewBag.BranchId = new SelectList(_context.TbBranches, "BranchId", "Name");
            ViewBag.ProductId = new SelectList(_context.TbProducts, "ProductId", "Name");
            return View();
        } 
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BranchProductModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }*/

        public IActionResult Delete(int branchId, int productId)
        {
            var item = _context.TbBranchProducts
                .FirstOrDefault(b => b.BranchId == branchId && b.ProductId == productId);
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int branchId, int productId)
        {
            var item = _context.TbBranchProducts
                .FirstOrDefault(b => b.BranchId == branchId && b.ProductId == productId);
            _context.TbBranchProducts.Remove(item);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
