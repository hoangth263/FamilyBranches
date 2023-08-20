using System;
using System.Collections.Generic;

namespace InteractiveFamilyTree.DTO.Models;

public partial class ChildAndParentsRelationShip
{
    public int Id { get; set; }

    public int ParentId { get; set; }

    public int ChildId { get; set; }

    public virtual FamilyMember? Child { get; set; }

    public virtual FamilyMember? Parent { get; set; }
}
