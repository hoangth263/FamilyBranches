using InteractiveFamilyTree.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveFamilyTree.DAO.IServices;
public interface IMemberService
{
    Task<List<Member>> Get(Expression<Func<Member, bool>> filter = null,
Func<IQueryable<Member>, IOrderedQueryable<Member>> orderBy = null,
params Expression<Func<Member, object>>[] includeProperties);
    Task<Member> CheckLogin(string email, string password);
    Task<Member> CheckEmail(string email);
    Task<Member> CheckPhone(string phone);
    Task<Member> GetByID(int id
        , params Expression<Func<Member, object>>[] includeProperties);
    Task AddAsync(Member entity);
    Task AddRangeAsync(List<Member> entities);
    Task<bool> Delete(int id);
    Task<bool> Delete(Member entityToDelete);
    Task Update(Member entityToUpdate);
    void Dispose();
}
