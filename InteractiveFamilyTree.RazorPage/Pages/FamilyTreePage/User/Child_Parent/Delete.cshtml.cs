using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InteractiveFamilyTree.DTO.Models;
using InteractiveFamilyTree.DAO.IServices;

namespace InteractiveFamilyTree.RazorPage.Pages.Child_Parent
{
    public class DeleteModel : PageModel
    {
        private readonly IChildAndParentsRelationShipService _childAndParentsRelationShipService;

        public DeleteModel(IChildAndParentsRelationShipService childAndParentsRelationShipService)
        {
            this._childAndParentsRelationShipService = childAndParentsRelationShipService;
        }

        [BindProperty]
      public ChildAndParentsRelationShip ChildAndParentsRelationShip { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || this._childAndParentsRelationShipService == null)
            {
                RedirectToPage("./Index");
            }

            ChildAndParentsRelationShip = await _childAndParentsRelationShipService.GetByID(id??0);
            if (ChildAndParentsRelationShip == null)
            {
                RedirectToPage("./Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || this._childAndParentsRelationShipService == null)
            {
                RedirectToPage("./Index");
            }
            ChildAndParentsRelationShip = await _childAndParentsRelationShipService.GetByID(id ?? 0);
            if (ChildAndParentsRelationShip != null)
            { await _childAndParentsRelationShipService.Delete(ChildAndParentsRelationShip);
            }

            return RedirectToPage("./Index");
        }
    }
}
