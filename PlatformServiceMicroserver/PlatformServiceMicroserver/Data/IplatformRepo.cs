using PlatformServiceMicroserver.Models;
using System.Collections.Generic;

namespace PlatformServiceMicroserver.Data
{
	public interface IplatformRepo
	{
		bool SaveChanges();

		IEnumerable<Platform> GetAllPlatforms();

		Platform GetPlatformById(int id);

		void CreatePlatform(Platform platform);
	}
}
