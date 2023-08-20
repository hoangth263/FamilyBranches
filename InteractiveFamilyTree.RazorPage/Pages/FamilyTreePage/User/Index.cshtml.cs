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
using System.Security.Claims;
using InteractiveFamilyTree.DAO.Services;

namespace InteractiveFamilyTree.RazorPage.Pages.FamilyTreePage.User
{
    [Authorize(Roles = "member,manager")]
    public class IndexModel : PageModel
    {
        private readonly IFamilyMemberService _familyMemberService;
        private readonly IFamilyTreeService _familyTreeService;

        public IndexModel(IFamilyTreeService familyTreeService
            ,IFamilyMemberService familyMemberService)
        {
            _familyMemberService = familyMemberService;
            _familyTreeService = familyTreeService;
        }

        public FamilyTree FamilyTree { get; set; } = default!;
        public string Role { get; set; } = null;

        public async Task OnGetAsync()
        {
            int id = int.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value);
            var members = await _familyMemberService.Get(e => e.MemberId.Equals(id));
            if (_familyTreeService != null && members.Count>0)
            {
                FamilyTree = await _familyTreeService.GetByID(members.FirstOrDefault().TreeId);
                Role = members.FirstOrDefault().Role;
            }
        }
    }
}
