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

public class CareerService : ICareerService
{
    private ICareerRepo _careerRepo;

    public CareerService(ICareerRepo careerRepo)
    {
        this._careerRepo = careerRepo;
    }

    public async Task AddAsync(Career entity)
    {
        await _careerRepo.AddAsync(entity);
        await _careerRepo.SaveChangesAsync();
    }

    public async Task AddRangeAsync(List<Career> entities)
    {
        await _careerRepo.AddRangeAsync(entities);
        await _careerRepo.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var status = await _careerRepo.Delete(id);
        await _careerRepo.SaveChangesAsync();
        return status;
    }

    public async Task<bool> Delete(Career entityToDelete)
    {
        var status = _careerRepo.Delete(entityToDelete);
        await _careerRepo.SaveChangesAsync();
        return status;
    }

    public async Task<List<Career>> Get(Expression<Func<Career, bool>> filter = null
        , Func<IQueryable<Career>, IOrderedQueryable<Career>> orderBy = null
        , params Expression<Func<Career, object>>[] includeProperties)
    {
        var entities = await _careerRepo
            .Get(filter, orderBy, includeProperties);
        return entities.ToList();
    }

    public Task<Career> GetByID(int id
        , params Expression<Func<Career, object>>[] includeProperties)
    {
        return _careerRepo.GetByID(id, includeProperties);
    }

    public async Task Update(Career entityToUpdate)
    {

        _careerRepo.Update(entityToUpdate);
        await _careerRepo.SaveChangesAsync();
    }
}
