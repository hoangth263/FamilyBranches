using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InteractiveFamilyTree.DTO.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace InteractiveFamilyTree.RazorPage.Pages.Events.Participants
{
    [Authorize(Roles = "admin")]
    public class CreateModel : PageModel
    {
        private readonly InteractiveFamilyTree.DTO.Models.InteractiveFamilyTreeOfficalContext _context;

        public CreateModel(InteractiveFamilyTree.DTO.Models.InteractiveFamilyTreeOfficalContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["EventId"] = new SelectList(_context.FamilyEvents, "Id", "Description");
        ViewData["FamilyMemberId"] = new SelectList(_context.FamilyMembers, "Id", "FullName");
            return Page();
        }

        [BindProperty]
        public EventParticipant EventParticipant { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.EventParticipants == null || EventParticipant == null)
            {
                return Page();
            }

            _context.EventParticipants.Add(EventParticipant);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
