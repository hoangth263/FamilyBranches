using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InteractiveFamilyTree.DTO.Models;
using Microsoft.AspNetCore.Authorization;
using InteractiveFamilyTree.DAO.IServices;

namespace InteractiveFamilyTree.RazorPage.Pages.Events
{
    [Authorize(Roles ="admin")]
    public class IndexModel : PageModel
    {
        private readonly IFamilyEventService _service;

        public IndexModel(IFamilyEventService service)
        {
            _service = service;
        }

        public IList<FamilyEvent> FamilyEvent { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_service != null)
            {
                FamilyEvent = await _service.Get(includeProperties: e => e.Tree);
            }
        }
    }
}
