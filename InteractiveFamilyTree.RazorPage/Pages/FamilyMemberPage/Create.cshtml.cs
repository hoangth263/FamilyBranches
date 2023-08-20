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
using System.Data;

namespace InteractiveFamilyTree.RazorPage.Pages.FamilyMemberPage
{
    [Authorize(Roles = "manager")]
    public class CreateModel : PageModel
    {
        private readonly IFamilyMemberService _familyMemberService;
        private readonly IMemberService _memberService;
        private readonly IFamilyTreeService _familyTreeService;

        public CreateModel(IFamilyMemberService familyMemberService
            , IMemberService memberService
            , IFamilyTreeService familyTreeService)
        {
            _familyMemberService = familyMemberService;
            _memberService = memberService;
            _familyTreeService = familyTreeService;
        }



        public async Task<IActionResult> OnGet()
        {
            return Page();
        }

        [BindProperty]
        public FamilyMember FamilyMember { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            FamilyMember.TreeId = SessionHelper.GetIntFromSession(HttpContext.Session, "treeId");
            var tree = await _familyTreeService.GetByID(FamilyMember.TreeId);
            if (!ModelState.IsValid || _familyMemberService == null || FamilyMember == null)
            {
                return Page();
            }

            if (FamilyMember.Generation > tree.TotalGeneration)
            {
                TempData["genError"] = "Out of range of tree's generation";
                return Page();
            }

            await _familyMemberService.AddAsync(FamilyMember);

            return RedirectToPage("./User/Index");
        }
    }
}