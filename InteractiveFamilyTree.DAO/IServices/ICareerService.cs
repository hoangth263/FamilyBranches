using InteractiveFamilyTree.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveFamilyTree.DAO.IServices;
public interface ICareerService
{
    Task<List<Career>> Get(Expression<Func<Career, bool>> filter = null,
    Func<IQueryable<Career>, IOrderedQueryable<Career>> orderBy = null,
    params Expression<Func<Career, object>>[] includeProperties);
    Task<Career> GetByID(int id
        , params Expression<Func<Career, object>>[] includeProperties);
    Task AddAsync(Career entity);
    Task AddRangeAsync(List<Career> entities);
    Task<bool> Delete(int id);
    Task<bool> Delete(Career entityToDelete);
    Task Update(Career entityToUpdate);
}
