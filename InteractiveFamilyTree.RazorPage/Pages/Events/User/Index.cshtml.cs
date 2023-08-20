using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InteractiveFamilyTree.DTO.Models;
using InteractiveFamilyTree.DAO.IServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using InteractiveFamilyTree.DAO.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using InteractiveFamilyTree.RazorPage.Paging;

namespace InteractiveFamilyTree.RazorPage.Pages.Events.User
{
    [Authorize(Roles = "member,admin,manager")]
    public class IndexModel : PageModel
    {
        private readonly IFamilyEventService _service;
        private readonly IFamilyMemberService _familyMemberService;
        private readonly IConfiguration Configuration;

        public IndexModel(IFamilyEventService service, IFamilyMemberService familyMemberService, IConfiguration configuration)
        {
            _service = service;
            _familyMemberService = familyMemberService;
            Configuration = configuration;
        }

        public PaginatedList<FamilyEvent> FamilyEvent { get; set; } = default!;

        public string? Search { get; set; } = null;

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string StatusSort { get; set; }
        public string CurrentFilter { get; set; }
        public string FilterType { get; set; }
        public string CurrentSort { get; set; }

        public string Role { get; set; } = null;

        public async Task<IActionResult> OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            int treeId = SessionHelper.GetIntFromSession(HttpContext.Session, "treeId");
            await CheckRole(treeId);

            CurrentSort = sortOrder;
            NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            StatusSort = sortOrder == "status" ? "status_desc" : "status";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            CurrentFilter = searchString;

            if (Role == null || Role == "") return RedirectToPage("/index");
            if (treeId == 0)
            {
                return RedirectToPage("/index");
            }

            //if (search != null)
            //{
            //    FamilyEvent = await _service.Get(e => e.TreeId == treeId && e.Name.ToLower().Contains(search.ToLower()),
            //        orderBy: e => e.OrderByDescending(e => e.Status).ThenBy(e => e.Date).ThenBy(e => e.Type),
            //        includeProperties: e => e.Tree);
            //}
            //else
            //{
            //    FamilyEvent = await _service.Get(e => e.TreeId == treeId,
            //        orderBy: e => e.OrderByDescending(e => e.Status).ThenBy(e => e.Date).ThenBy(e => e.Type),
            //        includeProperties: e => e.Tree);
            //}
            IQueryable<FamilyEvent> eventList =(await _service.Get(e => e.TreeId == treeId)).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                eventList = eventList.Where(s => s.Name.ToLower().Contains(searchString.ToLower()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    eventList = eventList.OrderByDescending(c => c.Name);
                    break;
                case "Date":
                    eventList = eventList.OrderBy(c => c.Date);
                    break;
                case "date_desc":
                    eventList = eventList.OrderByDescending(c => c.Date);
                    break;
                case "status_desc":
                    eventList = eventList.OrderByDescending(c => c.Status);
                    break;
                case "status":
                    eventList = eventList.OrderBy(c => c.Status);
                    break;
                default:
                    eventList = eventList.OrderBy(c => c.Name);
                    break;
            }
            var pageSize = Configuration.GetValue("PageSize", 4);
            FamilyEvent = await PaginatedList<FamilyEvent>.CreateAsync(
                eventList.AsNoTracking(), pageIndex ?? 1, pageSize);
            return Page();
        }

        public async Task CheckRole(int treeId)
        {
            int id = int.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value);
            var check = await _familyMemberService.Get(f => f.MemberId == id && f.TreeId == treeId);
            if (check != null && check.Count() > 0)
            {
                Role = check.FirstOrDefault().Role;
            }
            return;
        }
    }
}
