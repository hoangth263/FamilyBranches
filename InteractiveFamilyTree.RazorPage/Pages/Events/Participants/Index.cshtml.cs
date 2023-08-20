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
using System.Linq.Expressions;

namespace InteractiveFamilyTree.RazorPage.Pages.Events.Participants
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly IEventParticipantService _service;
        private readonly IFamilyMemberService _familyMemberService;

        public IndexModel(IEventParticipantService service, IFamilyMemberService familyMemberService)
        {
            _service = service;
            _familyMemberService = familyMemberService;
        }

        public IList<EventParticipant> EventParticipant { get;set; } = default!;



        public async Task OnGetAsync(int? id, int? treeId)
        {
            if (_service != null)
            {
                EventParticipant = await _service.Get(p => p.EventId == id,
                    includeProperties: new Expression<Func<EventParticipant, object>>[]
                        {
                            f => f.Event,
                            f => f.FamilyMember
                        });
            }
        }
    }
}
