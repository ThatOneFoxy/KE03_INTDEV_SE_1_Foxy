using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Webshop.Pages.OrderPages
{
    public class OrderHistoryModel : PageModel
    {
        private readonly MatrixIncDbContext _context;

        public OrderHistoryModel(MatrixIncDbContext context)
        {
            _context = context;
        }

        public List<OrderViewModel> Orders { get; set; } = new();

        public List<string> Categories { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = "";

        public List<OrderViewModel> FilteredOrders =>
            string.IsNullOrWhiteSpace(SearchTerm)
                ? Orders
                : Orders.Where(o => o.OrderLines.Any(ol =>
                      ol.Product.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

        public async Task<IActionResult> OnGetAsync()
        {
            Categories = await _context.Categories
                .Select(p => p.Name)
                .Distinct()
                .OrderBy(p => p)
                .ToListAsync();

            ViewData["Categories"] = Categories;

            var currentCustomerName = "Neo";

            var customer = await _context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderLines)
                        .ThenInclude(ol => ol.Product)
                .FirstOrDefaultAsync(c => c.Name == currentCustomerName);

            if (customer == null)
            {
                return NotFound();
            }

            Orders = customer.Orders
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    CustomerName = customer.Name,
                    OrderDate = o.OrderDate,
                    OrderStatus = o.OrderStatus,
                    PaymentMethod = o.PaymentMethod,
                    DeliveryMethod = o.DeliveryMethod,
                    ShippingAddress = o.ShippingAddress,
                    OrderLines = o.OrderLines,
                    TotalPrice = o.OrderLines.Sum(ol => ol.Quantity * ol.Product.Price)
                })
                .ToList();

            return Page();
        }
    }
}
