using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InteractiveFamilyTree.DTO.Models;
using InteractiveFamilyTree.DAO.IServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace InteractiveFamilyTree.RazorPage.Pages.Events.User.Participants
{
    [Authorize(Roles = "member,manager")]
    public class IndexModel : PageModel
    {
        private readonly IEventParticipantService _service;
        private readonly IFamilyMemberService _familyMemberService;

        public IndexModel(IEventParticipantService service, IFamilyMemberService familyMemberService)
        {
            _service = service;
            _familyMemberService = familyMemberService;
        }

        public IList<EventParticipant> EventParticipant { get; set; } = default!;

        public int EventId { get; set; }

        public int FamilyMemberId { get; set; }

        public string Role { get; set; } = null;

        public async Task<IActionResult> OnGetAsync(int eventId, bool? error)
        {
            if (error == true)
            {
                TempData["Message"] = "All family member have been added!!!";
            }

            int treeId = SessionHelper.GetIntFromSession(HttpContext.Session, "treeId");
            await CheckRole(treeId, eventId);

            if (Role == null || Role == "") return RedirectToPage("/Index");

            if (treeId == 0)
            {
                return NotFound();
            }

            if (_service != null || eventId != 0)
            {
                EventId = eventId;
                var list = (await _service.Get(
                    p => p.EventId == eventId,
                    p => p.OrderBy(p => p.FamilyMemberId),
                    p => p.FamilyMember, p => p.Event)).AsQueryable();
                EventParticipant = list.Where(e => e.FamilyMember.Status == true).ToList();
            }
            return Page();
        }

        public async Task<IActionResult> OnGetCreate(int eventId)
        {
            int treeId = SessionHelper.GetIntFromSession(HttpContext.Session, "treeId");
            await CheckRole(treeId, eventId);

            if (Role == null || Role == "") return RedirectToPage("/Index");
            int id = int.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value);
            if (id != 0)
            {
                EventParticipant participant = new EventParticipant { EventId = eventId, FamilyMemberId = id, Status = "Confirmed" };
                await _service.AddAsync(participant);
            }
            return RedirectToAction(nameof(OnGetAsync), new { eventId = eventId });
        }

        public async Task<IActionResult> OnGetEdit(int eventId, int id)
        {
            int treeId = SessionHelper.GetIntFromSession(HttpContext.Session, "treeId");
            await CheckRole(treeId, eventId);

            if (Role == null || Role == "") return RedirectToPage("/Index");

            if (id != 0)
            {
                var participant = await _service.GetByID(id);
                if (participant != null)
                {
                    if (participant.Status == "") participant.Status = "Invited";
                    if (participant.Status != "Invited")
                    {
                        if (participant.Status == "Confirmed")
                        {
                            participant.Status = "Attended";
                        }
                        else participant.Status = "Confirmed";
                    }
                    await _service.Update(participant);
                }
            }
            return RedirectToAction(nameof(OnGetAsync), new { eventId = eventId});
        }

        public async Task<IActionResult> OnGetAccept(int eventId, int id)
        {
            int treeId = SessionHelper.GetIntFromSession(HttpContext.Session, "treeId");
            await CheckRole(treeId, eventId);

            if (Role == null || Role == "") return RedirectToPage("/Index");

            if (id != 0)
            {
                var participant = await _service.GetByID(id);
                if (participant != null)
                {
                    participant.Status = "Confirmed";
                    await _service.Update(participant);
                }
            }
            return RedirectToAction(nameof(OnGetAsync), new { eventId = eventId });
        }

        public async Task<IActionResult> OnGetDelete(int eventId, int id)
        {
            int treeId = SessionHelper.GetIntFromSession(HttpContext.Session, "treeId");
            await CheckRole(treeId, eventId);

            if (Role == null || Role == "") return RedirectToPage("/Index");

            if (id != 0)
            {
                await _service.Delete(id);
            }
            return RedirectToAction(nameof(OnGetAsync), new { eventId = eventId});
        }

        public async Task CheckRole(int treeId, int eventId)
        {
            int id = int.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value);
            var check = await _familyMemberService.Get(f => f.MemberId == id && f.TreeId == treeId);
            if( check != null && check.Count() > 0)
            {
                Role = check.FirstOrDefault().Role;
            }
            if (Role == "member")
            {
                FamilyMemberId = check.FirstOrDefault().Id;
                if ((await _service.Get(p => p.FamilyMemberId == FamilyMemberId && p.EventId == eventId)).Count() > 0)
                {
                    Role = "Added Member";
                }
            }
        }
    }
}
