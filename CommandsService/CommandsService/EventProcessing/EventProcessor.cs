using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;

namespace CommandsService.EventProcessing
{
	public class EventProcessor : IEventProcessor
	{
		private readonly IServiceScopeFactory _scope;
		private readonly IMapper _mapper;

		public EventProcessor(IServiceScopeFactory scope, IMapper mapper)
		{
			_scope = scope;
			_mapper = mapper;
		}
		public void ProcessEvent(string message)
		{
			var eventType = DetermineEvent(message);
			switch (eventType)
			{

				case EventType.PlatformPublished:
					AddPlatform(message);
					break;
				default:
					break;

			}

		}

		private EventType DetermineEvent(string notificationMessage)
		{
			Console.WriteLine("Determine Event");

			var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
			Console.WriteLine($"EventType: {eventType.Event}");

			switch (eventType.Event)
			{
				case "Platform_Published":
					Console.WriteLine("platform published event Detached");
					return EventType.PlatformPublished;
				default:
					Console.WriteLine("could not determine the event type");
					return EventType.Undetermined;
			}
		}

		private void AddPlatform(string platformPublishedMessage)
		{
			using (var scope = _scope.CreateScope())
			{
				var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

				var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

				try
				{
					var plat = _mapper.Map<Platform>(platformPublishedDto);
					if(!repo.ExternalPlatformExist(plat.ExternalID))
					{
						repo.CreatePlatform(plat);
						repo.SaveChanges();
						Console.WriteLine("platform added!............");

					}
					else
					{ 
						Console.WriteLine("platform already exist............");
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"could not add platform to DB {ex.Message}");
				}
			}
		}
	}

	enum EventType
	{
		PlatformPublished, Undetermined
	}
}
