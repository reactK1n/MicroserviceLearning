using CommandsService.Model;
using System.Collections.Generic;

namespace CommandsService.SyncDataServices.Grpc
{
	public interface IPlatformDataClient
	{
		IEnumerable<Platform> ReturnAllPlatforms();
	}
}
