using InteractiveFamilyTree.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveFamilyTree.DAO.IServices;
public interface IFamilyEventService
{
    Task<List<FamilyEvent>> Get(Expression<Func<FamilyEvent, bool>> filter = null,
Func<IQueryable<FamilyEvent>, IOrderedQueryable<FamilyEvent>> orderBy = null,
params Expression<Func<FamilyEvent, object>>[] includeProperties);
    Task<FamilyEvent> GetByID(int id
        , params Expression<Func<FamilyEvent, object>>[] includeProperties);
    Task AddAsync(FamilyEvent entity);
    Task AddRangeAsync(List<FamilyEvent> entities);
    Task<bool> Delete(int id);
    Task<bool> Delete(FamilyEvent entityToDelete);
    Task Update(FamilyEvent entityToUpdate);
}
