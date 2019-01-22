using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;

namespace ITour.Pages.AppUsers.Customers
{
    public class SearchPersonModel : PageModel
    {
        ApplicationDbContext _context;

        public SearchPersonModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public JsonResult OnGet(string term)
        {
            List<Person> people = _context.People
                .Where(p => p.Surname.Contains(term))
                .AsNoTracking().ToList();

            return people.Count == 0
                ? new JsonResult(new { label = "Не найден..." })
                : new JsonResult(people.Select(t => new { value = t.Id, label = t.FullName }));

        }
    }
}