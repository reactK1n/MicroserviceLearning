using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlatformServiceMicroserver.Models;
using System;
using System.Linq;

namespace PlatformServiceMicroserver.Data
{
	public static class PrepDb
	{
		public static void PrepPopulation(this IApplicationBuilder app, IWebHostEnvironment env)
		{
			using (var serviceScope = app.ApplicationServices.CreateScope())
			{
				seedData(serviceScope.ServiceProvider.GetService<PlatformDbContext>(), env);
			}
		}

		private static void seedData(PlatformDbContext context, IWebHostEnvironment env)
		 {

			if(env.IsProduction())
			{
				Console.WriteLine("attempting to apply migrations......");
				try
				{

				context.Database.Migrate();
				}
				catch(Exception ex)
				{
					Console.WriteLine("could not run migrations " + ex.ToString());
				}
			}
			var dataExist = context.Platforms.Any();
			var myData = context.Platforms.ToList();
			if (!dataExist)
			{
				Console.Write("Seeding Data......");
				context.Platforms.AddRange(
					new Platform { Name = "Platform 1", Publisher = "Publisher 1", Cost = "Free" },
					new Platform { Name = "Platform 2", Publisher = "Publisher 2", Cost = "Free" },
					new Platform { Name = "Platform 3", Publisher = "Publisher 3", Cost = "Free" },
					new Platform { Name = "Platform 4", Publisher = "Publisher 4", Cost = "Free" },
					new Platform { Name = "Platform 5", Publisher = "Publisher 5", Cost = "Free" }
				);

				context.SaveChanges();
			}
			else
			{
				Console.Write("We already have data");
			}

		}
	}
}
