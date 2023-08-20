using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InteractiveFamilyTree.RazorPage.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            //SessionHelper.RemoveStringFromSession(HttpContext.Session, "treeId");
            ////test
            //SessionHelper.SetStringToSession(HttpContext.Session, "treeId", "1");
            return Page();
        }
    }
}