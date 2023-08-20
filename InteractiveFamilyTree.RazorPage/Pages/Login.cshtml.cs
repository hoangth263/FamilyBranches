using InteractiveFamilyTree.DAO.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace InteractiveFamilyTree.RazorPage.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IFamilyMemberService _familyMemberService;
        private readonly IMemberService _service;
        private readonly IConfiguration _configuration;

        public LoginModel(IMemberService service, IFamilyMemberService familyMemberService)
        {
            _familyMemberService = familyMemberService;
            _service = service;
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        [BindProperty]
        public string Email { get; set; } = null;

        [BindProperty]
        public string Password { get; set; } = null;

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }
        public IActionResult OnGetLogout()
        {
            HttpContext.SignOutAsync("MyCookieAuth");
            // Clear all session data
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }

        public IActionResult OnGetRegister()
        {
            return RedirectToPage("/Register");
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var adEmail = config["DefaultAccount:Email"];
            var adPwd = config["DefaultAccount:Password"];
            var member = await _service.CheckLogin(Email, Password);
            // code to check username and password
            if (Email == adEmail && Password == adPwd)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, "admin")
                };
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync("MyCookieAuth", principal);
                return RedirectToPage("/Admin/Index");
            }

            if (member != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, member.Id.ToString()),
                    new Claim(ClaimTypes.Name, member.FullName),
                    new Claim(ClaimTypes.Role, "member")
                };
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync("MyCookieAuth", principal);
                var tree = await _familyMemberService.Get(filter: e => e.MemberId == member.Id);
                if (tree.Count > 0)
                {
                    HttpContext.SignOutAsync("MyCookieAuth");
                    claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, member.Id.ToString()),
                    new Claim(ClaimTypes.Name, member.FullName),
                    new Claim(ClaimTypes.Role, tree.FirstOrDefault().Role)
                };
                    identity = new ClaimsIdentity(claims, "MyCookieAuth");
                    principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync("MyCookieAuth", principal);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "treeId", tree.FirstOrDefault().TreeId);
                }
                return RedirectToPage("/Index");
            }
            else if (IsValidLoginAdmin(Email, Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Admin"),
                    new Claim(ClaimTypes.Role, "admin")
                };
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync("MyCookieAuth", principal);
                return RedirectToPage("/Index");
            }
            else
            {
                TempData["Message"] = "Invalid username or password.";
                return Page();
            }
        }

        private bool IsValidLoginAdmin(string email, string password)
        {
            string adminEmail = _configuration.GetValue<string>("AdminCredentials:Email");
            string adminPassword = _configuration.GetValue<string>("AdminCredentials:Password");

            return (email == adminEmail && password == adminPassword);
        }
    }
}
