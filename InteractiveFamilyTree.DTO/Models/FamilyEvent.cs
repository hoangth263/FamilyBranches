using System;
using System.Collections.Generic;

namespace InteractiveFamilyTree.DTO.Models;

public partial class FamilyEvent
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int TreeId { get; set; }

    public bool Type { get; set; }

    public DateTime Date { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();

    public virtual FamilyTree? Tree { get; set; }
}
