using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pressford.News.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Pressford.News.Services.Mapper;

namespace Pressford.News.Services.Extensions
{
    internal static class QueryableExtensions
    {
        public static async Task<PagedList<T>> ToPageListAsync<T>(
            this IQueryable<T> source,
            int pageNumber,
            int pageSize)
        {
            var totalCount = await source.CountAsync();
            var items = await source.Skip(pageSize * (pageNumber - 1))
                                    .Take(pageSize)
                                    .ToListAsync();

            return new PagedList<T>(items, totalCount, pageNumber, pageSize);
        }

        /*
         * Interesting fact is that we can use the same technique for filtering if we want to allow it on all fields.
         * We could use the mapping table to find out how a property on a resource maps to the underlying store and 
         * use dynamic link to create the Where clauses. 
         * So this approach is pretty flexible and can be used for more than just sorting..
         * */
        public static IQueryable<TSource> ApplySorting<TSource, TDest>(
            this IQueryable<TSource> source,
            string orderBy,
            Dictionary<string, PropertyMappingValue> mappingDictionary)
                where TSource : class 
                where TDest   : class
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return source;

            var sortClauses = orderBy.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var finalClauses = new List<string>();

            foreach (var clause in sortClauses)
            {
                var parts = clause.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var field = parts[0];
                var direction = parts.Length == 2 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase)
                                ? "desc" : "asc";

                if (!mappingDictionary.TryGetValue(field, out var map))
                    throw new ArgumentException($"Cannot sort by '{field}'");

                var effective = map.ReverseDirection
                                ? (direction == "asc" ? "desc" : "asc")
                                : direction;

                foreach (var dest in map.DestinationProperties)
                    finalClauses.Add($"{dest} {effective}");
            }

            var orderString = string.Join(",", finalClauses);

            return source.OrderBy(orderString);
        }

    }
}
