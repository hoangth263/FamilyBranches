using System;
using System.Collections.Generic;

namespace InteractiveFamilyTree.DTO.Models;

public partial class EventParticipant
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public int FamilyMemberId { get; set; }

    public string Status { get; set; }

    public virtual FamilyEvent? Event { get; set; }

    public virtual FamilyMember? FamilyMember { get; set; }
}
