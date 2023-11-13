using Assignment1v3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Assignment1v3.Pages
{
    public class CalendarModel : PageModel
    {
        private readonly Data.Assignment1v3Context _context;

        public CalendarModel(Data.Assignment1v3Context context)
        {
            _context = context;
        }

        public IList<Event> UserEventList { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Event != null)
            {
                // Get the currently authenticated user's email address claim
                var userEmailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

                if (userEmailClaim != null)
                {
                    UserEventList = await _context.Event.Where(x => x.userId.Contains(userEmailClaim.Value)).ToListAsync();
                }
            }
        }
    }
}