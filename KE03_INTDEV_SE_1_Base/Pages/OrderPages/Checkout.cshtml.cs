using DataAccessLayer.Models;
using KE03_INTDEV_SE_1_Base.Pages.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Webshop.Pages.OrderPages
{
    public class CheckoutModel : PageModel
    {
        [BindProperty, Required(ErrorMessage = "Naam is verplicht")]
        public string CustomerName { get; set; }

        [BindProperty, Required(ErrorMessage = "Adres is verplicht")]
        public string CustomerAddress { get; set; }

        [BindProperty, Required(ErrorMessage = "Bezorgmethode is verplicht")]
        public string DeliveryMethod { get; set; }

        [BindProperty, Required(ErrorMessage = "Betaalmethode is verplicht")]
        public string PaymentMethod { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var checkoutInfo = new CheckoutInfo
            {
                Name = CustomerName,
                Address = CustomerAddress,
                DeliveryMethod = DeliveryMethod,
                PaymentMethod = PaymentMethod
            };

            HttpContext.Session.SetObject("CheckoutInfo", checkoutInfo);

            return RedirectToPage("CheckoutReview");
        }
    }
}
