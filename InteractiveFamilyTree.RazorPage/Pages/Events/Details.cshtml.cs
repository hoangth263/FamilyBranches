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

namespace InteractiveFamilyTree.RazorPage.Pages.Events
{
    [Authorize(Roles = "admin")]
    public class DetailsModel : PageModel
    {
        private readonly IFamilyEventService _service;

        public DetailsModel(IFamilyEventService service)
        {
            _service = service;
        }

        public FamilyEvent FamilyEvent { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _service == null)
            {
                return NotFound();
            }

            var familyevent = await _service.GetByID((int)id,includeProperties: e => e.Tree);
            if (familyevent == null)
            {
                return NotFound();
            }
            else 
            {
                FamilyEvent = familyevent;
            }
            return Page();
        }
    }
}
