using OnlineOrderApi.Repository.Interface;
using OnlineOrderApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace OnlineOrderApi.Repository
{
  public class Repository<T> : IRepository<T> where T : class
  {
    private readonly ApplicationDataContext _context;
    internal DbSet<T> dbSet;
    public Repository(ApplicationDataContext context)
    {
      _context = context;
      this.dbSet = _context.Set<T>();
    }

    public async Task CreateAsync(T entity)
    {
      await dbSet.AddAsync(entity);
      await SaveAsync();
    }
    public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracking = true)
    {
      IQueryable<T> query = dbSet;
      if (!tracking)
      {
        query = query.AsNoTracking();
      }
      if (filter != null)
      {
        query = query.Where(filter);
      }
      return await query.FirstOrDefaultAsync();
    }
    public async Task<List<T>> GetAllAsync()
    {
      return await dbSet.ToListAsync();
    }
    public async Task<T> UpdateAsync(T entity)
    {
      dbSet.Update(entity);
      await SaveAsync();
      return entity;
    }

    public async Task RemoveAsync(T entity)
    {
      dbSet.Remove(entity);
      await SaveAsync();
    }
    public async Task SaveAsync()
    {
      await _context.SaveChangesAsync();
    }
  }
}
