﻿using System.ComponentModel.DataAnnotations;

namespace PlatformServiceMicroserver.DTOs
{
	public class PlatformReadDtos
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Publisher { get; set; }

		public string Cost { get; set; }
	}
}
