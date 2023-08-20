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
using System.Data;

namespace InteractiveFamilyTree.RazorPage.Pages.Events
{
    [Authorize(Roles = "admin")]
    public class CreateModel : PageModel
    {
        private readonly IFamilyEventService _service;
        private readonly IFamilyTreeService _treeservice;

        public CreateModel(IFamilyEventService service, IFamilyTreeService treeservice)
        {
            _service = service;
            _treeservice = treeservice;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["TreeId"] = new SelectList(await _treeservice.Get(), "Id", "FirstName");
            return Page();
        }

        [BindProperty]
        public FamilyEvent FamilyEvent { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _service == null || FamilyEvent == null)
            {
                return Page();
            }
            await _service.AddAsync(FamilyEvent);

            return RedirectToPage("./Index");
        }
    }
}
