using System.Linq.Expressions;

namespace ImageManager.Repository
{
	public interface IRepository<T, TKey>
	{
		Task<T> GetAsync(TKey id);
		Task DeleteAsync(TKey id);
		Task<TKey> CreateAsync(T entity);
		Task UpdateAsync(T entity);
		Task<IEnumerable<T>> GetAllAsync();
		Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
	}
}
