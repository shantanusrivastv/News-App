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

		//todo will this work, the above is still useful for  eager loading actually

		// Usage:
		//var authorsWithArticles = await authorRepository.FindByAsync(
		//	a => a.IsActive,         // predicate: find active authors
		//	a => a.Articles          // include related articles
		//);
		//*/
		//Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
		//Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
		//Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>> predicate);
		//Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate); //This allow more complex delete operation

		//*/

	}
}