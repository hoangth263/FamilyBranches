using System;
using System.Collections.Generic;

namespace InteractiveFamilyTree.DTO.Models;

public partial class FamilyTree
{

    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime ModifyDate { get; set; }

    public int TotalGeneration { get; set; }
    public int ManagerId{ get; set; }

    public bool Status { get; set; }
    public virtual Member? Member { get; set; }

    public virtual ICollection<FamilyEvent> FamilyEvents { get; set; } 

    public virtual ICollection<FamilyMember> FamilyMembers { get; set; } 
    public FamilyTree()
    {
        FamilyEvents = new List<FamilyEvent>();
        FamilyMembers = new List<FamilyMember>();
    }


}
