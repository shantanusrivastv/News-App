using System.Collections.Generic;
using System.Dynamic;

namespace Pressford.News.Services.Interfaces
{
    public interface IDataShaper<T>
    {
        IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fields);
        ExpandoObject ShapeData(T entity, string fields);
    }
}
