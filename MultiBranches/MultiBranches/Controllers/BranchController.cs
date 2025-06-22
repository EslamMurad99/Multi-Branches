using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiBranches.Data;
using MultiBranches.Models;

namespace MultiBranches.Controllers
{
    public class BranchController : Controller
    {
        ApplicationDbContext _context;
        public BranchController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var branches = _context.TbBranches.ToList();
            return View(branches);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BranchModel branch)
        {
            if (ModelState.IsValid)
            {
                _context.TbBranches.Add(branch); // into TbnBranches
                _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(branch);
        }

        public IActionResult Edit(int id)
        {
            var branch = _context.TbBranches.Find(id);
            return View(branch);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, BranchModel branch)
        {
            //if (id != branch.BranchId) 
            //    return NotFound();

            if (ModelState.IsValid)
            {
                _context.TbBranches.Update(branch); // into TbnBranches
                _context.SaveChanges();
               
                return RedirectToAction(nameof(Index));
            }
            return View(branch);
        }

        public IActionResult Delete(int id)
        {
            var branch = _context.TbBranches.FirstOrDefault(m => m.BranchId == id);
            return View(branch);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var branch = _context.TbBranches.Find(id);
            _context.TbBranches.Remove(branch);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
