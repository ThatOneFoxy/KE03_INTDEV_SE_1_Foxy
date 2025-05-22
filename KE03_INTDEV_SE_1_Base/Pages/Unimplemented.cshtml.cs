using DataAccessLayer;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Webshop.Pages
{
    public class Unimplemented : PageModel
    {

        public Unimplemented(MatrixIncDbContext context)
        {
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
