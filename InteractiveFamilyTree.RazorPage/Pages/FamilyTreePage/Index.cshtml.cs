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

namespace InteractiveFamilyTree.RazorPage.Pages.FamilyTreePage;

[Authorize(Roles = "admin")]
public class IndexModel : PageModel
{
    private readonly IFamilyTreeService _familyTreeService;

    public IndexModel(IFamilyTreeService familyTreeService)
    {
        _familyTreeService = familyTreeService;
    }

    public IList<FamilyTree> FamilyTree { get; set; } = default!;

    public async Task OnGetAsync()
    {
        if (_familyTreeService != null)
        {
            FamilyTree = await _familyTreeService.Get();
        }
    }
}
