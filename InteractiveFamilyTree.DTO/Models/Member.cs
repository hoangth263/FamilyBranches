using System;
using System.Collections.Generic;

namespace InteractiveFamilyTree.DTO.Models;

public partial class Member
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Image { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; } = null!;

    public bool Gender { get; set; }

    public DateTime Birthday { get; set; }

    public string Password { get; set; } = null!;

    public bool Status { get; set; }

    public virtual ICollection<FamilyMember> FamilyMembers { get; set; } = new List<FamilyMember>();
}
