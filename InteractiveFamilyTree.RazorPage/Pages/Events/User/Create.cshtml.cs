using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InteractiveFamilyTree.DTO.Models;
using InteractiveFamilyTree.DAO.IServices;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace InteractiveFamilyTree.RazorPage.Pages.Events.User
{
    [Authorize(Roles = "member,admin,manager")]
    public class CreateModel : PageModel
    {
        private readonly IFamilyEventService _eventService;
        private readonly IFamilyTreeService _treeService;
        private readonly IFamilyMemberService _familyMemberService;

        public CreateModel(IFamilyEventService eventService, IFamilyTreeService treeService, IFamilyMemberService familyMemberService)
        {
            _eventService = eventService;
            _treeService = treeService;
            _familyMemberService = familyMemberService;
        }

        public async Task<IActionResult> OnGet()
        {
            int treeId = SessionHelper.GetIntFromSession(HttpContext.Session, "treeId");
            if (await CheckRole(treeId) == null || await CheckRole(treeId) != "manager")
            {
                return RedirectToPage("./Index");
            }
            return Page();
        }

        [BindProperty]
        public FamilyEvent FamilyEvent { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            int treeId = SessionHelper.GetIntFromSession(HttpContext.Session, "treeId");

            var tree = (await _treeService.Get(t => t.Id == treeId)).FirstOrDefault();
            FamilyEvent.TreeId = tree.Id;
            FamilyEvent.Tree = tree;
            //if(FamilyEvent.Date < DateTime.Now && FamilyEvent.Type == false)
            //{
            //    ModelState.AddModelError("Date", "Invalid Date!!!");
            //}

            if (!ModelState.IsValid || _eventService == null || FamilyEvent == null)
            {
                return Page();
            }

            await _eventService.AddAsync(FamilyEvent);

            return RedirectToPage("./Index");
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
