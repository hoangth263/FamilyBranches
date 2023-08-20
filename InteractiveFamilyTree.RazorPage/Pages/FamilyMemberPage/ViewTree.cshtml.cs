using InteractiveFamilyTree.DAO.IServices;
using InteractiveFamilyTree.DTO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Configuration;

namespace InteractiveFamilyTree.RazorPage.Pages.FamilyMemberPage
{
    public class ViewTreeModel : PageModel
    {
        private readonly IFamilyMemberService _familyMemberService;
        private readonly ICoupleRelationshipService _coupleRelationshipService;
        private readonly IChildAndParentsRelationShipService _childAndParentsRelationShipService;
        public ViewTreeModel(IFamilyMemberService familyMemberService, ICoupleRelationshipService coupleRelationshipService, IChildAndParentsRelationShipService childAndParentsRelationShipService)
        {
            _familyMemberService = familyMemberService;
            _coupleRelationshipService = coupleRelationshipService;
            _childAndParentsRelationShipService = childAndParentsRelationShipService;
        }

        public List<FamilyMember> FamilyMember { get; set; } = default!;
        public List<CoupleRelationship> CoupleRelationship { get; set; } = default!;
        public List<ChildAndParentsRelationShip> ChildAndParentsRelationShip { get; set; } = default!;
        public int Oldest { get; set; } = 0;
        public int Youngest { get; set; } = 0;
        public int[,] Tree { get; set; } = new int[100, 100];
        public string[]Love { get; set; }=new string[100];
        public string[,] ChildParent { get; set; } = new string[100,100];
        public FamilyMember[,] TreeMember { get; set; } = new FamilyMember[100, 100];
        private void Swap(int x1, int y1, int x2, int y2)
        {
            int temp = this.Tree[x1, y1];
            this.Tree[x1, y1] = this.Tree[x2, y2];
            this.Tree[x2, y2] = temp;
        }
        private int[,] AddIntoTree(int[,] tree, FamilyMember member, int x, int y)
        {
            bool add = false;
            if (member != null) {
                for (int i = 0; i <= tree.Length; i++)
                {
                    if (i == x)
                    {
                        for (int j = 0; j <= tree.Length; j++)
                        {
                            if (tree[i, j] == 0)
                            {
                                tree[i, j] = member.Id;
                                add = true;
                                break;
                            }
                        }
                    }
                    if (add) break;
                }

            }
            return tree;
        }
        private void FindLove(int Love1, int Love2)
        {
            int x1 = -1, y1 = -1, x2 = -1, y2 = -1;
            for (int i = Oldest; i <= Youngest; i++)
            {
                if (x1 != -1 && y1 != -1 && x2 != -1 && y2 != -1) break;
                for (int j = 0; j <= Tree.Length; j++)
                {
                    if (Tree[i, j] == 0) { break; }
                    if (Tree[i, j] == Love1)
                    {
                        x1 = i; y1 = j;
                    } else if (Tree[i, j] == Love2)
                    {
                        x2 = i; y2 = j;
                    }
                    if (x1 != -1 && y1 != -1 && x2 != -1 && y2 != -1) break;
                }
            }
            if (x1 != -1 && y1 != -1 && x2 != -1 && y2 != -1) {
                this.Tree[x2, y2] = 0;
                if (this.Tree[x1, y1 + 1] == 0)
                {  
                    this.Tree[x1, y1 + 1] = Love2;
                }
                else
                {
                    MoveBack(x1, y1 + 1,y1);
                    this.Tree[x1, y1 + 1] = Love2;
                }
            }
        }
        private void MoveBack(int x,int y,int y1)
        {
            int max = -1;
          for(int i=y1;i<=Tree.Length;i++)
            {
                if (Tree[x, i] == 0)
                {
                    break;
                }
                else max = i;
            }
          for(int j=y1+max;j>y1;j--)
            {
                this.Tree[x,j] = this.Tree[x,j-1];
            }
            this.Tree[x,y] = 0;
        }


        public async Task OnGetAsync(int id)
        {

            if (_familyMemberService != null && _coupleRelationshipService != null && _childAndParentsRelationShipService != null)
            {
                FamilyMember = await _familyMemberService
                    .Get(includeProperties: f => f.Member);
                CoupleRelationship = CoupleRelationship = await _coupleRelationshipService.Get(includeProperties: f => f.Husband);
                CoupleRelationship = await _coupleRelationshipService.Get(includeProperties: f => f.Wife);
                ChildAndParentsRelationShip = await _childAndParentsRelationShipService.Get(includeProperties: f => f.Parent);
                ChildAndParentsRelationShip = await _childAndParentsRelationShipService.Get(includeProperties: f => f.Child);
                Oldest = FamilyMember[0].Generation;
                Oldest = FamilyMember[0].Generation;
                foreach (FamilyMember familyMember in FamilyMember.ToList())
                {
                    if (familyMember.TreeId != id)
                    {
                        FamilyMember.Remove(familyMember);
                    }
                    else
                    {
                        if (familyMember.Generation < Oldest)
                        {
                            Oldest = familyMember.Generation;
                        }
                        else
                                          if (familyMember.Generation > Youngest)
                        {
                            Youngest = familyMember.Generation;
                        }
                    }
                }
                int y = 1;

                foreach (FamilyMember familyMember in FamilyMember)
                {
                    if (familyMember.Generation == Oldest)
                    {
                        this.Tree = AddIntoTree(this.Tree, familyMember, Oldest, y);
                    }
                    else this.Tree = AddIntoTree(Tree, familyMember, (familyMember.Generation), y);
                }

                for (int i = Oldest; i <= Youngest; i++)
                {
                    for (int j = 0; j < Tree.Length - 1; j++)
                    {
                        if (Tree[i, j] != 0)
                        {
                            for (int t = j + 1; t < Tree.Length; t++)
                            {
                                if (Tree[i, t] != 0)
                                {
                                    if (_familyMemberService.GetByID(Tree[i, j]).Result.Birthday > _familyMemberService.GetByID(Tree[i, t]).Result.Birthday)
                                    {
                                        Swap(i, j, i, t);
                                    }

                                }
                                else break;
                            }
                        }
                        else break;
                    }
                }
                foreach (CoupleRelationship relationShips in CoupleRelationship)
                {
                    var random = new Random(); var color = String.Format("{0:X6}", random.Next(0x1000000));
                    Love[relationShips.HusbandId] = color;
                    Love[relationShips.WifeId] = color;
                }
                List<int> parents = new List<int>();
                foreach (ChildAndParentsRelationShip relationShips in ChildAndParentsRelationShip)
                {
                    if (!parents.Contains(relationShips.ParentId))
                    {
                        parents.Add(relationShips.ParentId);
                    }
                }  
                foreach(int parent in parents)
                {
                    var random = new Random(); var color = String.Format("{0:X6}", random.Next(0x1000000));

                    ChildParent[parent,0] = color;
                    foreach(ChildAndParentsRelationShip childAndParentsRelationShips in ChildAndParentsRelationShip)
                    {
                        if(childAndParentsRelationShips.ParentId == parent)
                        {
                            ChildParent[childAndParentsRelationShips.ChildId, 1] = color;
                        }
                    }
                }
                   
                for (int i = Oldest; i <= Youngest; i++)
                {
                    for (int j = 0; j <= Tree.Length; j++)
                    {
                        if (Tree[i, j] != 0)
                        {
                            TreeMember[i, j] = _familyMemberService.GetByID(Tree[i, j]).Result;
                        }
                        else break;
                    }

                }
            }
        }
    }
}
