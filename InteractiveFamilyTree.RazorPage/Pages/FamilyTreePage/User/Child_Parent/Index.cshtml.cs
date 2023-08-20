using InteractiveFamilyTree.DAO.IServices;
using InteractiveFamilyTree.DTO.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InteractiveFamilyTree.RazorPage.Pages.FamilyTreePage.User.Child_Parent
{
    public class IndexModel : PageModel
    {
     private readonly IChildAndParentsRelationShipService _relationShipService;
        private readonly IFamilyMemberService _familyMemberService;
        public IndexModel(IChildAndParentsRelationShipService relationShipService, IFamilyMemberService familyMemberService)
        {
            this._relationShipService = relationShipService;
            this._familyMemberService = familyMemberService;
        }
        public IList<ChildAndParentsRelationShip> ChildAndParentsRelationShip { get;set; } = default!;
        public async Task OnGetAsync()
        {
            if (_relationShipService != null && _familyMemberService!=null)
            {
                if(SessionHelper.GetStringFromSession(HttpContext.Session, "treeId") != null)
                {
                    int id = int.Parse(SessionHelper.GetStringFromSession(HttpContext.Session, "treeId")); 
                    IList<FamilyMember> allFamilyMembers = await _familyMemberService.Get(includeProperties: m => m.Member);
                    IList<FamilyMember> familyMembers = new List<FamilyMember>();
                    if (allFamilyMembers != null)
                    {
                        foreach (FamilyMember member in allFamilyMembers)
                        {
                            if (member.TreeId == id)
                            {
                                familyMembers.Add(member);
                            }
                        }
                        if (familyMembers.Count != 0)
                        {
                            IList<ChildAndParentsRelationShip> allRelationship = await _relationShipService.Get(includeProperties: t => t.Parent);
                            allRelationship = await _relationShipService.Get(includeProperties: t => t.Child);
                            ChildAndParentsRelationShip = new List<ChildAndParentsRelationShip>();
                            foreach (ChildAndParentsRelationShip relationShip in allRelationship)
                            {
                                bool addCheck = false;
                                foreach (FamilyMember member in familyMembers)
                                {
                                    if (addCheck) { break; }
                                    if (member.Id == relationShip.ParentId || member.Id == relationShip.ChildId)
                                    {
                                        addCheck = true;
                                    }
                                }
                                if (addCheck) ChildAndParentsRelationShip.Add(relationShip);
                            }
                        }
                    }
              
                    
                }
               
            }

        }
    }
}
