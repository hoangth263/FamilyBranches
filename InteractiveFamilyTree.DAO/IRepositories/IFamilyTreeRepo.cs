using InteractiveFamilyTree.DTO.Models;
using InteractiveFamilyTree.DAO.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveFamilyTree.DAO.IRepositories;

public interface IFamilyTreeRepo : IGenericRepository<FamilyTree, int>
{
}
