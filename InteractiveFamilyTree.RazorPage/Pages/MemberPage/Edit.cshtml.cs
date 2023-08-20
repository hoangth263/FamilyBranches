using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InteractiveFamilyTree.DTO.Models;
using InteractiveFamilyTree.DAO.IServices;
using Microsoft.AspNetCore.Authorization;

namespace InteractiveFamilyTree.RazorPage.Pages.MemberPage
{
    [Authorize(Roles = "admin")]
    public class EditModel : PageModel
    {
        private readonly InteractiveFamilyTreeOfficalContext _context;
        private readonly IMemberService _memberService;

        public EditModel(IMemberService memberService, InteractiveFamilyTreeOfficalContext context)
        {
            _context = context;
            _memberService = memberService;
        }
        [BindProperty]
        public Member Member { get; set; } = default!;
        [BindProperty]
        public string? emailError { get; set; } = null;
        [BindProperty]
        public string? phoneError { get; set; } = null;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _memberService == null)
            {
                return NotFound();
            }

            var member =  await _memberService.GetByID(id??0);
            if (member == null)
            {
                return NotFound();
            }
            Member = member;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var phoneCheck = await _memberService.CheckPhone(Member.Phone);
            var emailCheck = await _memberService.CheckEmail(Member.Email);
            if (phoneCheck != null && phoneCheck.Id != Member.Id)
            {
                phoneError = "This phone number is existed!";
            }
            if (emailCheck != null && emailCheck.Id != Member.Id)
            {
                emailError = "This email is existed!";
            }
            if (emailError != null && phoneError != null)
            {
                return Page();
            }
            try
            {
                _context.Entry(phoneCheck).State = EntityState.Detached;
                _context.Entry(emailCheck).State = EntityState.Detached;
                await _memberService.Update(Member);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(Member.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MemberExists(int id)
        {
          return !_memberService.GetByID(id).Equals(null);
        }
    }
}
