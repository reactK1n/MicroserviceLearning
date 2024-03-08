using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformServiceMicroserver.AsyncDataServices;
using PlatformServiceMicroserver.Data;
using PlatformServiceMicroserver.DTOs;
using PlatformServiceMicroserver.Models;
using PlatformServiceMicroserver.SyncDataServices.HTTP;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlatformServiceMicroserver.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlatformsController : ControllerBase
	{
		private readonly IplatformRepo _repo;
		private readonly IMapper _mapper;
		private readonly ICommandDataClient _cmd;
		private readonly IMessageBusClient _messageBusClient;

		public PlatformsController(IplatformRepo repo,
			IMapper mapper,
			ICommandDataClient cmd,
			IMessageBusClient messageBusClient)
		{
			_repo = repo;
			_mapper = mapper;
			_cmd = cmd;
			_messageBusClient = messageBusClient;
		}

		[HttpGet]
		[Route("Get_All_Platform")]
		public ActionResult<IEnumerable<PlatformReadDtos>> GetPlatform()
		{
			Console.WriteLine("Getting Platform.........");
			var PlatformItem = _repo.GetAllPlatforms();

			return Ok(_mapper.Map<IEnumerable<PlatformReadDtos>>(PlatformItem));
		}

		[HttpGet]
		[Route("{id}")]
		public ActionResult<PlatformReadDtos> GetPlatformById(int id)
		{
			Console.WriteLine("Getting Platform.........");
			var PlatformItem = _repo.GetPlatformById(id);
			if (PlatformItem != null)
			{

				return Ok(_mapper.Map<PlatformReadDtos>(PlatformItem));
			}

			return NotFound();

		}


		[HttpPost]
		public async Task<ActionResult<PlatformReadDtos>> Createplatform(PlatformCreateDtos platformCreateDto)
		{
			var platformModel = _mapper.Map<Platform>(platformCreateDto);
			_repo.CreatePlatform(platformModel);
			_repo.SaveChanges();

			var platformReadDto = _mapper.Map<PlatformReadDtos>(platformModel);
			//send sync message
			try
			{
				await _cmd.SendPlatformToCommand(platformReadDto);
			}
			catch (Exception ex)
			{
				Console.WriteLine(" Could not send synchronous: " + ex.Message);
			}

			//send async message
			try
			{
				var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
				platformPublishedDto.Event = "Platform_Published";
				_messageBusClient.PublishNewPlatform(platformPublishedDto);
			}
			catch (Exception ex)
			{
				Console.WriteLine(" Could not send asynchronous: " + ex.Message);
			}

			//return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
			return Ok(platformReadDto);

		}
	}
}
