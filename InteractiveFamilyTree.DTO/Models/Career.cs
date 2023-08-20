using System;
using System.Collections.Generic;

namespace InteractiveFamilyTree.DTO.Models;

public partial class Career
{
    public int Id { get; set; }

    public int? FamilyMemberId { get; set; }

    public string Detail { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool Status { get; set; }

    public virtual FamilyMember? FamilyMember { get; set; }
}
