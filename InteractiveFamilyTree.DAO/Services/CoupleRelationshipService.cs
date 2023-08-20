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

public class CoupleRelationshipService : ICoupleRelationshipService
{
    private ICoupleRelationshipRepo _CoupleRelationshipRepo;

    public CoupleRelationshipService(ICoupleRelationshipRepo CoupleRelationshipRepo)
    {
        this._CoupleRelationshipRepo = CoupleRelationshipRepo;
    }

    public async Task AddAsync(CoupleRelationship entity)
    {
        await _CoupleRelationshipRepo.AddAsync(entity);
        await _CoupleRelationshipRepo.SaveChangesAsync();
    }

    public async Task AddRangeAsync(List<CoupleRelationship> entities)
    {
        await _CoupleRelationshipRepo.AddRangeAsync(entities);
        await _CoupleRelationshipRepo.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var status = await _CoupleRelationshipRepo.Delete(id);
        await _CoupleRelationshipRepo.SaveChangesAsync();
        return status;
    }

    public async Task<bool> Delete(CoupleRelationship entityToDelete)
    {
        var status = _CoupleRelationshipRepo.Delete(entityToDelete);
        await _CoupleRelationshipRepo.SaveChangesAsync();
        return status;
    }

    public async Task<List<CoupleRelationship>> Get(Expression<Func<CoupleRelationship, bool>> filter = null
        , Func<IQueryable<CoupleRelationship>, IOrderedQueryable<CoupleRelationship>> orderBy = null
        , params Expression<Func<CoupleRelationship, object>>[] includeProperties)
    {
        var entities = await _CoupleRelationshipRepo
            .Get(filter, orderBy, includeProperties);
        return entities.ToList();
    }

    public Task<CoupleRelationship> GetByID(int id
        , params Expression<Func<CoupleRelationship, object>>[] includeProperties)
    {
        return _CoupleRelationshipRepo.GetByID(id, includeProperties);
    }

    public async Task Update(CoupleRelationship entityToUpdate)
    {

        _CoupleRelationshipRepo.Update(entityToUpdate);
        await _CoupleRelationshipRepo.SaveChangesAsync();
    }
}
