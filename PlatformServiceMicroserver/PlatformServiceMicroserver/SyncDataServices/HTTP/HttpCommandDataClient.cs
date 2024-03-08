
using PlatformServiceMicroserver.DTOs;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using System;
using Microsoft.Extensions.Configuration;

namespace PlatformServiceMicroserver.SyncDataServices.HTTP
{
	public class HttpCommandDataClient : ICommandDataClient
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _config;

		public HttpCommandDataClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
			_config = config;
		}
        public async Task SendPlatformToCommand(PlatformReadDtos platform)
		{
			var httpContent = new StringContent(
				JsonSerializer.Serialize(platform),
				Encoding.UTF8,
				"application/json");

			var response = await _httpClient.PostAsync($"{_config["CommandService"]}", httpContent);

            if (response.IsSuccessStatusCode)
			{
				Console.WriteLine("sync POST to CommandService was OK!");
			}

            else
			{
				Console.WriteLine("sync POST to CommandService was not OK!");
			}
        }

	}
}
