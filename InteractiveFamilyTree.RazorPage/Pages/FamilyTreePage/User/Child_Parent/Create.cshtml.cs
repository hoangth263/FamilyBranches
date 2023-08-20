using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InteractiveFamilyTree.DTO.Models;
using InteractiveFamilyTree.DAO.IRepositories;
using InteractiveFamilyTree.DAO.IServices;
using InteractiveFamilyTree.DAO.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;

namespace InteractiveFamilyTree.RazorPage.Pages.Child_Parent
{
    public class CreateModel : PageModel
    {
        private readonly IChildAndParentsRelationShipService childAndParentsRelationShipService;
        private readonly IFamilyMemberService familyMemberService;
        public CreateModel(IChildAndParentsRelationShipService childAndParentsRelationShipService, IFamilyMemberService familyMemberService)
        {
            this.childAndParentsRelationShipService = childAndParentsRelationShipService;
            this.familyMemberService = familyMemberService;
        }
        //[BindProperty]
        //public IList<FamilyMember> FamilyMembers { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (childAndParentsRelationShipService != null && familyMemberService != null)
            {
                int treeId = SessionHelper.GetIntFromSession(HttpContext.Session, "treeId");
                if (treeId != null)
                {
                    var familyMembers = await GetFamilyMember(treeId);

                    if (familyMembers.Count != 0)
                    {
                        ViewData["Members"] = new SelectList(familyMembers, "Id", "FullName");
                        FamilyMembers = familyMembers;

                        return Page();
                    }
                }
            }

            return Page();
        }
        public async Task<List<FamilyMember>> GetFamilyMember(int id)
        {
            List<FamilyMember> allFamilyMembers = await familyMemberService.Get(includeProperties: m => m.Member);
            List<FamilyMember> familyMembers = new List<FamilyMember>();
            if (allFamilyMembers != null)
            {
                foreach (FamilyMember member in allFamilyMembers)
                {
                    if (member.TreeId == id)
                    {
                        familyMembers.Add(member);
                    }
                }
            }
            return familyMembers;
        }

        [BindProperty]
        public ChildAndParentsRelationShip ChildAndParentsRelationShip { get; set; } = default!;
        public List<FamilyMember> FamilyMembers { get; set; }=default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid || childAndParentsRelationShipService == null || ChildAndParentsRelationShip == null)
            {
                ViewData["Members"] = new SelectList(FamilyMembers, "Id", "FullName");
                return Page();
            }
            if((await childAndParentsRelationShipService.Get(c=>c.ChildId==ChildAndParentsRelationShip.ChildId && c.ParentId == ChildAndParentsRelationShip.ParentId)).Count() <= 0)
            {
await childAndParentsRelationShipService.AddAsync(ChildAndParentsRelationShip);
                var parent = await familyMemberService.GetByID(ChildAndParentsRelationShip.ParentId);
                var child = await familyMemberService.GetByID(ChildAndParentsRelationShip.ChildId);
                child.Generation = parent.Generation + 1;
                await familyMemberService.Update(child);
            }
            else
            {
                ViewData["Members"] = new SelectList(FamilyMembers, "Id", "FullName");
                ViewData["Nofication"] = "Duplicated member relationship";
            }
            
             return RedirectToPage("./Index");
        }
    }
}
