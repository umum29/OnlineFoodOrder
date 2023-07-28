using System.Linq.Expressions;
namespace OnlineOrderApi.Repository.Interface
{
  public interface IRepository<T> where T : class
  {
    Task<List<T>> GetAllAsync();
    Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracking = true);
    Task CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task RemoveAsync(T entity);
    Task SaveAsync();
  }
}