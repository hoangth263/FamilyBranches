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
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace InteractiveFamilyTree.RazorPage.Pages.FamilyMemberPage
{
    [Authorize(Roles = "admin")]
    public class EditModel : PageModel
    {
        private readonly IFamilyMemberService _familyMemberService;
        private readonly IFamilyTreeService _familyTreeService;
        private readonly IMemberService _memberService;

        public EditModel(IFamilyMemberService familyMemberService
            , IFamilyTreeService familyTreeService
            , IMemberService memberService)
        {
            _familyMemberService = familyMemberService;
            _familyTreeService = familyTreeService;
            _memberService = memberService;
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
            FamilyMember = familymember;
            ViewData["MemberId"] = new SelectList(await _memberService.Get(), "Id", "Email");
            ViewData["TreeId"] = new SelectList(await _familyTreeService.Get(), "Id", "FirstName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var tree = await _familyTreeService.GetByID(FamilyMember.TreeId);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (FamilyMember.Generation > tree.TotalGeneration)
            {
                TempData["genError"] = "Out of range of tree's generation";
                ViewData["MemberId"] = new SelectList(await _memberService.Get(), "Id", "Email");
                ViewData["TreeId"] = new SelectList(await _familyTreeService.Get(), "Id", "FirstName");
                return Page();
            }
            try
            {
                await _familyMemberService.Update(FamilyMember);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FamilyMemberExists(FamilyMember.Id))
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

        private bool FamilyMemberExists(int id)
        {
            return !_familyMemberService.GetByID(id).Equals(null);
        }
    }
}
