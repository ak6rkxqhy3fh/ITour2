using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppUsers.People.Files
{
    public class ThumbnailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IAppFileHandler _appFileHandler;

        public ThumbnailModel(ApplicationDbContext context, IAppFileHandler appFileHandler)
        {
            _context = context;
            _appFileHandler = appFileHandler;
        }

        public PersonFile PersonFile { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
                return NotFound();

            PersonFile = await _context.PersonFile.AsNoTracking()
                .Include(p => p.Person).FirstOrDefaultAsync(m => m.Id == id);

            if (PersonFile == null)
                return NotFound();

            return PhysicalFile(_appFileHandler.GetThumbnailPath(PersonFile), "image/jpeg");
        }
    }
}
