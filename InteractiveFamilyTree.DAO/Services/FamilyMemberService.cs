using InteractiveFamilyTree.DAO.IRepositories;
using InteractiveFamilyTree.DAO.IServices;
using InteractiveFamilyTree.DAO.Repositories;
using InteractiveFamilyTree.DTO.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveFamilyTree.DAO.Services;

public class FamilyMemberService : IFamilyMemberService
{
    private IFamilyMemberRepo _familyMemberRepo;

    public FamilyMemberService(IFamilyMemberRepo familyMemberRepo)
    {
        this._familyMemberRepo = familyMemberRepo;
    }

    public async Task AddAsync(FamilyMember entity)
    {
        //var list = await _familyMemberRepo.Get();
        //entity.Id = list.Count() + 1;
        await _familyMemberRepo.AddAsync(entity);
        await _familyMemberRepo.SaveChangesAsync();
    }

    public async Task AddRangeAsync(List<FamilyMember> entities)
    {
        await _familyMemberRepo.AddRangeAsync(entities);
        await _familyMemberRepo.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var status = await _familyMemberRepo.Delete(id);
        await _familyMemberRepo.SaveChangesAsync();
        return status;
    }

    public async Task<bool> Delete(FamilyMember entityToDelete)
    {
        var status = _familyMemberRepo.Delete(entityToDelete);
        await _familyMemberRepo.SaveChangesAsync();
        return status;
    }

    public async Task<List<FamilyMember>> Get(Expression<Func<FamilyMember, bool>> filter = null
        , Func<IQueryable<FamilyMember>, IOrderedQueryable<FamilyMember>> orderBy = null
        , params Expression<Func<FamilyMember, object>>[] includeProperties)
    {
        var entities = await _familyMemberRepo
            .Get(filter, orderBy, includeProperties);
        return entities.ToList();
    }

    public Task<FamilyMember> GetByID(int id
        , params Expression<Func<FamilyMember, object>>[] includeProperties)
    {
        return _familyMemberRepo.GetByID(id, includeProperties);
    }

    public async Task Update(FamilyMember entityToUpdate)
    {

        _familyMemberRepo.Update(entityToUpdate);
        await _familyMemberRepo.SaveChangesAsync();
    }
}
