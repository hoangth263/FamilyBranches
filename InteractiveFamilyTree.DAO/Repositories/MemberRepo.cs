using InteractiveFamilyTree.DAO.IRepositories;
using InteractiveFamilyTree.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveFamilyTree.DAO.Repositories;

public class MemberRepo : GenericRepository<Member, int>, IMemberRepo
{
    public MemberRepo(InteractiveFamilyTreeOfficalContext context) : base(context)
    {
    }
}
