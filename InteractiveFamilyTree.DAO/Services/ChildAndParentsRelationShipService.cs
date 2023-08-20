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

public class ChildAndParentsRelationShipService : IChildAndParentsRelationShipService
{
    private IChildAndParentsRelationShipRepo __ChildAndParentsRelationShipRepo;

    public ChildAndParentsRelationShipService(IChildAndParentsRelationShipRepo _ChildAndParentsRelationShipRepo)
    {
        this.__ChildAndParentsRelationShipRepo = _ChildAndParentsRelationShipRepo;
    }

    public async Task AddAsync(ChildAndParentsRelationShip entity)
    {
        await __ChildAndParentsRelationShipRepo.AddAsync(entity);
        await __ChildAndParentsRelationShipRepo.SaveChangesAsync();
    }

    public async Task AddRangeAsync(List<ChildAndParentsRelationShip> entities)
    {
        await __ChildAndParentsRelationShipRepo.AddRangeAsync(entities);
        await __ChildAndParentsRelationShipRepo.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var status = await __ChildAndParentsRelationShipRepo.Delete(id);
        await __ChildAndParentsRelationShipRepo.SaveChangesAsync();
        return status;
    }

    public async Task<bool> Delete(ChildAndParentsRelationShip entityToDelete)
    {
        var status = __ChildAndParentsRelationShipRepo.Delete(entityToDelete);
        await __ChildAndParentsRelationShipRepo.SaveChangesAsync();
        return status;
    }

    public async Task<List<ChildAndParentsRelationShip>> Get(Expression<Func<ChildAndParentsRelationShip, bool>> filter = null
        , Func<IQueryable<ChildAndParentsRelationShip>, IOrderedQueryable<ChildAndParentsRelationShip>> orderBy = null
        , params Expression<Func<ChildAndParentsRelationShip, object>>[] includeProperties)
    {
        var entities = await __ChildAndParentsRelationShipRepo
            .Get(filter, orderBy, includeProperties);
        return entities.ToList();
    }

    public Task<ChildAndParentsRelationShip> GetByID(int id
        , params Expression<Func<ChildAndParentsRelationShip, object>>[] includeProperties)
    {
        return __ChildAndParentsRelationShipRepo.GetByID(id, includeProperties);
    }

    public async Task Update(ChildAndParentsRelationShip entityToUpdate)
    {

        __ChildAndParentsRelationShipRepo.Update(entityToUpdate);
        await __ChildAndParentsRelationShipRepo.SaveChangesAsync();
    }
}
