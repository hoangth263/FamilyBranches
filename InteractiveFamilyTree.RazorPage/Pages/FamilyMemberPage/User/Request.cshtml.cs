using InteractiveFamilyTree.DAO.IServices;
using InteractiveFamilyTree.DTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InteractiveFamilyTree.RazorPage.Pages.FamilyMemberPage.User;

[Authorize(Roles = "manager")]
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

    public IList<FamilyMember> FamilyMembers{ get; set; } = default!;
    [BindProperty]
    public int Id { get; set; }

    public async Task OnGetAsync()
    {
        if (_familyTreeService != null)
        {
            FamilyMembers = await _familyMemberService.Get(filter: t => t.Status == false
            , includeProperties: i => i.Member);
        }
    }
    public async Task<IActionResult> OnPost()
    {
        // Get the value of the clicked button from the Request.Form collection
        string buttonValue = Request.Form["submitButton"];
        var familyMember = await _familyMemberService.GetByID(Id);
        // Handle each button based on its value
        if (buttonValue == "accept")
        {
            familyMember.Status = true;
            await _familyMemberService.Update(familyMember);
            // Handle the "Save" button click
            // Perform the save action here
        }
        else if (buttonValue == "reject")
        {
            //tree.FirstName = tree.FirstName.Substring(0) + " (Rejected)";
            //await _familyTreeService.Update(tree);
            await _familyMemberService.Delete(familyMember);

        }
        if (_familyTreeService != null)
        {
            FamilyMembers = await _familyMemberService.Get(filter: t => t.Status == false
            , includeProperties: i => i.Member);
        }
        // If no button value matches, return to the same page or perform appropriate action
        return Page();
    }
}
