using InteractiveFamilyTree.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveFamilyTree.DAO.IServices;
public interface IFamilyMemberService
{
    Task<List<FamilyMember>> Get(Expression<Func<FamilyMember, bool>> filter = null,
Func<IQueryable<FamilyMember>, IOrderedQueryable<FamilyMember>> orderBy = null,
params Expression<Func<FamilyMember, object>>[] includeProperties);
    Task<FamilyMember> GetByID(int id
        , params Expression<Func<FamilyMember, object>>[] includeProperties);
    Task AddAsync(FamilyMember entity);
    Task AddRangeAsync(List<FamilyMember> entities);
    Task<bool> Delete(int id);
    Task<bool> Delete(FamilyMember entityToDelete);
    Task Update(FamilyMember entityToUpdate);
}
