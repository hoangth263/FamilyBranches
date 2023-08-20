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
using System.Data;

namespace InteractiveFamilyTree.RazorPage.Pages.Events
{
    [Authorize(Roles = "admin")]
    public class EditModel : PageModel
    {
        private readonly IFamilyEventService _service;
        private readonly IFamilyTreeService _treeservice;

        public EditModel(IFamilyEventService service, IFamilyTreeService treeservice)
        {
            _service = service;
            _treeservice = treeservice; 
        }

        [BindProperty]
        public FamilyEvent FamilyEvent { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _service == null)
            {
                return NotFound();
            }

            var familyevent =  await _service.GetByID((int) id);
            if (familyevent == null)
            {
                return NotFound();
            }
            FamilyEvent = familyevent;
           ViewData["TreeId"] = new SelectList(await _treeservice.Get(), "Id", "FirstName");
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
            var familyevent = await _service.GetByID(FamilyEvent.Id);

            if (familyevent != null)
            {
                await _service.Update(FamilyEvent);
            }

            return RedirectToPage("./Index");
        }

    }
}
