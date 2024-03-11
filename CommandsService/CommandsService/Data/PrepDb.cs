using CommandsService.Model;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace CommandsService.Data
{
	public static class PrepDb
	{
		public static void PrepPopulation(this IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices.CreateScope())
			{
				var grpcCLient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

				var platforms = grpcCLient.ReturnAllPlatforms();

				seedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(), platforms);
			}
		}

		private static void seedData(ICommandRepo repo, IEnumerable<Platform> platforms)
		{
			Console.WriteLine("------------>seeding new platforms......");

			foreach (var platform in platforms)
			{
				if (!repo.ExternalPlatformExist(platform.ExternalID))
				{
					repo.CreatePlatform(platform);
				}
				repo.SaveChanges();
			}
		}

	}
}
