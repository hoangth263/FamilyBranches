using InteractiveFamilyTree.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveFamilyTree.DAO.IServices;
public interface IEventParticipantService
{
    Task<List<EventParticipant>> Get(Expression<Func<EventParticipant, bool>> filter = null,
Func<IQueryable<EventParticipant>, IOrderedQueryable<EventParticipant>> orderBy = null,
params Expression<Func<EventParticipant, object>>[] includeProperties);
    Task<EventParticipant> GetByID(int id
        , params Expression<Func<EventParticipant, object>>[] includeProperties);
    Task AddAsync(EventParticipant entity);
    Task AddRangeAsync(List<EventParticipant> entities);
    Task<bool> Delete(int id);
    Task<bool> Delete(EventParticipant entityToDelete);
    Task Update(EventParticipant entityToUpdate);
}
