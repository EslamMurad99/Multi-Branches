using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiBranches.Data;
using MultiBranches.Models;

namespace MultiBranches.Controllers
{
    public class HomeController : Controller
    {
        // to open object from dbcontext
        ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int branchId)
        {
            // put branches in list to viewbag
            var branches = _context.TbBranches.ToList();
            ViewBag.TbBranches = branches;
            
            /*
            var selectedBranchId = branchId ?? branches.FirstOrDefault()?.BranchId;
            ViewBag.SelectedBranchId = selectedBranchId;*/

            // to select branch with id 
            var selectedBranchId = branches.FirstOrDefault().BranchId;
            
            // to return product with select from branch
            ViewBag.SelectedBranchId = selectedBranchId;

            // use Linq to do filteration of elements and to chose product from element with selected branch
            var products = _context.TbBranchProducts
                .Where(p => p.BranchId == selectedBranchId) 
                .Include(p => p.Product)
                .ToList();
            
            return View(products);
        }

        [HttpPost]
        public IActionResult Buy(int productId, int branchId)
        {
            var item = _context.TbBranchProducts
                .FirstOrDefault(p => p.ProductId == productId && p.BranchId == branchId);
            // to subtract Qty if we chose product 
            if (item != null && item.Quantity > 0)
            {
                item.Quantity -= 1;
                _context.SaveChanges();
            }

            return RedirectToAction("Index", new { branchId });
        }
    }
}
