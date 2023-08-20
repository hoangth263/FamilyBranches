using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InteractiveFamilyTree.DTO.Models;
using InteractiveFamilyTree.DAO.IServices;
using InteractiveFamilyTree.DAO.Services;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Security.Claims;

namespace InteractiveFamilyTree.RazorPage.Pages.FamilyTreePage.User
{
    [Authorize(Roles = "member")]
    public class CreateModel : PageModel
    {
        private readonly IFamilyTreeService _familyTreeService;
        private readonly IMemberService _memberService;
        private readonly IFamilyMemberService _familyMemberService;

        public CreateModel(IFamilyTreeService familyTreeService
            , IMemberService memberService
            ,IFamilyMemberService familyMemberService)
        {
            _familyTreeService = familyTreeService;
            _memberService = memberService;
            _familyMemberService = familyMemberService;
        }


        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public FamilyTree FamilyTree { get; set; } = default!;
        [TempData]
        public string Notice { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _familyTreeService == null || FamilyTree == null)
            {
                return Page();
            }
            
            int id = int.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value);
            string name = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Name).Value;

            FamilyTree.ManagerId = id;
            FamilyTree.Status = false;
            var tree = await _familyTreeService.AddAsync(FamilyTree);
            if(tree == null)
            {
                Notice = "You have send create request. Please wait for approval from Administrator!";
                return Page();
            }
            SessionHelper.SetStringToSession(HttpContext.Session, "treeId", ""+tree.Id);


            var member = await _memberService.GetByID(id);



            return RedirectToPage("./Index");
        }
    }
}
