using AutoMapper;
using PlatformServiceMicroserver.DTOs;
using PlatformServiceMicroserver.Models;

namespace PlatformServiceMicroserver.Profiles
{
	public class PlatformProfile : Profile
	{
        public PlatformProfile()
        {
            //source -> target
            CreateMap<Platform, PlatformReadDtos>();
            CreateMap<PlatformCreateDtos, Platform>();
			CreateMap<PlatformReadDtos, PlatformPublishedDto>();

		}
	}
}
