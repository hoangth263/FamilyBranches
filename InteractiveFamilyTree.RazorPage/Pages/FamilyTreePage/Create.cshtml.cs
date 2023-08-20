using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using InteractiveFamilyTree.DTO.Models;
using InteractiveFamilyTree.DAO.IServices;
using InteractiveFamilyTree.DAO.Services;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Security.Claims;

namespace InteractiveFamilyTree.RazorPage.Pages.FamilyTreePage
{
    [Authorize(Roles = "admin")]
    public class CreateModel : PageModel
    {
        private readonly IFamilyTreeService _familyTreeService;

        public CreateModel(IFamilyTreeService familyTreeService)
        {
            _familyTreeService = familyTreeService;
        }


        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public FamilyTree FamilyTree { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _familyTreeService == null || FamilyTree == null)
            {
                return Page();
            }
            int id = int.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value);

            FamilyTree.ManagerId = id;
            await _familyTreeService.AddAsync(FamilyTree);

            return RedirectToPage("./Index");
        }
    }
}
