using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pressford.News.Entities;

namespace Pressford.News.Data
{
    public class PressfordNewsContext : DbContext
    {
        public PressfordNewsContext(DbContextOptions<PressfordNewsContext> options) : base(options)
        {
            ChangeTracker.Tracked += ChangeTracker_Tracked;
        }

        private void ChangeTracker_Tracked(object sender, EntityTrackedEventArgs e)
        {
            DateTime now = DateTime.Now;
            if (e.Entry.Entity is IEntityDate entity)
            {
                switch (e.Entry.State)
                {
                    case EntityState.Added:
                        entity.DatePublished = now;
                        entity.DateModified = now;
                        //For future use
                        //entity.CreatedBy = CurrentUser.GetUsername();
                        //entity.UpdatedBy = CurrentUser.GetUsername();
                        break;

                    case EntityState.Modified:
                        Entry(entity).Property(x => x.DatePublished).IsModified = false;
                        entity.DateModified = now;
                        break;
                }
            }
        }

        public DbSet<Article> Article { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserLogin> UserLogin { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}