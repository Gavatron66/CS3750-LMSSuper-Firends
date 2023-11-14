﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Assignment1v3.Data;
using Assignment1v3.Models;
using System.Security.Claims;
using System.Security.Principal;
using Stripe;

namespace Assignment1v3.Pages.Courses
{
    public class CreateModel : PageModel
    {
        private readonly Assignment1v3.Data.Assignment1v3Context _context;
        public List<SelectListItem> Items { get; set; }

        public CreateModel(Assignment1v3.Data.Assignment1v3Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            Schools list = new Schools();
            Items = list.strings.Select(a =>
                                          new SelectListItem
                                          {
                                              Value = a.ToString(),
                                              Text = a
                                          }).ToList();
            return Page();
        }

        [BindProperty]
        public Course Course { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //This nasty line of code grabs the logged-in user, grabs their ID, and converts it to an integer for the DB
            int instructorID = Convert.ToInt32((User.Claims.ElementAt(3).ToString()).Remove(0, 4));
            Course.InstructorId = instructorID;

            if (!ModelState.IsValid || _context.Course == null || Course == null)
            {
                return Page();
            }

            string dateFormat = "MM/dd/yyyy HH:mm";

            var newevent = new Assignment1v3.Models.Event
            {
                title = Course.CourseNumber + ": " + Course.CourseName,
                startTime = Course.StartTime,
                endTime = Course.EndTime,
                //startRecur = Course.StartRecur,
                //endRecur = Course.EndRecur,
                //daysOfWeek = Course.ClassDays,

                // Temporary until we add recur and daysOfWeek functionality to Course creation
                startRecur = DateTime.Parse("2023-10-01 00:00:00.000"),
                endRecur = DateTime.Parse("2023-10-31 00:00:00.000"),
                daysOfWeek = "[1]",
                // ----------------------------------------------------------------------------

                userId = this.User.Claims.ElementAt(1).ToString(),
                url = "/Home/InstructorDashboard",
            };
            _context.Event.Add(newevent);

            _context.Course.Add(Course);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
