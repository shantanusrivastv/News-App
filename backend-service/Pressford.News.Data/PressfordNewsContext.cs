using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
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

		//todo Move the logic to SaveChangesAsync
		private void ChangeTracker_Tracked(object sender, EntityTrackedEventArgs e)
		{
			DateTime now = DateTime.UtcNow;
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

		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			//foreach (var entry in ChangeTracker.Entries<IEntityDate>())
			//{
			//	if (entry.State == EntityState.Added)
			//	{
			//		entry.Entity.DatePublished = DateTime.UtcNow;
			//		entry.Entity.DateModified = DateTime.UtcNow;
			//	}
			//	else if (entry.State == EntityState.Modified)
			//	{
			//		entry.Entity.DateModified = DateTime.UtcNow;
			//	}
			//}

			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		public DbSet<Article> Article { get; set; }
		public DbSet<User> User { get; set; }
		public DbSet<UserLogin> UserLogin { get; set; }
		public DbSet<ArticleLikes> ArticleLikes { get; set; }
		public DbSet<Artist> Artist { get; set; }
		public DbSet<Cover> Cover { get; set; }
		public DbSet<AuthorWithArticles> AuthorWithArticles { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//It searches for all the configuration of all entities
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			modelBuilder.Entity<AuthorWithArticles>().HasNoKey().ToView(nameof(AuthorWithArticles));

			//For JSOn Support currently it does not support for entity part of has data:
			//HasData is not supported for entities mapped to JSON
			//modelBuilder.Entity<Artist>().OwnsOne(u => u.Contact, navBuilder => { navBuilder.ToJson();});

			//var contact1 = new ContactDetails { Address = "1234 Main St", City = "New York", State = "NY", Zip = "10001" };
			//var contact2 = new ContactDetails { Address = "5678 Elm St", City = "Los Angeles", State = "CA", Zip = "90001" };
			//var contact3 = new ContactDetails { Address = "91011 Pine St", City = "Chicago", State = "IL", Zip = "60007" };

			//modelBuilder.Entity<User>().ComplexProperty(m => m.Name).IsRequired(false);

			//Add a shadow property to one type
			//modelBuilder.Entity<User>().Property<DateTime>("LastUpdated");

			modelBuilder.Entity<User>().HasData(
				new User { Id = 1, FirstName = "W", LastName = "Pressford ", Email = "w.Pressford@pressford.com" },
				new User { Id = 2, FirstName = "Admin", LastName = "User", Email = "adminUser@pressford.com" },
				new User { Id = 3, FirstName = "Normal", LastName = "User", Email = "normalUser@pressford.com" });

			modelBuilder.Entity<UserLogin>().HasData(
				new UserLogin { Username = "w.Pressford@pressford.com", Password = "admin", Role = RoleType.Publisher },
				new UserLogin { Username = "adminUser@pressford.com", Password = "admin", Role = RoleType.Publisher },
				new UserLogin { Username = "normalUser@pressford.com", Password = "user", Role = RoleType.User });

			modelBuilder.Entity<Artist>().HasData(
				new Artist { ArtistId = 1, FirstName = "John", LastName = "Doe" },
				new Artist { ArtistId = 2, FirstName = "Jane", LastName = "Smith" },
				new Artist { ArtistId = 3, FirstName = "Michael", LastName = "Johnson" });

			modelBuilder.Entity<Cover>().HasData(
				new Cover { CoverId = 1, DesignIdeas = "Left hand in dark", DigitalOnly = true },
				new Cover { CoverId = 2, DesignIdeas = "Add a clock", DigitalOnly = true },
				new Cover { CoverId = 3, DesignIdeas = "Massive Cloud in dark background", DigitalOnly = false });
		}

		#region Shadow Property
		//private void UpdateAuditData()
		//{
		//	foreach (var entry in ChangeTracker.Entries().Where(e => e.Entity is User))
		//	{
		//		entry.Property("LastUpdated").CurrentValue = DateTime.Now;
		//	}
		//}

		//public override int SaveChanges()
		//{
		//	UpdateAuditData();
		//	return base.SaveChanges();
		//}
		#endregion
	}
}