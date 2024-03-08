﻿using Microsoft.EntityFrameworkCore;
using PlatformServiceMicroserver.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlatformServiceMicroserver.Data
{
	public class PlatformRepo : IplatformRepo
	{
		private PlatformDbContext _context;

		public PlatformRepo(PlatformDbContext context)
        {
			_context = context;
        }

        public void CreatePlatform(Platform platform)
		{
            if (platform == null)
            {
				throw new ArgumentNullException(nameof(platform));
            }
			_context.Add(platform);
        }

		public IEnumerable<Platform> GetAllPlatforms()
		{
			return _context.Platforms.ToList();
		}

		public Platform GetPlatformById(int id)
		{
			return _context.Platforms.FirstOrDefault(p => p.Id == id);
		}

		public bool SaveChanges()
		{
			return (_context.SaveChanges() >= 0);
		}
	}
}
