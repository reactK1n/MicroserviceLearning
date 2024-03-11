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
			CreateMap<Platform, GrpcPlatformModel>()
				.ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher));


		}
	}
}
