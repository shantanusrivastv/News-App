using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pressford.News.Entities;

namespace Pressford.News.Data
{
    public sealed class PressfordNewsContext : DbContext
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
                        //Update(entity);
                        //Entry(entity).Property(x => x.DatePublished).IsModified = false;
                        entity.DatePublished = e.Entry.GetDatabaseValues().GetValue<DateTime>(nameof(entity.DatePublished));
                        entity.DateModified = now;
                        break;
                }
            }
        }

        public DbSet<Article> Article { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserLogin> UserLogin { get; set; }
        public DbSet<ArticleLikes> ArticleLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //It searches for all the configuration of all entities
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<User>().HasData(
                    new User { Id = 1, FirstName = "W", LastName = "Pressford ", Email = "w.Pressford@pressford.com" },
                    new User { Id = 2, FirstName = "Admin", LastName = "User", Email = "adminUser@pressford.com" },
                    new User { Id = 3, FirstName = "Normal", LastName = "User", Email = "normalUser@pressford.com" });

            modelBuilder.Entity<UserLogin>().HasData(
                    new UserLogin { Username = "w.Pressford@pressford.com", Password = "admin", Role = RoleType.Publisher },
                    new UserLogin { Username = "adminUser@pressford.com", Password = "admin", Role = RoleType.Publisher },
                    new UserLogin { Username = "normalUser@pressford.com", Password = "user", Role = RoleType.User });
        }
    }
}