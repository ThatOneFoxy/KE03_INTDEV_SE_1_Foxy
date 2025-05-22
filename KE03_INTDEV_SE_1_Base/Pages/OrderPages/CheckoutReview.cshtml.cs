using DataAccessLayer.Models;
using KE03_INTDEV_SE_1_Base.Pages.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Webshop.Pages.OrderPages;

public class CheckoutReviewModel : PageModel
{
    public CheckoutInfo CheckoutInfo { get; set; } = new();
    public List<CartItem> CartItems { get; set; } = new();
    public decimal Total => CartItems.Sum(x => x.Price * x.Quantity) +
                            (CheckoutInfo.DeliveryMethod == "Thuisbezorgd" ? 0.99m : 0);

    public IActionResult OnGet()
    {
        CheckoutInfo = HttpContext.Session.GetObject<CheckoutInfo>("CheckoutInfo") ?? new();
        CartItems = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new();

        if (!CartItems.Any())
        {
            TempData["ToastMessage"] = "Winkelwagen is leeg.";
            return RedirectToPage("/Cart");
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        CheckoutInfo = HttpContext.Session.GetObject<CheckoutInfo>("CheckoutInfo") ?? new();
        CartItems = HttpContext.Session.GetObject<List<CartItem>>("Cart") ?? new();

        var order = new Order
        {
            CustomerId = 1,
            OrderDate = DateTime.Now,
            DeliveryMethod = CheckoutInfo.DeliveryMethod,
            PaymentMethod = CheckoutInfo.PaymentMethod,
            ShippingCost = CheckoutInfo.DeliveryMethod == "Thuisbezorgd" ? 0.99m : 0m,
            OrderLines = CartItems.Select(ci => new OrderLine
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Price
            }).ToList()
        };


        HttpContext.Session.Remove("Cart");
        HttpContext.Session.Remove("CheckoutInfo");

        return RedirectToPage("CheckoutConfirmation");
    }
}
