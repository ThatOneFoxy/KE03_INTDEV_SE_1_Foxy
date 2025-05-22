using DataAccessLayer;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Webshop.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger, MatrixIncDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        private readonly MatrixIncDbContext _context;

        public List<string> Categories { get; set; } = new();

        public async Task OnGetAsync()
        {
            Categories = await _context.Categories
                 .Select(p => p.Name)
                 .Distinct()
                 .OrderBy(p => p)
                 .ToListAsync();

            ViewData["Categories"] = Categories;
        }
    }
}
