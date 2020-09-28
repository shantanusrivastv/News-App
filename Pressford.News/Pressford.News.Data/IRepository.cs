using System.Linq;
using System.Threading.Tasks;

namespace Pressford.News.Data
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        Task<TEntity> AddAsync(TEntity entity);
        IQueryable<TEntity> GetAll();
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}