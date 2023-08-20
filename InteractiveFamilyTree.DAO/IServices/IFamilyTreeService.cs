using InteractiveFamilyTree.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveFamilyTree.DAO.IServices;
public interface IFamilyTreeService
{
    Task<List<FamilyTree>> Get(Expression<Func<FamilyTree, bool>> filter = null,
Func<IQueryable<FamilyTree>, IOrderedQueryable<FamilyTree>> orderBy = null,
params Expression<Func<FamilyTree, object>>[] includeProperties);
    Task<FamilyTree> GetByID(int id
        , params Expression<Func<FamilyTree, object>>[] includeProperties);
    Task<FamilyTree> AddAsync(FamilyTree entity);
    Task AddRangeAsync(List<FamilyTree> entities);
    Task<bool> Delete(int id);
    Task<bool> Delete(FamilyTree entityToDelete);
    Task Update(FamilyTree entityToUpdate);
}
