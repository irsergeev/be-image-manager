using ImageManager.NHibernate.Interfaces;
using ImageManager.Repository;
using NHibernate;
using NHibernate.Linq;
using System.Linq.Expressions;

namespace ImageManager.NHibernate
{
	public class NHibernateRepository<T, TKey> : IRepository<T, TKey> 
		where T : IIdentityEntity<TKey>
	{
		private readonly ISession _session;
		private ITransaction? _transaction = null;

		public NHibernateRepository(ISession session)
		{
			_session = session;
		}

		public async Task<TKey> CreateAsync(T entity)
		{
			TKey key = default!;

			await DoTransactionOperation(async () =>
			{
				await _session.SaveAsync(entity);
				await _session.FlushAsync();

				key = entity.Id;
			});

			return key;
		}

		public async Task UpdateAsync(T entity)
		{
			await DoTransactionOperation(async () =>
			{
				await _session.UpdateAsync(entity);

				/*session.Query<T>()
					.Where(filter)
					.UpdateBuilder()
					.Set(Setter)
					...
					.Update();
				 */
			});
		}

		public async Task DeleteAsync(TKey id)
		{
			await DoTransactionOperation(async () =>
			{
				var instance = await _session.GetAsync<T>(id);
				await _session.DeleteAsync(instance);
			});
		}

		public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
		{
			await DoTransactionOperation(async () =>
			{
				await _session.Query<T>().Where(predicate).DeleteAsync();
			});
		}

		public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
		{
			var result = await _session.Query<T>().Where(predicate).ToListAsync();
			return result;
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			var result = await _session.Query<T>().ToListAsync();
			return result;
		}

		public async Task<T> GetAsync(TKey id)
		{
			var instance = await _session.GetAsync<T>(id);
			return instance;
		}

		private async Task DoTransactionOperation(Func<Task> action)
		{
			_transaction = _session.BeginTransaction();

			try
			{
				await action();
				await _transaction.CommitAsync();
			}
			catch (Exception ex)
			{
				await _transaction?.RollbackAsync();
				throw new Exception(ex.Message, ex);
			}
			finally
			{
				_transaction?.Dispose();
				_transaction = null;
			}
		}
	}
}
