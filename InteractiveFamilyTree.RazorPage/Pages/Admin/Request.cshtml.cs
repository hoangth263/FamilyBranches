using InteractiveFamilyTree.DAO.IServices;
using InteractiveFamilyTree.DAO.Services;
using InteractiveFamilyTree.DTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Metrics;
using System.Linq;

namespace InteractiveFamilyTree.RazorPage.Pages.Admin;

[Authorize(Roles = "admin")]
public class RequestModel : PageModel
{
    private readonly IFamilyTreeService _familyTreeService;
    private readonly IMemberService _memberService;
    private readonly IFamilyMemberService _familyMemberService;

    public RequestModel(IFamilyTreeService familyTreeService
        , IMemberService memberService
        , IFamilyMemberService familyMemberService)
    {
        _familyTreeService = familyTreeService;
        _memberService = memberService;
        _familyMemberService = familyMemberService;
    }

    public IList<FamilyTree> FamilyTree { get; set; } = default!;
    [BindProperty]
    public int Id { get; set; }

    public async Task OnGetAsync()
    {
        if (_familyTreeService != null)
        {
            FamilyTree = await _familyTreeService.Get(filter: t => t.Status == false
            , includeProperties: i => i.Member);
        }
    }
    public async Task<IActionResult> OnPost()
    {
        // Get the value of the clicked button from the Request.Form collection
        string buttonValue = Request.Form["submitButton"];
        var tree = await _familyTreeService.GetByID(Id);
        // Handle each button based on its value
        if (buttonValue == "accept")
        {
            tree.Status = true;
            await _familyTreeService.Update(tree);
            var member =await _memberService.GetByID(tree.ManagerId);
            await _familyMemberService.AddAsync(new FamilyMember
            {
                TreeId = tree.Id,
                MemberId = tree.ManagerId,
                FullName = member.FullName,
                Gender = member.Gender,
                Generation = 1,
                Birthday = member.Birthday,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Role = "manager",
                Status = true,
                StatusHealth = true
            });
            // Handle the "Save" button click
            // Perform the save action here
        }
        else if (buttonValue == "reject")
        {
            //tree.FirstName = tree.FirstName.Substring(0) + " (Rejected)";
            //await _familyTreeService.Update(tree);
            await _familyTreeService.Delete(tree);
        }
        if (_familyTreeService != null)
        {
            FamilyTree = await _familyTreeService.Get(filter: t => t.Status == false
            , includeProperties: i => i.Member);
        }
        // If no button value matches, return to the same page or perform appropriate action
        return Page();
    }
}

