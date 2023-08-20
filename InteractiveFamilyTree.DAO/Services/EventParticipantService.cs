using InteractiveFamilyTree.DAO.IRepositories;
using InteractiveFamilyTree.DAO.IServices;
using InteractiveFamilyTree.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveFamilyTree.DAO.Services;

public class EventParticipantService : IEventParticipantService
{
    private IEventParticipantRepo _eventParticipantRepo;

    public EventParticipantService(IEventParticipantRepo eventParticipantRepo)
    {
        this._eventParticipantRepo = eventParticipantRepo;
    }

    public async Task AddAsync(EventParticipant entity)
    {
        await _eventParticipantRepo.AddAsync(entity);
        await _eventParticipantRepo.SaveChangesAsync();
    }

    public async Task AddRangeAsync(List<EventParticipant> entities)
    {
        await _eventParticipantRepo.AddRangeAsync(entities);
        await _eventParticipantRepo.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var status = await _eventParticipantRepo.Delete(id);
        await _eventParticipantRepo.SaveChangesAsync();
        return status;
    }

    public async Task<bool> Delete(EventParticipant entityToDelete)
    {
        var status = _eventParticipantRepo.Delete(entityToDelete);
        await _eventParticipantRepo.SaveChangesAsync();
        return status;
    }

    public async Task<List<EventParticipant>> Get(Expression<Func<EventParticipant, bool>> filter = null
        , Func<IQueryable<EventParticipant>, IOrderedQueryable<EventParticipant>> orderBy = null
        , params Expression<Func<EventParticipant, object>>[] includeProperties)
    {
        var entities = await _eventParticipantRepo
            .Get(filter, orderBy, includeProperties);
        return entities.ToList();
    }

    public Task<EventParticipant> GetByID(int id
        , params Expression<Func<EventParticipant, object>>[] includeProperties)
    {
        return _eventParticipantRepo.GetByID(id, includeProperties);
    }

    public async Task Update(EventParticipant entityToUpdate)
    {

        _eventParticipantRepo.Update(entityToUpdate);
        await _eventParticipantRepo.SaveChangesAsync();
    }
}
