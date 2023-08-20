using InteractiveFamilyTree.DAO.IRepositories;
using InteractiveFamilyTree.DAO.IServices;
using InteractiveFamilyTree.DTO.Models;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveFamilyTree.DAO.Services;

public class MemberService : IMemberService, IDisposable
{
    private IMemberRepo _memberRepo;
    private InteractiveFamilyTreeOfficalContext _context;

    public MemberService(IMemberRepo memberRepo, InteractiveFamilyTreeOfficalContext context)
    {
        this._memberRepo = memberRepo;
        _context = context;
    }

    public async Task AddAsync(Member entity)
    {
        var list = await _memberRepo.Get();
        entity.Id= list.Count() + 1;
        while ((await Get(m => m.Id == entity.Id)).Count() > 0)
        {
            entity.Id++;
        }
        await _memberRepo.AddAsync(entity);
        await _memberRepo.SaveChangesAsync();
    }

    public async Task<Member> CheckEmail(string email)
    {
        var list = await _memberRepo.Get(m => m.Email.Equals(email));
        if (list.Count() > 0)
        {
            var m = list.FirstOrDefault();
            return m;
        }
        //_context.Entry(list).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        return null;
    }

    public async Task<Member> CheckPhone(string phone)
    {
        var list = await _memberRepo.Get(m => m.Phone.Equals(phone));
        if (list.Count() > 0)
        {
            var m = list.FirstOrDefault();
            return m;
        }
        //_context.Entry(list).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        return null;
    }

    public async Task AddRangeAsync(List<Member> entities)
    {
        await _memberRepo.AddRangeAsync(entities);
        await _memberRepo.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var status = await _memberRepo.Delete(id);
        await _memberRepo.SaveChangesAsync();
        return status;
    }

    public async Task<bool> Delete(Member entityToDelete)
    {
        var status = _memberRepo.Delete(entityToDelete);
        await _memberRepo.SaveChangesAsync();
        return status;
    }

    public async Task<Member> CheckLogin(string email, string password)
    {
        if (email == null || password == null) return null;

        var list = await _memberRepo.Get(m => m.Email.Equals(email) && m.Password.Equals(password));

        if (list.Count() > 0 )
        {
            var m = list.FirstOrDefault();
            return m;
        }

        return null;
    }

    public async Task<List<Member>> Get(Expression<Func<Member, bool>> filter = null
        , Func<IQueryable<Member>, IOrderedQueryable<Member>> orderBy = null
        , params Expression<Func<Member, object>>[] includeProperties)
    {
        var entities = await _memberRepo
            .Get(filter, orderBy, includeProperties);
        return entities.ToList();
    }

    public Task<Member> GetByID(int id
        , params Expression<Func<Member, object>>[] includeProperties)
    {
        return _memberRepo.GetByID(id, includeProperties);
    }

    public async Task Update(Member entityToUpdate)
    {
        _memberRepo.Update(entityToUpdate);
        await _memberRepo.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this.disposed = true;
    }
}
