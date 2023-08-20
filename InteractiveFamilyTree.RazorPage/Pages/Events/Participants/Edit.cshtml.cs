using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InteractiveFamilyTree.DTO.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace InteractiveFamilyTree.RazorPage.Pages.Events.Participants
{
    [Authorize(Roles = "admin")]
    public class EditModel : PageModel
    {
        private readonly InteractiveFamilyTree.DTO.Models.InteractiveFamilyTreeOfficalContext _context;

        public EditModel(InteractiveFamilyTree.DTO.Models.InteractiveFamilyTreeOfficalContext context)
        {
            _context = context;
        }

        [BindProperty]
        public EventParticipant EventParticipant { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.EventParticipants == null)
            {
                return NotFound();
            }

            var eventparticipant =  await _context.EventParticipants.FirstOrDefaultAsync(m => m.Id == id);
            if (eventparticipant == null)
            {
                return NotFound();
            }
            EventParticipant = eventparticipant;
           ViewData["EventId"] = new SelectList(_context.FamilyEvents, "Id", "Description");
           ViewData["FamilyMemberId"] = new SelectList(_context.FamilyMembers, "Id", "FullName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(EventParticipant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventParticipantExists(EventParticipant.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool EventParticipantExists(int id)
        {
          return (_context.EventParticipants?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
