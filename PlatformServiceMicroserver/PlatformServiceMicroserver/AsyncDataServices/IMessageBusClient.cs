using PlatformServiceMicroserver.DTOs;

namespace PlatformServiceMicroserver.AsyncDataServices
{
	public interface IMessageBusClient
	{
		void PublishNewPlatform(PlatformPublishedDto platformPublished);
	}
}
