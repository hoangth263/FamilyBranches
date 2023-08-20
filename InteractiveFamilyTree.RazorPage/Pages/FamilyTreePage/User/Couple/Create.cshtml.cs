using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InteractiveFamilyTree.DTO.Models;
using InteractiveFamilyTree.DAO.IServices;
using Microsoft.Identity.Client;

namespace InteractiveFamilyTree.RazorPage.Pages.FamilyTreePage.User.Couple
{
    public class CreateModel : PageModel
    {
        private readonly ICoupleRelationshipService _coupleRelationshipService;
        private readonly IFamilyMemberService _familyMemberService;
        public CreateModel(ICoupleRelationshipService coupleRelationshipService, IFamilyMemberService familyMemberService)
        {
            this._coupleRelationshipService = coupleRelationshipService;
            this._familyMemberService = familyMemberService;
        }
        [BindProperty] public IList<FamilyMember> MemberInTree { get; set; } = new List<FamilyMember>();
        public async Task<IActionResult> OnGet()
        {
            if (_coupleRelationshipService != null && _familyMemberService != null)
            {
                if (SessionHelper.GetStringFromSession(HttpContext.Session, "treeId") != null)
                {
                    int id = int.Parse(SessionHelper.GetStringFromSession(HttpContext.Session, "treeId"));
                    IList<FamilyMember> familyMembers = await _familyMemberService.Get(includeProperties: c => c.Member);
                    if (familyMembers != null)
                    {
                        foreach (FamilyMember familyMember in familyMembers)
                        {
                            if (familyMember.TreeId == id)
                            {
                                if (MemberInTree == null)
                                {
                                    MemberInTree = new List<FamilyMember>();
                                }
                                MemberInTree.Add(familyMember);
                            }
                        }
                        IList<FamilyMember> Males = new List<FamilyMember>();
                        IList<FamilyMember> Females = new List<FamilyMember>();
                        foreach (FamilyMember familyMember in MemberInTree)
                        {
                            if (familyMember.Gender)
                            {
                                Males.Add(familyMember);
                            }
                            else
                            {
                                Females.Add(familyMember);
                            }
                        }

                        ViewData["HusbandId"] = new SelectList(Males, "Id", "FullName");
                        ViewData["WifeId"] = new SelectList(Females, "Id", "FullName");
                    }
                }
                else RedirectToPage("/Index");
            }
            else RedirectToPage("/Index");
            return Page();
        }

        [BindProperty]
        public CoupleRelationship CoupleRelationship { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid ||_coupleRelationshipService == null || CoupleRelationship == null)
            {
                return Page();
            }
          await _coupleRelationshipService.AddAsync(CoupleRelationship);
            return RedirectToPage("./Index");
        }
    }
}
