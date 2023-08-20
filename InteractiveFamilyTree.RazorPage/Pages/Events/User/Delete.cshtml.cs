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
using InteractiveFamilyTree.DAO.IServices;
using System.Security.Claims;

namespace InteractiveFamilyTree.RazorPage.Pages.Events.User
{
    [Authorize(Roles = "member,admin,manager")]
    public class DeleteModel : PageModel
    {
        private readonly IFamilyEventService _service;
        private readonly IFamilyMemberService _familyMemberService;

        public DeleteModel(IFamilyEventService service, IFamilyMemberService familyMemberService)
        {
            _service = service;
            _familyMemberService = familyMemberService;
        }

        [BindProperty]
      public FamilyEvent FamilyEvent { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _service == null)
            {
                return NotFound();
            }

            int treeId = SessionHelper.GetIntFromSession(HttpContext.Session, "treeId");
            if(await CheckRole(treeId) == null || await CheckRole(treeId) != "manager")
            {
                return RedirectToPage("./Index");
            }

            var familyevent = await _service.GetByID((int)id);

            if (familyevent == null)
            {
                return NotFound();
            }
            else 
            {
                FamilyEvent = familyevent;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _service == null)
            {
                return NotFound();
            }
            var familyevent = await _service.GetByID((int)id);

            if (familyevent != null)
            {
                FamilyEvent = familyevent;
                await _service.Delete(FamilyEvent);
            }

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
