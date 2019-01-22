using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ITour.Data;
using ITour.Models;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using ITour.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITour.Pages.AppUsers.People
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Person> Person { get; set; }

        [BindProperty(SupportsGet = true)]
        public PersonFilter PersonFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public PersonSort PersonSort { get; set; }

        [BindProperty(SupportsGet = true)]
        public PersonPaginate PersonPaginate { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<Person> personIQ = _context.People
                .Include(p => p.ApplicationUser)
                .Include(p => p.PersonFiles);

            personIQ = PersonFilter.Process(personIQ);

            personIQ = PersonSort.Process(personIQ);

            personIQ = PersonPaginate.Process(personIQ);

            Person = await personIQ.AsNoTracking().ToListAsync();

            ViewData["MonthsOfYear"] = new SelectList(MonthsOfYear.GetDictionary(), "Key", "Value");

            ViewData["PageSize"] = new SelectList(PersonPaginate.PageSizeDictionary, "Key", "Value", PersonPaginate.PageSize);
        }
    }

    public class PersonFilter
    {
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Display(Name = "Имя")]
        public string Firstname { get; set; }
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Месяц рождения")]
        public int? MonthOfBirth { get; set; }

        public IQueryable<Person> Process(IQueryable<Person> personIQ)
        {
            if(Surname != null)
                personIQ = personIQ.Where(p => p.Surname.Contains(Surname));

            if (Firstname != null)
                personIQ = personIQ.Where(p => p.Firstname.Contains(Firstname));

            if (PhoneNumber != null)
                personIQ = personIQ.Where(p => p.ApplicationUser.PhoneNumber.Contains(PhoneNumber));

            if (Email != null)
                personIQ = personIQ.Where(p => p.ApplicationUser.Email.Contains(Email));

            if (MonthOfBirth != null)
                personIQ = personIQ.Where(p => p.BirthDateMonth == MonthOfBirth);

            return personIQ;
        }
        public bool NotAllParamsIsNull => Surname != null || Firstname != null || PhoneNumber != null || Email != null || MonthOfBirth != null;
    }


    public enum PersonSortState
    {
        SurnameAsc = 1, SurnameDesc = 2,
        BirthDateAsc = 3, BirthDateDesc = 4
    }

    public class PersonSort
    {
        public PersonSortState SortOrder { get; set; }
        public PersonSortState SortState { get; set; }
        public PersonSortState SurnameSort { get; set; }
        public PersonSortState BirthDateSort { get; set; }

        public IQueryable<Person> Process(IQueryable<Person> personIQ)
        {
            if (SortOrder == 0)
                SortOrder = SortState;
            else
                SortState = SortOrder;

            SurnameSort = SortOrder == PersonSortState.SurnameAsc ? PersonSortState.SurnameDesc : PersonSortState.SurnameAsc;
            BirthDateSort = SortOrder == PersonSortState.BirthDateAsc ? PersonSortState.BirthDateDesc : PersonSortState.BirthDateAsc;

            switch (SortOrder)
            {
                case PersonSortState.SurnameDesc:
                    personIQ = personIQ.OrderByDescending(o => o.Surname).ThenByDescending(p => p.Firstname).ThenByDescending(p => p.Middlename);
                    break;

                case PersonSortState.BirthDateAsc:
                    personIQ = personIQ.OrderBy(o => o.BirthDate);
                    break;
                case PersonSortState.BirthDateDesc:
                    personIQ = personIQ.OrderByDescending(o => o.BirthDate);
                    break;

                default:
                    personIQ = personIQ.OrderBy(o => o.Surname).ThenBy(p => p.Firstname).ThenBy(p => p.Middlename);
                    break;
            }
            return personIQ;
        }
    }


    public class PersonPaginate : Paginate<Person> { }
}
