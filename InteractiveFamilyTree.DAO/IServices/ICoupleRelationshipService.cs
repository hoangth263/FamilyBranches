using InteractiveFamilyTree.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveFamilyTree.DAO.IServices;
public interface ICoupleRelationshipService
{
    Task<List<CoupleRelationship>> Get(Expression<Func<CoupleRelationship, bool>> filter = null,
Func<IQueryable<CoupleRelationship>, IOrderedQueryable<CoupleRelationship>> orderBy = null,
params Expression<Func<CoupleRelationship, object>>[] includeProperties);
    Task<CoupleRelationship> GetByID(int id
        , params Expression<Func<CoupleRelationship, object>>[] includeProperties);
    Task AddAsync(CoupleRelationship entity);
    Task AddRangeAsync(List<CoupleRelationship> entities);
    Task<bool> Delete(int id);
    Task<bool> Delete(CoupleRelationship entityToDelete);
    Task Update(CoupleRelationship entityToUpdate);
}
