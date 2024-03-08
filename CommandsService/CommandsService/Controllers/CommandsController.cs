using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;

namespace CommandsService.Controllers
{
	//[Route("api/c/platforms/[platformId]/[controller]")]
	[Route("api/c/platforms/")]
	[ApiController]
	public class CommandsController : ControllerBase
	{

		private readonly ICommandRepo _repo;
		private readonly IMapper _map;

		public CommandsController(ICommandRepo repo, IMapper map)
		{
			_repo = repo;
			_map = map;
		}



		[HttpGet]
		[Route("{platformId}/[controller]")]
		public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
		{
			Console.WriteLine($"getting  GetCommandsForPlatform {platformId}");
			if (!_repo.PlatformExist(platformId))
			{
				return NotFound();
			}

			var commands = _repo.GetCommandsForPlatform(platformId);

			return Ok(_map.Map<IEnumerable<CommandReadDto>>(commands));
		}

		[HttpGet]
		[Route("{platformId}/[controller]/{commandId}/GetCommandForPlatform")]
		public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
		{
			Console.WriteLine($"getting  GetCommandForPlatform {platformId} and {commandId}........");
			if (!_repo.PlatformExist(platformId))
			{
				return NotFound();
			}
			var command = _repo.GetCommand(platformId, commandId);
			if (command == null)
			{
				return NotFound();
			}

			return Ok(_map.Map<CommandReadDto>(command));
		}

		[HttpPost]
		[Route("{platformId}/commands")]
		public ActionResult<IEnumerable<CommandReadDto>> CreateCommand(int platformId, CommandCreateDto command)
		{
			Console.WriteLine($"  Creating Command {platformId}........ ");
			if (!_repo.PlatformExist(platformId))
			{
				return NotFound();
			}


			var mapCommand = _map.Map<Command>(command);

			_repo.CreateCommand(platformId, mapCommand);
			_repo.SaveChanges();
			return Ok(_map.Map<CommandReadDto>(mapCommand));

		}


	}
}
