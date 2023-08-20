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

namespace InteractiveFamilyTree.RazorPage.Pages
{
    [Authorize(Roles = "member,admin,manager")]
    public class UserEventModel : PageModel
    {
        private readonly IFamilyEventService _service;
        private readonly IFamilyMemberService _familyMemberService;
        private readonly IEventParticipantService _eventParticipantService;
        private readonly IConfiguration Configuration;

        public UserEventModel(IFamilyEventService service, IFamilyMemberService familyMemberService, IConfiguration configuration, IEventParticipantService eventParticipantService)
        {
            _service = service;
            _familyMemberService = familyMemberService;
            Configuration = configuration;
            _eventParticipantService = eventParticipantService;
        }

        public PaginatedList<EventNotify> Notifies { get; set; } = default!;

        public string? Search { get; set; } = null;

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string StatusSort { get; set; }
        public string CurrentFilter { get; set; }
        public string FilterType { get; set; }
        public string CurrentSort { get; set; }

        public string Role { get; set; } = null;

        public class EventNotify
        {
            public int EventId { get; set; }
            public string EventName { get; set; }
            public DateTime Date { get; set; }
            public bool Status { get; set; }
            public string MemberStatus { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            int treeId = HttpContext.Session.GetIntFromSession("treeId");
            var member = await CheckRole(treeId);

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
            var eventList = await _service.Get(e => e.TreeId == treeId);
            var eventJoinedList = await _eventParticipantService.Get(p => p.FamilyMemberId == member.Id);
            List<EventNotify>? listTemp = new List<EventNotify>();
            foreach(var eventJoined in eventJoinedList)
            {
                foreach (var events in eventList)
                {
                    if (eventJoined.EventId == events.Id)
                    {
                        var notify = new EventNotify
                        {
                            EventId = events.Id,
                            EventName = events.Name,
                            Date = events.Date,
                            Status = events.Status,
                            MemberStatus = eventJoined.Status

                        };
                        listTemp.Add(notify);
                    }
                }
            }

            IQueryable<EventNotify> list = listTemp.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                list = list.Where(s => s.EventName.ToLower().Contains(searchString.ToLower()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    list = list.OrderByDescending(c => c.EventName);
                    break;
                case "Date":
                    list = list.OrderBy(c => c.Date);
                    break;
                case "date_desc":
                    list = list.OrderByDescending(c => c.Date);
                    break;
                case "status_desc":
                    list = list.OrderByDescending(c => c.Status);
                    break;
                case "status":
                    list = list.OrderBy(c => c.Status);
                    break;
                default:
                    list = list.OrderBy(c => c.EventName);
                    break;
            }
            var pageSize = Configuration.GetValue("PageSize", 4);
            Notifies = await PaginatedList<EventNotify>.CreateAsync(
                list.AsNoTracking(), pageIndex ?? 1, pageSize);
            return Page();
        }

        public async Task<FamilyMember> CheckRole(int treeId)
        {
            int id = int.Parse(((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Sid).Value);
            var check = await _familyMemberService.Get(f => f.MemberId == id && f.TreeId == treeId);
            if (check != null && check.Count() > 0)
            {
                Role = check.FirstOrDefault().Role;
                return check.FirstOrDefault();
            }
            return null;
        }

        public async Task<IActionResult> OnGetAccept(int eventId)
        {
            int treeId = SessionHelper.GetIntFromSession(HttpContext.Session, "treeId");
            var member = await CheckRole(treeId);

            if (Role == null || Role == "") return RedirectToPage("/Index");

            if (member.Id != 0)
            {
                var participant = (await _eventParticipantService.Get(p => p.FamilyMemberId == member.Id && p.EventId == eventId)).FirstOrDefault();
                if (participant != null)
                {
                    participant.Status = "Confirmed";
                    await _eventParticipantService.Update(participant);
                }
            }
            return RedirectToAction(nameof(OnGetAsync));
        }

        public async Task<IActionResult> OnGetDelete(int eventId)
        {
            int treeId = SessionHelper.GetIntFromSession(HttpContext.Session, "treeId");
            var member = await CheckRole(treeId);

            if (Role == null || Role == "") return RedirectToPage("/Index");
            var participant = (await _eventParticipantService.Get(p => p.FamilyMemberId == member.Id && p.EventId == eventId)).FirstOrDefault();

            if (participant.Id != 0)
            {
                await _eventParticipantService.Delete(participant.Id);
            }
            return RedirectToAction(nameof(OnGetAsync));
        }
    }
}
