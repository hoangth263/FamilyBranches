using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InteractiveFamilyTree.DTO.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace InteractiveFamilyTree.RazorPage.Pages.Events.Participants
{
    [Authorize(Roles = "admin")]
    public class DeleteModel : PageModel
    {
        private readonly InteractiveFamilyTree.DTO.Models.InteractiveFamilyTreeOfficalContext _context;

        public DeleteModel(InteractiveFamilyTree.DTO.Models.InteractiveFamilyTreeOfficalContext context)
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

            var eventparticipant = await _context.EventParticipants.FirstOrDefaultAsync(m => m.Id == id);

            if (eventparticipant == null)
            {
                return NotFound();
            }
            else 
            {
                EventParticipant = eventparticipant;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.EventParticipants == null)
            {
                return NotFound();
            }
            var eventparticipant = await _context.EventParticipants.FindAsync(id);

            if (eventparticipant != null)
            {
                EventParticipant = eventparticipant;
                _context.EventParticipants.Remove(EventParticipant);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
