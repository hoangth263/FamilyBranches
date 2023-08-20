using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InteractiveFamilyTree.DTO.Models;
using InteractiveFamilyTree.DAO.IServices;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using InteractiveFamilyTree.DAO.Services;
using System.Security.Claims;

namespace InteractiveFamilyTree.RazorPage.Pages.Events.User
{
    [Authorize(Roles = "member,admin,manager")]
    public class EditModel : PageModel
    {
        private readonly IFamilyEventService _eventService;
        private readonly IFamilyTreeService _treeService;
        private readonly IFamilyMemberService _familyMemberService;

        public EditModel(IFamilyEventService eventService, IFamilyTreeService treeService, IFamilyMemberService familyMemberService)
        {
            _eventService = eventService;
            _treeService = treeService;
            _familyMemberService = familyMemberService;
        }

        [BindProperty]
        public FamilyEvent FamilyEvent { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _eventService == null)
            {
                return NotFound();
            }
            int treeId = SessionHelper.GetIntFromSession(HttpContext.Session, "treeId");
            if (await CheckRole(treeId) == null || await CheckRole(treeId) != "manager")
            {
                return RedirectToPage("./Index");
            }
            var familyevent =  (await _eventService.Get(m => m.Id == id)).FirstOrDefault();

            if (familyevent == null)
            {
                return NotFound();
            }
            FamilyEvent = familyevent;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            int treeId = SessionHelper.GetIntFromSession(HttpContext.Session, "treeId");

            var tree = (await _treeService.Get(t => t.Id == treeId)).FirstOrDefault();
            FamilyEvent.TreeId = tree.Id;
            FamilyEvent.Tree = tree;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _eventService.Update(FamilyEvent);
            }
            catch (DbUpdateConcurrencyException)
            {
                if ((await _eventService.Get(m => m.Id == FamilyEvent.Id)).FirstOrDefault() == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new { id = FamilyEvent.Id });
        }

        public async Task<string> CheckRole(int treeId)
        {
            int id = int.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value);
            var check = await _familyMemberService.Get(f => f.MemberId == id && f.TreeId == treeId);
            if (check != null && check.Count() > 0)
            {
                return check.FirstOrDefault().Role;
            }
            return null;
        }
    }
}
