﻿using CommandsService.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandsService.Data
{
	public class CommandRepo : ICommandRepo
	{
		private readonly CommandDbContext _context;

		public CommandRepo(CommandDbContext context)
		{
			_context = context;
		}
		public void CreateCommand(int platformId, Command command)
		{
			if (command == null)
			{
				throw new ArgumentNullException(nameof(command));
			}

			command.PlatformId = platformId;

			_context.Add(command);
		}

		public void CreatePlatform(Platform platform)
		{
			if (platform == null)
			{
				throw new ArgumentNullException(nameof(platform));
			}

			_context.Platforms.Add(platform);
		}

		public bool ExternalPlatformExist(int externalPlatformId)
		{
			return _context.Platforms.Any(p => p.ExternalID == externalPlatformId);
		}

		public IEnumerable<Platform> GetAllPlatforms()
		{
			return _context.Platforms.ToList();
		}

		public Command GetCommand(int platformId, int commandId)
		{
			return _context.Commands
				.FirstOrDefault(c => c.PlatformId == platformId && c.Id == commandId);
		}

		public IEnumerable<Command> GetCommandsForPlatform(int platformId)
		{
			return _context.Commands
				.Where(c => c.PlatformId == platformId)
				.OrderBy(c => c.Platform.Name);
		}

		public bool PlatformExist(int platformId)
		{
			return _context.Platforms.Any(p => p.Id == platformId);

		}

		public bool SaveChanges()
		{
			return (_context.SaveChanges() >= 0);
		}
	}
}
