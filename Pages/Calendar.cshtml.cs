using Assignment1v3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Assignment1v3.Pages
{
    public class CalendarModel : PageModel
    {
        private readonly Data.Assignment1v3Context _context;

        public CalendarModel(Data.Assignment1v3Context context)
        {
            _context = context;
        }

        public IList<Event> FullEventList { get; set; } = default!;
        public IEnumerable<Event> UserEventList { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Event != null)
            { 
                FullEventList = await _context.Event.ToListAsync();
                //Event = _context.Event.Select(x => new SelectListItem { Value = x.id.ToString(), Text = x.title }).ToList() as IEnumerable<SelectListItem>;

                UserEventList = FullEventList.Where(item => item.userId == "test");
            }
        }
    }
}