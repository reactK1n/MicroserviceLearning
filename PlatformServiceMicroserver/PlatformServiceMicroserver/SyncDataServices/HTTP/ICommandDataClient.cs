using PlatformServiceMicroserver.DTOs;
using System.Threading.Tasks;

namespace PlatformServiceMicroserver.SyncDataServices.HTTP
{
	public interface ICommandDataClient
	{
		Task SendPlatformToCommand(PlatformReadDtos platform);
	}
}
