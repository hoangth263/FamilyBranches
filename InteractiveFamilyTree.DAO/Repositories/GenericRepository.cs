using InteractiveFamilyTree.DTO.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InteractiveFamilyTree.DAO.Repositories;

public interface IGenericRepository<T, TKey> where T : class
{
    Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        params Expression<Func<T, object>>[] includeProperties);
    Task<T> GetByID(TKey id
        , params Expression<Func<T, object>>[] includeProperties);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task<bool> Delete(TKey id);
    bool Delete(T entityToDelete);
    void Update(T entityToUpdate);
    Task SaveChangesAsync();

}
public class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : class
{
    internal InteractiveFamilyTreeOfficalContext context;
    internal DbSet<T> dbSet;

    public GenericRepository(InteractiveFamilyTreeOfficalContext context)
    {
        this.context = context;
        dbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> Get(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
    }

    public virtual async Task<T> GetByID(TKey id
        , params Expression<Func<T, object>>[] includeProperties)
    {
        //var list = new List<T> { dbSet.Find(id) };
        var entity = await dbSet.FindAsync(id);
        IQueryable<T> query = dbSet;
        if (query != null)
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
                //await context.Entry(entity).Reference(includeProperty).LoadAsync();
            }
        }
        var list = query.ToList();
        return list.FirstOrDefault(
             e => e.GetType()
            .GetProperty("Id")
            .GetValue(e)
            .Equals(id));
    }

    public virtual async Task AddAsync(T entity)
    {
        await dbSet.AddAsync(entity);
    }
    public virtual async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await dbSet.AddRangeAsync(entities);
    }

    public virtual async Task<bool> Delete(TKey id)
    {
        T entityToDelete = await dbSet.FindAsync(id);
        if (entityToDelete == null)
        {
            return false;
        }
        return Delete(entityToDelete);
    }

    public virtual bool Delete(T entityToDelete)
    {
        if (context.Entry(entityToDelete).State == EntityState.Detached)
        {
            dbSet.Attach(entityToDelete);
        }
        dbSet.Remove(entityToDelete);
        return true;
    }

    public virtual void Update(T entityToUpdate)
    {
        dbSet.Attach(entityToUpdate);
        context.Entry(entityToUpdate).State = EntityState.Modified;
    }
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
