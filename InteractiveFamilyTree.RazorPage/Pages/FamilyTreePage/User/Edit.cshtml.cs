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

namespace InteractiveFamilyTree.RazorPage.Pages.FamilyTreePage.User
{
    [Authorize(Roles = "manager")]
    public class EditModel : PageModel
    {
        private readonly IFamilyTreeService _familyTreeService;

        public EditModel(IFamilyTreeService familyTreeService)
        {
            _familyTreeService = familyTreeService;
        }

        [BindProperty]
        public FamilyTree FamilyTree { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _familyTreeService == null)
            {
                return NotFound();
            }

            var familytree = await _familyTreeService.GetByID(id??0);
            if (familytree == null)
            {
                return NotFound();
            }
            FamilyTree = familytree;
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
            try
            {
                await _familyTreeService.Update(FamilyTree);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FamilyTreeExists(FamilyTree.Id))
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

        private bool FamilyTreeExists(int id)
        {
            return !_familyTreeService.GetByID(id).Equals(null);
        }
    }
}
