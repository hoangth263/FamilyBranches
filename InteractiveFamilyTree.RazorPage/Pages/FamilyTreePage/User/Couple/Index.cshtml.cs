using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InteractiveFamilyTree.DTO.Models;
using InteractiveFamilyTree.DAO.IServices;

namespace InteractiveFamilyTree.RazorPage.Pages.FamilyTreePage.User.Couple
{
    public class IndexModel : PageModel
    {
        private readonly ICoupleRelationshipService _coupleRelationshipService;
        private readonly IFamilyMemberService _familyMemberService;
        public IndexModel(ICoupleRelationshipService coupleRelationshipService, IFamilyMemberService familyMemberService)
        {
            this._coupleRelationshipService = coupleRelationshipService;
            this._familyMemberService = familyMemberService;
        }
        public IList<CoupleRelationship> CoupleRelationship { get; set; } = default!;
        public IList<FamilyMember> MemberInTree { get; set; }
        public async Task OnGetAsync()
        {
            if (_coupleRelationshipService != null && _familyMemberService != null)
            {
                if (SessionHelper.GetStringFromSession(HttpContext.Session, "treeId") != null)
                {
                    int id = int.Parse(SessionHelper.GetStringFromSession(HttpContext.Session, "treeId"));
                    IList<FamilyMember> familyMembers = await _familyMemberService.Get(includeProperties: c => c.Member);
                    IList<CoupleRelationship> allCouple = await _coupleRelationshipService.Get(includeProperties: c => c.Husband);
                    allCouple = await _coupleRelationshipService.Get(includeProperties: c => c.Husband);
                    if (familyMembers != null && allCouple!=null)
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
                        foreach(CoupleRelationship coupleRelationships in CoupleRelationship)
                        {
                            bool checkAdd = false;
                            foreach (FamilyMember familyMember in MemberInTree)
                            {
                                if(checkAdd) { break; }
                                if(coupleRelationships.WifeId==familyMember.Id || coupleRelationships.HusbandId == familyMember.Id)
                                {
                                    CoupleRelationship.Add(coupleRelationships);
                                    checkAdd = true;
                                    break;
                                }
                            }
                        }

                    }
                }
            }
        }
    }
}
