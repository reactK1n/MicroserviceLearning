using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Model;

namespace CommandsService.Profiles
{
	public class CommandsProfile : Profile
	{
		public CommandsProfile()
		{
			//source   ---> target
			CreateMap<Platform, PlatformReadDto>();
			CreateMap<CommandCreateDto, Command>();
			CreateMap<Command, CommandReadDto>();
			CreateMap<PlatformPublishedDto, Platform>()
				.ForMember(desc => desc.ExternalID, opt => opt.MapFrom(src => src.Id));

		}
	}
}
