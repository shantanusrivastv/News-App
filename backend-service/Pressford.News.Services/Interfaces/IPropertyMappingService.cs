using Pressford.News.Services.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pressford.News.Services.Interfaces
{
    //Didn't named it to IPropertyMappingService for sorting as it can be used for other operations too
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        List<string> ValidateSortFields<TSource, TDestination>(string orderBy);
    }
}
