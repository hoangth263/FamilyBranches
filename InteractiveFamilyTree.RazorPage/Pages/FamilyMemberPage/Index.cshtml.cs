using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InteractiveFamilyTree.DTO.Models;
using InteractiveFamilyTree.DAO.IServices;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace InteractiveFamilyTree.RazorPage.Pages.FamilyMemberPage
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly IFamilyMemberService _familyMemberService;

        public IndexModel(IFamilyMemberService familyMemberService)
        {
            _familyMemberService = familyMemberService;
        }

        public List<FamilyMember> FamilyMember { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_familyMemberService != null)
            {
                FamilyMember = await _familyMemberService
                    .Get();
            }

        }
    }
}
