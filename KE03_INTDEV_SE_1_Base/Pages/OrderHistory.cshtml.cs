using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Webshop.Pages.OrderPages;

public class OrderHistoryModel : PageModel
{
    private readonly MatrixIncDbContext _context;

    public OrderHistoryModel(MatrixIncDbContext context)
    {
        _context = context;
    }

    public List<Order> Orders { get; set; } = new();
    public List<Order> FilteredOrders => string.IsNullOrWhiteSpace(SearchTerm)
        ? Orders
        : Orders.Where(o => o.OrderLines.Any(ol =>
              ol.Product.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))).ToList();

    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; } = "";

    public IActionResult OnGet()
    {
        // Simulatie van klant, niet alle bestellingen laten zien
        var currentCustomerName = "Neo"; 

        var customer = _context.Customers
            .Include(c => c.Orders)
                .ThenInclude(o => o.OrderLines)
                    .ThenInclude(ol => ol.Product)
            .FirstOrDefault(c => c.Name == currentCustomerName);

        if (customer == null)
        {
            return NotFound();
        }

        Orders = customer.Orders.OrderByDescending(o => o.OrderDate).ToList();
        return Page();
    }
}
