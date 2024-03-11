﻿using AutoMapper;
using Grpc.Core;
using PlatformServiceMicroserver.Data;
using System.Threading.Tasks;

namespace PlatformServiceMicroserver.SyncDataServices.Grpc
{
	public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
	{
		private readonly IplatformRepo _repo;
		private readonly IMapper _mapper;

		public GrpcPlatformService(IplatformRepo repo, IMapper mapper)
		{
			_repo = repo;
			_mapper = mapper;
		}

		public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
		{
			var response = new PlatformResponse();
			var platforms = _repo.GetAllPlatforms();

            foreach (var plat in platforms)
            {
				response.Platform.Add(_mapper.Map<GrpcPlatformModel>(plat));
            }
            return Task.FromResult(response);
		}
	}
}
