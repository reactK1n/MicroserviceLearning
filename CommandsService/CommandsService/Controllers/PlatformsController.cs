using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CommandsService.Controllers
{
	[Route("api/c/[controller]")]
	[ApiController]
	public class PlatformsController : ControllerBase
	{
		private readonly ICommandRepo _repo;
		private readonly IMapper _map;

		public PlatformsController(ICommandRepo repo, IMapper map)
        {
			_repo = repo;
			_map = map;
		}



		[HttpGet]
		public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
		{
			Console.WriteLine("getting platform from commandService");

			var platformItem = _repo.GetAllPlatforms();

			return Ok(_map.Map<IEnumerable<PlatformReadDto>>(platformItem));
		}

	
		[HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("Inbound POST # command service");

            return Ok("Inbound text of from platform controller");
        }
    }
}
