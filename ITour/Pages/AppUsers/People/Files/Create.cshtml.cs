using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ITour.Data;
using ITour.Models;
using ITour.Services.Tenants;
using System;
using Microsoft.AspNetCore.Http;

namespace ITour.Pages.AppUsers.People.Files
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenantProvider;
        private readonly IAppFileHandler _appFileHandler;

        public CreateModel(ApplicationDbContext context, ITenantProvider tenantProvider, IAppFileHandler appFileHandler)
        {
            _context = context;
            _tenantProvider = tenantProvider;
            _appFileHandler = appFileHandler;
        }

        public IActionResult OnGet(Guid personId)
        {
            ViewData["PersonId"] = personId;
            return Page();
        }

        [BindProperty]
        public PersonFile PersonFile { get; set; }

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        public async Task<IActionResult> OnPostAsync(Guid personId)
        {
            if (!ModelState.IsValid) {
                ViewData["PersonId"] = personId;
                return Page();}

            PersonFile.Name = UploadedFile.FileName;
            PersonFile.TenantId = _tenantProvider.Tenant.Id;
            PersonFile.PersonId = personId;

            _context.PersonFile.Add(PersonFile);
            await _context.SaveChangesAsync();

            await _appFileHandler.CreateFileAsync(PersonFile, UploadedFile, ModelState);

            if (!ModelState.IsValid)
            {
                ViewData["PersonId"] = personId;
                return Page();
            }

            return RedirectToPage("./Index", new { personId });
        }
    }
}