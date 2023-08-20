using System;
using System.Collections.Generic;

namespace InteractiveFamilyTree.DTO.Models;

public partial class FamilyMember
{
    public int Id { get; set; }

    public int TreeId { get; set; }

    public int? MemberId { get; set; }

    public string FullName { get; set; } = null!;

    public bool Gender { get; set; }

    public int Generation { get; set; }

    public DateTime Birthday { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public string Role { get; set; }

    public bool Status { get; set; }

    public bool StatusHealth { get; set; }

    public virtual ICollection<Career> Careers { get; set; } = new List<Career>();

    public virtual ICollection<ChildAndParentsRelationShip> ChildAndParentsRelationShipChildren { get; set; } = new List<ChildAndParentsRelationShip>();

    public virtual ICollection<ChildAndParentsRelationShip> ChildAndParentsRelationShipParents { get; set; } = new List<ChildAndParentsRelationShip>();

    public virtual ICollection<CoupleRelationship> CoupleRelationshipHusbands { get; set; } = new List<CoupleRelationship>();

    public virtual ICollection<CoupleRelationship> CoupleRelationshipWives { get; set; } = new List<CoupleRelationship>();

    public virtual ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();

    public virtual Member? Member { get; set; }

    public virtual FamilyTree? Tree { get; set; }
}
