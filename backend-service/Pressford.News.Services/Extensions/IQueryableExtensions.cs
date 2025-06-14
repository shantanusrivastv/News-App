using Microsoft.EntityFrameworkCore;
using Pressford.News.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pressford.News.Services.Extensions
{
    internal static class QueryableExtensions
    {
        public static async Task<PagedList<T>> ToPageListAsync<T>(
            this IQueryable<T> source ,
            int pageNumber,
            int pageSize
            )
        {
            var totalCount = await source.CountAsync();
            var items = await source.Skip(pageSize * (pageNumber - 1))
                                    .Take(pageSize)
                                    .ToListAsync();

            return new PagedList<T>( items, totalCount, pageNumber, pageSize);
        }

    }
}
