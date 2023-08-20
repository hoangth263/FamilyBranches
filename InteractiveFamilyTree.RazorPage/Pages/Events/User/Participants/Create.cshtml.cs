using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InteractiveFamilyTree.DTO.Models;
using InteractiveFamilyTree.DAO.IServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace InteractiveFamilyTree.RazorPage.Pages.Events.User.Participants
{
    [Authorize(Roles = "member,manager")]
    public class CreateModel : PageModel
    {
        private readonly IEventParticipantService _service;
        private readonly IFamilyMemberService _familyMemberservice;

        public CreateModel(IEventParticipantService service, IFamilyMemberService familyMemberservice)
        {
            _service = service;
            _familyMemberservice = familyMemberservice;
        }

        public async Task<IActionResult> OnGetAsync(int eventId)
        {
            int treeId = SessionHelper.GetIntFromSession(HttpContext.Session, "treeId");
            if (treeId == 0)
            {
                return NotFound();
            }
            if (await CheckRole(treeId) == null || await CheckRole(treeId) != "manager")
            {
                return RedirectToPage("./Index");
            }
            EventId = eventId;
            var test = await _familyMemberservice.Get(f => f.TreeId == treeId && f.Status == true);
            var test2 = await _service.Get(p => p.EventId == eventId);
            foreach (var item in test2)
            {
                test.RemoveAll(f => f.Id == item.FamilyMemberId);
            }
            if (test.Count() == 0)
            {
                return RedirectToPage("./Index", new { eventId = eventId, error = true });
            }
            ViewData["FamilyMemberId"] = new SelectList(test, "Id", "FullName");
            TempData["eventId"] = eventId;
            return Page();
        }

        [BindProperty]
        public EventParticipant EventParticipant { get; set; } = default!;
        [BindProperty]
        public int EventId { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            EventParticipant.EventId = EventId;
            int treeId = SessionHelper.GetIntFromSession(HttpContext.Session, "treeId");
            if (await CheckRole(treeId) == "manager" && EventParticipant.FamilyMemberId == CheckId())
            {
                EventParticipant.Status = "Confirmed";
            }
            else
            {
                EventParticipant.Status = "Invited";
            }
            await _service.AddAsync(EventParticipant);
            return RedirectToPage("./Index", new { eventId = EventId, error = false });
        }

        public async Task<string> CheckRole(int treeId)
        {
            int id = int.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value);
            var check = await _familyMemberservice.Get(f => f.MemberId == id && f.TreeId == treeId);
            if (check != null && check.Count() > 0)
            {
                return check.FirstOrDefault().Role;
            }
            return null;
        }

        public int CheckId()
        {
            int id = int.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value);
            return id;
        }
    }
}
