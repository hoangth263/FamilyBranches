using InteractiveFamilyTree.DAO.IRepositories;
using InteractiveFamilyTree.DAO.IServices;
using InteractiveFamilyTree.DAO.Repositories;
using InteractiveFamilyTree.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveFamilyTree.DAO.Services;

public class FamilyTreeService : IFamilyTreeService
{
    private IFamilyTreeRepo _familyTreeRepo;

    public FamilyTreeService(IFamilyTreeRepo familyTreeRepo)
    {
        this._familyTreeRepo = familyTreeRepo;
    }

    public async Task<FamilyTree> AddAsync(FamilyTree entity)
    {
        //var list = await _familyTreeRepo.Get();
        //entity.Id = list.Count() + 1;
        if(await IsExisted(entity.ManagerId))
        {
            return null;
        }
        await _familyTreeRepo.AddAsync(entity);
        await _familyTreeRepo.SaveChangesAsync();
        return entity;
    }

    public async Task AddRangeAsync(List<FamilyTree> entities)
    {
        await _familyTreeRepo.AddRangeAsync(entities);
        await _familyTreeRepo.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var status = await _familyTreeRepo.Delete(id);
        await _familyTreeRepo.SaveChangesAsync();
        return status;
    }

    public async Task<bool> Delete(FamilyTree entityToDelete)
    {
        var status = _familyTreeRepo.Delete(entityToDelete);
        await _familyTreeRepo.SaveChangesAsync();
        return status;
    }

    public async Task<List<FamilyTree>> Get(Expression<Func<FamilyTree, bool>> filter = null
        , Func<IQueryable<FamilyTree>, IOrderedQueryable<FamilyTree>> orderBy = null
        , params Expression<Func<FamilyTree, object>>[] includeProperties)
    {
        var entities = await _familyTreeRepo
            .Get(filter, orderBy, includeProperties);
        return entities.ToList();
    }

    public Task<FamilyTree> GetByID(int id
        , params Expression<Func<FamilyTree, object>>[] includeProperties)
    {
        return _familyTreeRepo.GetByID(id, includeProperties);
    }

    public async Task Update(FamilyTree entityToUpdate)
    {

        _familyTreeRepo.Update(entityToUpdate);
        await _familyTreeRepo.SaveChangesAsync();
    }
    public async Task<bool> IsExisted(int memberId)
    {
        var tree = await _familyTreeRepo.Get(filter: t => t.ManagerId == memberId);
        return tree.Count() > 0;
    }
}
