using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InteractiveFamilyTree.DTO.Models;
using InteractiveFamilyTree.DAO.IServices;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace InteractiveFamilyTree.RazorPage.Pages.MemberPage
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly IMemberService _memberService;

        public IndexModel(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public IList<Member> Member { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_memberService != null)
            {
                Member = await _memberService.Get();
            }
        }
    }
}
