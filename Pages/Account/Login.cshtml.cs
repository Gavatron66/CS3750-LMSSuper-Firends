using Assignment1v3.Models;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Assignment1v3.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; }

        private readonly Assignment1v3.Data.Assignment1v3Context _context;

        public LoginModel(Assignment1v3.Data.Assignment1v3Context context)
        {
            _context = context;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (_context.Login == null || Credential == null)
            {
                return Page();
            }

            var UNameList = _context.Login.Where(x => x.Email_Username == Credential.Username).ToList();
            if (UNameList.Count <= 0)
            {
                return NotFound();
            }
            else
            {
                var PassList = UNameList.Where(x => x.Password == Credential.Password).ToList();
                if (PassList.Count == 1)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, UNameList.First().Name_First),
                new Claim(ClaimTypes.Email, UNameList.First().Email_Username),
                new Claim("Role", UNameList.First().Role),
                new Claim("id", UNameList.First().Id.ToString())
            };
                    var identity = new ClaimsIdentity(claims, "AuthCookie");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("AuthCookie", claimsPrincipal);

                    if (claimsPrincipal.Claims.Count() > 1)
                    {
                        if (claimsPrincipal.Claims.ElementAt(2).ToString() == "Role: Student")
                        {
                            return RedirectToPage("/Home/StudentDashboard");
                        }
                        else if (claimsPrincipal.Claims.ElementAt(2).ToString() == "Role: Instructor")
                        {
                            return RedirectToPage("/Home/InstructorDashboard");
                        }
                    }
                    return RedirectToPage("/Logins/Index");

                }
                else
                {
                    return NotFound();
                }
            }
        }

    }
    public class Credential
    {
        public Credential() { }

        public Credential(string username, string password)
        {
            Username = username;
            Password = password;
        }

        [Required]
        [Display(Name = "User Name")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string Password { get; set; }
    }
}