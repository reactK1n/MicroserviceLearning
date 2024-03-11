using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PlatformServiceMicroserver.AsyncDataServices;
using PlatformServiceMicroserver.Data;
using PlatformServiceMicroserver.SyncDataServices.Grpc;
using PlatformServiceMicroserver.SyncDataServices.HTTP;
using System;
using System.IO;

namespace PlatformServiceMicroserver
{
	public class Startup
	{
		private readonly IWebHostEnvironment _env;

		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			_env = env;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{


			services.AddDbContext<PlatformDbContext>(opt =>
			{


				if (_env.IsProduction())
				{
					opt.UseSqlServer(Configuration["ConnectionStrings:PlatformConn"]);
					Console.WriteLine("Local db used");
					return;
				}
				else
				{
					opt.UseInMemoryDatabase("InMemoryDatabase");

					Console.WriteLine("In Memory database used");
				}
			});
			services.AddScoped<IplatformRepo, PlatformRepo>();

			//this is how to add http client
			services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
			services.AddSingleton<IMessageBusClient, MessageBusClient>();
			//register grpc service
			services.AddGrpc();


			services.AddControllers();
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());	

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlatformServiceMicroserver", Version = "v1" });
			});

			Console.WriteLine("Command Endpoint " + Configuration["CommandService"]);


		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformServiceMicroserver v1"));
			}


			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();


			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				//adding grpc service after registering it in the configureservice
				endpoints.MapGrpcService<GrpcPlatformService>();


				//option in grpc setup
				endpoints.MapGet("/Protos/platforms.proto", async context =>
				{
					await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
				});
			});

			app.PrepPopulation(env);
		}
	}
}
