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

namespace InteractiveFamilyTree.RazorPage.Pages.FamilyTreePage
{
    [Authorize(Roles = "admin")]
    public class DetailsModel : PageModel
    {
        private readonly IFamilyTreeService _familyTreeService;

        public DetailsModel(IFamilyTreeService familyTreeService)
        {
            _familyTreeService = familyTreeService;
        }


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
            else
            {
                FamilyTree = familytree;
            }
            return Page();
        }
    }
}
