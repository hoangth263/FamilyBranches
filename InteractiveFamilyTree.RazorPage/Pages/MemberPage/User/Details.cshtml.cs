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

namespace InteractiveFamilyTree.RazorPage.Pages.MemberPage.User
{
    [Authorize(Roles = "member")]
    public class DetailsModel : PageModel
    {
        private readonly IMemberService _memberService;

        public DetailsModel(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public Member Member { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _memberService == null)
            {
                return NotFound();
            }

            var member = await _memberService.GetByID(id??0);
            if (member == null)
            {
                return NotFound();
            }
            else 
            {
                Member = member;
            }
            return Page();
        }
    }
}
