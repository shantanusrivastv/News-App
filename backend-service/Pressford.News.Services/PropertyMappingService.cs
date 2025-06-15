using Pressford.News.Entities;
using Pressford.News.Services.Interfaces;
using Pressford.News.Services.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pressford.News.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            // Register all your specific property mappings here.
            // For each entity/DTO pair you want to support sorting for, you'll add its mapping.
            _propertyMappings.Add(new ArticlePropertyMapping());
            // Example: If you had another entity, say "Author", and its DTO "ReadAuthorDto":
            // _propertyMappings.Add(new AuthorPropertyMapping()); // Assuming AuthorPropertyMapping exists
        }

        public Dictionary<string, PropertyMappingValue> MappingDictionary { get; private set; }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            // Find a mapping that matches the source and destination types.
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>().FirstOrDefault();

            if (matchingMapping == null)
            {
                throw new InvalidOperationException($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}>");
            }

            return matchingMapping.MappingDictionary;
        }

        public List<string> ValidateSortFieldsForArticle(string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return new List<string>();

            var invalids = new List<string>();

            var orderByFields = orderBy.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(f => f.Trim().ToLower());

            foreach (var field in orderByFields)
            {
                var fieldName = field.Split(' ')[0];
                if (!MappingDictionary.ContainsKey(fieldName))
                    invalids.Add(fieldName);
            }

            return invalids;
        }

        public List<string> ValidateSortFields<TSource, TDestination>(string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return new List<string>();

            var propertyMapping = GetPropertyMapping<TSource, TDestination>();

            var invalids = new List<string>();

            var orderByFields = orderBy.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(f => f.Trim().ToLower());

            foreach (var field in orderByFields)
            {
                var fieldName = field.Split(' ')[0];
                if (!propertyMapping.ContainsKey(fieldName))
                    invalids.Add(fieldName);
            }

            return invalids;
        }
    }
}
