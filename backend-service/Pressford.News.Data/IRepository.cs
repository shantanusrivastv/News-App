using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pressford.News.Data
{
	public interface IRepository<TEntity> where TEntity : class, new()
	{
		Task<TEntity> AddAsync(TEntity entity);

		Task<bool> Delete<TUniqueType>(TUniqueType uniqueIdentifier);

		IQueryable<TEntity> GetAll();

		Task<TEntity> UpdateAsync(TEntity entity);

		IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
		Task<List<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
	}
}