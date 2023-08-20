using InteractiveFamilyTree.DAO.IServices;
using InteractiveFamilyTree.DTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace InteractiveFamilyTree.RazorPage.Pages.Admin;

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
            FamilyTree = await _familyTreeService.Get(includeProperties: t => t.Member);
        }
    }
}
