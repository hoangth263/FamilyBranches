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

public class FamilyEventService : IFamilyEventService
{
    private IFamilyEventRepo _familyEventRepo;

    public FamilyEventService(IFamilyEventRepo familyEventRepo)
    {
        this._familyEventRepo = familyEventRepo;
    }

    public async Task AddAsync(FamilyEvent entity)
    {
        await _familyEventRepo.AddAsync(entity);
        await _familyEventRepo.SaveChangesAsync();
    }

    public async Task AddRangeAsync(List<FamilyEvent> entities)
    {
        await _familyEventRepo.AddRangeAsync(entities);
        await _familyEventRepo.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var status = await _familyEventRepo.Delete(id);
        await _familyEventRepo.SaveChangesAsync();
        return status;
    }

    public async Task<bool> Delete(FamilyEvent entityToDelete)
    {
        var status = _familyEventRepo.Delete(entityToDelete);
        await _familyEventRepo.SaveChangesAsync();
        return status;
    }

    public async Task<List<FamilyEvent>> Get(Expression<Func<FamilyEvent, bool>> filter = null
        , Func<IQueryable<FamilyEvent>, IOrderedQueryable<FamilyEvent>> orderBy = null
        , params Expression<Func<FamilyEvent, object>>[] includeProperties)
    {
        var entities = await _familyEventRepo
            .Get(filter, orderBy, includeProperties);
        return entities.ToList();
    }

    public Task<FamilyEvent> GetByID(int id
        , params Expression<Func<FamilyEvent, object>>[] includeProperties)
    {
        return _familyEventRepo.GetByID(id, includeProperties);
    }

    public async Task Update(FamilyEvent entityToUpdate)
    {

        _familyEventRepo.Update(entityToUpdate);
        await _familyEventRepo.SaveChangesAsync();
    }
}
