using ITour.Data;
using System.Linq;

namespace ITour.Services.Options
{

    public class AppOptionsProvider : IAppOptionsProvider
    {
        private readonly ApplicationDbContext _context;
        public AppOptions AppOptions { get; set; }

        public AppOptionsProvider(ApplicationDbContext context)
        {
            _context = context;
            AppOptions = _context.AppOptions.FirstOrDefault(); 
        }

    }
}
