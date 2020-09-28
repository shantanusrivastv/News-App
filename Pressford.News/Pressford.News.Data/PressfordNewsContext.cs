using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pressford.News.Entities;

namespace Pressford.News.Data
{
    public class PressfordNewsContext : DbContext
    {
        public PressfordNewsContext(DbContextOptions<PressfordNewsContext> options) : base(options)
        {
        }

        public DbSet<Article> Article { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}