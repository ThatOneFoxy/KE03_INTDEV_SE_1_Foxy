using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KE03_INTDEV_SE_1_Base.Pages.Helpers;

namespace Webshop.Pages
{
    public class ProductModel(MatrixIncDbContext context) : PageModel
    {
        private readonly MatrixIncDbContext _context = context;

        public Product? Product { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public int CartCount { get; set; }

        public List<string> Categories { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Parts)
                .FirstOrDefaultAsync(p => p.Id == Id);

            Categories = await _context.Categories
                .Select(p => p.Name)
                .Distinct()
                .OrderBy(p => p)
                .ToListAsync();

            ViewData["Categories"] = Categories;

            if (Product == null)
                return NotFound();

            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new();
            CartCount = cart.Sum(item => item.Quantity);
            ViewData["CartCount"] = CartCount;

            ViewData["PreviousUrl"] = Request.Headers["Referer"].ToString();

            return Page();
        }

        public IActionResult OnPostAddToCart(int id, int quantity)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return RedirectToPage();

            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new();

            var existingItem = cart.FirstOrDefault(i => i.ProductId == id);
            if (existingItem != null)
                existingItem.Quantity += quantity;
            else
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = quantity
                });

            HttpContext.Session.SetObject("Cart", cart);
            TempData["ToastMessage"] = $"{quantity}x {product.Name} toegevoegd aan je winkelmandje";
            return RedirectToPage(new { id });
        }
    }
}
