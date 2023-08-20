using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InteractiveFamilyTree.DTO.Models;
using InteractiveFamilyTree.DAO.IServices;
using Microsoft.AspNetCore.Authorization;

namespace InteractiveFamilyTree.RazorPage.Pages.MemberPage
{
    [Authorize(Roles = "admin")]
    public class CreateModel : PageModel
    {
        private readonly IMemberService _memberService;

        public CreateModel(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Member Member { get; set; } = default!;
        [BindProperty]
        public string? emailError { get; set; } = null;
        [BindProperty]
        public string? phoneError { get; set; } = null;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _memberService == null || Member == null)
            {
                return Page();
            }
            if (await _memberService.CheckPhone(Member.Phone) != null)
            {
                phoneError = "This phone number is existed!";
            }
            if (await _memberService.CheckEmail(Member.Email) != null)
            {
                emailError = "This email is existed!";
            }
            if(emailError != null && phoneError != null)
            {
                return Page();
            }
            await _memberService.AddAsync(Member);

            return RedirectToPage("./Index");
        }
    }
}
