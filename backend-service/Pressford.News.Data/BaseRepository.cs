using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Pressford.News.Data
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
	{
		private readonly PressfordNewsContext _context;

		public Repository(PressfordNewsContext context)
		{
			_context = context;
		}

		public IQueryable<TEntity> GetAll()
		{
			try
			{
				return _context.Set<TEntity>();
			}
			catch (Exception)
			{
				throw new Exception("Couldn't retrieve entities");
			}
		}

		public async Task<TEntity> AddAsync(TEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
			}

			try
			{
				await _context.AddAsync(entity);
				await _context.SaveChangesAsync();

				return entity;
			}
			catch (Exception ex)
			{
				throw new Exception($"{nameof(entity)} could not be saved");
			}
		}

		public async Task<TEntity> UpdateAsync(TEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException($"{nameof(UpdateAsync)} entity must not be null");
			}

			try
			{
				_context.Update(entity);
				await _context.SaveChangesAsync();

				return entity;
			}
			catch (Exception)
			{
				throw new Exception($"{nameof(entity)} could not be updated");
			}
		}

		public async Task<bool> Delete<TUniqueType>(TUniqueType uniqueIdentifier)
		{
			TEntity entityToDelete = _context.Find<TEntity>(uniqueIdentifier);
			if (entityToDelete == null)
			{
				throw new InvalidOperationException($"{nameof(Delete)} entity not found");
			}

			try
			{
				_context.Remove(entityToDelete);
				await _context.SaveChangesAsync();

				return true;
			}
			catch (Exception)
			{
				throw new Exception($"{nameof(TEntity)} could not be deleted");
			}
		}

		public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
		{
			var query = _context.Set<TEntity>().Where(predicate);

			return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
		}
	}
}