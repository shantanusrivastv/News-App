## List of todos ##
1. Make it cross Platform and work with Sql Lite
2. Add Catching for API
3. Containerize the application
4. Update the UI
5. CQRS with MediatR Pattern
6. GraphQL 
7. Ocelot API Gateway
8. Event-Driven Architecture:RabbitMQ
9. Add a CI/CD Pipeline
10. Health checks : Prometheus
11. EF Core 
	1. add relationships and return author with all published articles
	2. YOu can use EF Core .Include() method to load related data
	3. Also add logging to see executed SQL queries
	4. Consider adding no tracking for many queries CreditSuise did this
	Example: 
	```csharp
		var articles = _context.Articles.Include(a => a.Author).AsNoTracking();
		//At Db Set level
		protected override void OnConfiguring (DbContextOptionsBui1der optionsBui1der)
		optionsBui1der
			. UseSq1Server (myconnectionString)
			. UseQueryTrackingBehavior
		QueryTrackingBehavior . NoTracking) ;
	```
	5.Use below link to work with both DB
https://jasonwatmore.com/post/2020/01/03/aspnet-core-ef-core-migrations-for-multiple-databases-sqlite-and-sql-server		
12. We need a pagination for the articles especially when we are returning all articles
	1. We can use Skip and Take methods to implement pagination
	1. We can also use a library like PagedList to implement pagination
13. Need a Serilog for logging along with Kibana and Elastic Search
14. Need to start using xunit with NSubistitute for testing but it would be least priority for now
15. Use Autofixture for fake data.
16. This might not be in near future but maybe BenchmarkDotNet 
	https://github.com/dotnet/BenchmarkDotNet
17. Add warning as error
