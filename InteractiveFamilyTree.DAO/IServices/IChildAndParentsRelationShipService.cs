using InteractiveFamilyTree.DTO.Models;
using System.Linq.Expressions;

namespace InteractiveFamilyTree.DAO.IServices;
public interface IChildAndParentsRelationShipService
{
    Task<List<ChildAndParentsRelationShip>> Get(Expression<Func<ChildAndParentsRelationShip, bool>> filter = null,
Func<IQueryable<ChildAndParentsRelationShip>, IOrderedQueryable<ChildAndParentsRelationShip>> orderBy = null,
params Expression<Func<ChildAndParentsRelationShip, object>>[] includeProperties);
    Task<ChildAndParentsRelationShip> GetByID(int id
        , params Expression<Func<ChildAndParentsRelationShip, object>>[] includeProperties);
    Task AddAsync(ChildAndParentsRelationShip entity);
    Task AddRangeAsync(List<ChildAndParentsRelationShip> entities);
    Task<bool> Delete(int id);
    Task<bool> Delete(ChildAndParentsRelationShip entityToDelete);
    Task Update(ChildAndParentsRelationShip entityToUpdate);
}
