using AutoMapper;
using CommandsService.Model;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using PlatformServiceMicroserver;
using System;
using System.Collections.Generic;

namespace CommandsService.SyncDataServices.Grpc
{
	public class PlatformDataClient : IPlatformDataClient
	{
		private readonly IConfiguration _config;
		private readonly IMapper _mapper;

		public PlatformDataClient(IConfiguration config, IMapper mapper)
		{
			_config = config;
			_mapper = mapper;
		}


		public IEnumerable<Platform> ReturnAllPlatforms()
		{
			Console.WriteLine($"-------> Calling Grpc service {_config["GrpcPlatform"]} -------->");
			var channel = GrpcChannel.ForAddress(_config["GrpcPlatform"]);
			var client = new GrpcPlatform.GrpcPlatformClient(channel);
			var request = new GetAllRequest();

			try
			{
				var reply = client.GetAllPlatforms(request);
				return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
			}
			catch(Exception ex)
			{
				Console.WriteLine($"--------> could not call GRPC Server {ex.Message}--------------->");
				return null;
			}
		}
	}
}
