using InteractiveFamilyTree.DAO.IServices;
using InteractiveFamilyTree.DTO.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace InteractiveFamilyTree.RazorPage.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IMemberService _memberService;

        public RegisterModel(IMemberService memberService)
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
            if (emailError != null && phoneError != null)
            {
                return Page();
            }
            Member.Id = 100;
            await _memberService.AddAsync(Member);
            return RedirectToPage("./Login");
        }
        public async Task<int> newId()
        {
            int id = (await _memberService.Get()).Count();
            while((await _memberService.Get(m => m.Id == id)).Count() > 0)
            {
                id++;
            }
            return id;
        }
    }
}
