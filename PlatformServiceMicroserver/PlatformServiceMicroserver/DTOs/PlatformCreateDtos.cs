using System.ComponentModel.DataAnnotations;

namespace PlatformServiceMicroserver.DTOs
{
	public class PlatformCreateDtos
	{

		//[Required]
		public string Name { get; set; }

		//[Required]
		public string Publisher { get; set; }

		//[Required]
		public string Cost { get; set; }
	}
}
