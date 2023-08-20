using System;
using System.Collections.Generic;

namespace InteractiveFamilyTree.DTO.Models;

public partial class CoupleRelationship
{
    public int Id { get; set; }

    public int HusbandId { get; set; }

    public int WifeId { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public string MarrageStatus { get; set; }

    public virtual FamilyMember? Husband { get; set; }

    public virtual FamilyMember? Wife { get; set; }
}
