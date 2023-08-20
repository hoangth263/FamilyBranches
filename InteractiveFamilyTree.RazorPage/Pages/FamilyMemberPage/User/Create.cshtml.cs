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
using System.Security.Claims;

namespace InteractiveFamilyTree.RazorPage.Pages.FamilyMemberPage.User
{
    [Authorize(Roles = "member")]
    public class CreateModel : PageModel
    {
        private readonly IFamilyMemberService _familyMemberService;
        private readonly IMemberService _memberService;
        private readonly IFamilyTreeService _familyTreeService;

        public CreateModel(IFamilyMemberService familyMemberService
            , IMemberService memberService
            , IFamilyTreeService familyTreeService)
        {
            _familyMemberService = familyMemberService;
            _memberService = memberService;
            _familyTreeService = familyTreeService;
        }



        public async Task<IActionResult> OnGet()
        {
            return Page();
        }

        [BindProperty]
        public string Code{ get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            int id = int.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value);
            var familyMember = await _familyMemberService.Get(filter: m => m.MemberId == id);
            var member = await _memberService.GetByID(id);
            if (familyMember.Count>0)
            {
                TempData["Message"] = "You have been a part of a tree already";
                return Page();
            }
            else
            {
                await _familyMemberService.AddAsync(new FamilyMember
                {
                    TreeId = DecodeInt(Code),
                    MemberId = id,
                    FullName = member.FullName,
                    Gender = member.Gender,
                    Generation = 1,
                    Birthday = member.Birthday,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Role = "member",
                    Status = false,
                    StatusHealth = true
                });
            }

            

            return RedirectToPage("/Index");
        }
        public static int DecodeInt(string encodedString)
        {
            IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
            var EncodingKey = config["SecretKey"];
            // Convert the Base64 string back to a byte array
            byte[] bytes = Convert.FromBase64String(encodedString);

            // Apply XOR operation with the encoding key to reverse the encoding
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ EncodingKey[i % EncodingKey.Length]);
            }

            // Convert the byte array back to an integer
            return BitConverter.ToInt32(bytes, 0);
        }

    }
}
