using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppUsers.People.Documents.DocumentTypes
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<DocumentType> DocumentType { get;set; }

        public async Task OnGetAsync()
        {
            DocumentType = await _context.DocumentTypes.AsNoTracking().ToListAsync();
        }
    }
}
