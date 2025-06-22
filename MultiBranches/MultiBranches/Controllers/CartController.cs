using Microsoft.AspNetCore.Mvc;
using MultiBranches.Data;
using MultiBranches.Extensions;
using MultiBranches.Models;

namespace MultiBranches.Controllers
{
    public class CartController : Controller
    {
        ApplicationDbContext _context;
        const string CartKey = "cart";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartKey);
            return View(cart);
        }

        public IActionResult AddToCart(int productId, int branchId)
        {
            var product = _context.TbProducts.Find(productId);if (product == null) return  NotFound();
            var branch = _context.TbBranches.Find(branchId);if (branch == null) return NotFound();

            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartKey) ?? new List<CartItem>();
            var item = cart.FirstOrDefault(i => i.ProductId == productId && i.BranchId == branchId);

            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = productId,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    BranchId = branchId
                });
            }

            HttpContext.Session.SetObject(CartKey, cart);
            return RedirectToAction("Index", "Home", new { branchId });
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartKey);
            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null) cart.Remove(item);
            HttpContext.Session.SetObject(CartKey, cart);
            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartKey) ?? new List<CartItem>();

            foreach (var item in cart)
            {
                var bp = _context.TbBranchProducts.FirstOrDefault(b =>
                    b.ProductId == item.ProductId && b.BranchId == item.BranchId);

                if (bp != null && bp.Quantity >= item.Quantity)
                {
                    bp.Quantity -= item.Quantity;
                }
                else
                {
                    TempData["Error"] = $"المنتج {item.ProductName} غير متوفر بالكمية المطلوبة.";
                    return RedirectToAction("Index");
                }
            }

            _context.SaveChanges();
            HttpContext.Session.Remove(CartKey);
            TempData["Success"] = "تمت عملية الشراء بنجاح ✅";
            return RedirectToAction("Index");
        }
        /*
        public IActionResult CheckoutRedirect()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartKey) ?? new List<CartItem>();
            var total = cart.Sum(i => i.Price * i.Quantity);
            return RedirectToAction("Payment", "Stripe", new { total });
        }
        */
        public IActionResult CheckoutRedirect()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartKey) ?? new List<CartItem>();
            var total = cart.Sum(i => i.Price * i.Quantity);

            if (total <= 0)
            {
                TempData["Error"] = "السلة فارغة. الرجاء إضافة منتجات قبل الدفع.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Payment", "Stripe", new { total });
        }


    }
}
