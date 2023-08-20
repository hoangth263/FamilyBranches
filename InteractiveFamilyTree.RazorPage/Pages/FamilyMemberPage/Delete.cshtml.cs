using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InteractiveFamilyTree.DTO.Models;
using InteractiveFamilyTree.DAO.IServices;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace InteractiveFamilyTree.RazorPage.Pages.FamilyMemberPage
{
    [Authorize(Roles = "admin")]
    public class DeleteModel : PageModel
    {
        private readonly IFamilyMemberService _familyMemberService;

        public DeleteModel(IFamilyMemberService familyMemberService)
        {
            _familyMemberService = familyMemberService;
        }

        [BindProperty]
        public FamilyMember FamilyMember { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _familyMemberService == null)
            {
                return NotFound();
            }

            var familymember = await _familyMemberService.GetByID(id ?? 0);

            if (familymember == null)
            {
                return NotFound();
            }
            else
            {
                FamilyMember = familymember;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _familyMemberService == null)
            {
                return NotFound();
            }
            var familymember = await _familyMemberService.GetByID(id ?? 0);

            if (familymember != null)
            {
                FamilyMember = familymember;
                FamilyMember.Status = false;
                await _familyMemberService.Update(FamilyMember);
            }

            return RedirectToPage("./Index");
        }
    }
}
