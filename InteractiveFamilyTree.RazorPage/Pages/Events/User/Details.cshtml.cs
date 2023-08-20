using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InteractiveFamilyTree.DTO.Models;
using InteractiveFamilyTree.DAO.IServices;
using InteractiveFamilyTree.DAO.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace InteractiveFamilyTree.RazorPage.Pages.Events.User
{
    [Authorize(Roles = "member,admin,manager")]
    public class DetailsModel : PageModel
    {
        private readonly IFamilyEventService _service;
        private readonly IFamilyMemberService _familyMemberService;

        public DetailsModel(IFamilyEventService service, IFamilyMemberService familyMemberService)
        {
            _service = service;
            _familyMemberService = familyMemberService;
        }

        public FamilyEvent FamilyEvent { get; set; } = default!;

        public string Role { get; set; } = null;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            int treeId = SessionHelper.GetIntFromSession(HttpContext.Session, "treeId");
            await CheckRole(treeId);

            if (Role == null || Role == "") return RedirectToPage("/index");

            if (id == null || _service == null || id == 0)
            {
                return NotFound();
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

        public async Task CheckRole(int treeId)
        {
            int id = int.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value);
            var check = await _familyMemberService.Get(f => f.MemberId == id && f.TreeId == treeId);
            if (check != null && check.Count() > 0)
            {
                Role = check.FirstOrDefault().Role;
            }
            return;
        }
    }
}
