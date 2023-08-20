using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InteractiveFamilyTree.DTO.Models;
using InteractiveFamilyTree.DAO.IServices;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace InteractiveFamilyTree.RazorPage.Pages.FamilyMemberPage.User
{
    [Authorize(Roles = "member,manager")]
    public class IndexModel : PageModel
    {
        private readonly IFamilyMemberService _familyMemberService;

        public IndexModel(IFamilyMemberService familyMemberService)
        {
            _familyMemberService = familyMemberService;
        }

        public List<FamilyMember> FamilyMember { get; set; } = default!;

        public string TreeId { get; set; } = default!;
        public string Code;

        public async Task<IActionResult> OnGetAsync()
        {
            int id = int.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value);
            var member = await _familyMemberService.Get(f => f.MemberId == id);
            if (_familyMemberService != null && member != null && member.FirstOrDefault().Status)
            {
                TreeId = member.FirstOrDefault().TreeId.ToString();
                FamilyMember = await _familyMemberService
                    .Get(e => e.TreeId.Equals(member.FirstOrDefault().TreeId) && e.Status);
                SessionHelper.SetStringToSession(HttpContext.Session, "treeId", member.FirstOrDefault().TreeId.ToString());
                Code = EncodeInt(member.FirstOrDefault().TreeId);

                return Page();
            }
            return RedirectToPage("/Index");
        }

        public string EncodeInt(int number)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var EncodingKey = config["SecretKey"];
            // Convert the integer to a byte array
            byte[] bytes = BitConverter.GetBytes(number);

            // Apply XOR operation with the encoding key
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(bytes[i] ^ EncodingKey[i % EncodingKey.Length]);
            }

            // Convert the modified byte array to a Base64 string
            return Convert.ToBase64String(bytes);
        }
    }
}
