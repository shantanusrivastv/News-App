using Pressford.News.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pressford.News.Services.Mapper
{
    public class DataShaper<T> : IDataShaper<T> where T : class
    {
        private readonly PropertyInfo[] _properties;
        public DataShaper()
        {
            _properties = typeof(T).GetProperties(BindingFlags.Public | 
                                                  BindingFlags.Instance);
        }
       
        private IEnumerable<PropertyInfo> GetRequestedProperties(string? fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
                return _properties;

            var fieldsAfterSplit = fields.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                         .Select(f => f.Trim());

            return _properties.Where(p => fieldsAfterSplit.Contains(p.Name, StringComparer.OrdinalIgnoreCase));
        }

        public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> sources, string fields)
        {
            var props = GetRequestedProperties(fields);
            foreach (var src in sources)
                yield return ShapeObject(src, props);
        }

        public ExpandoObject ShapeData(T source, string fields)
        {
            var props = GetRequestedProperties(fields);
            return ShapeObject(source, props);
        }

        private ExpandoObject ShapeObject(T src, IEnumerable<PropertyInfo> requestedProps)
        {
            var shapedObject = new ExpandoObject();
            var dict = shapedObject as IDictionary<string, object?>;
            foreach (var prop in requestedProps)
            {
                //dict[prop.Name] = prop.GetValue(src); this also works but add confision
                dict.Add(prop.Name, prop.GetValue(src));
            }
                
            return shapedObject;
        }
    }
}
