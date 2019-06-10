using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WaaS.Business.Dtos;
using WaaS.Business.Dtos.ScrapeJob;
using WaaS.Business.Dtos.User;
using WaaS.Business.Entities;

namespace WaaS.Business.Mapping
{
  public class AutoMapperProfile: Profile
  {
    public AutoMapperProfile()
    {
      CreateMap<UserDto, IdentityUser>()
        .ForMember(
          destination => destination.UserName,
          options => options.MapFrom(source => source.Email));
      CreateMap<IdentityUser, UserDto>()
        .ForMember(destination => destination.Password, options => options.Ignore());

      CreateMap<ScrapeJobDto, ScrapeJob>()
        .ForMember(destination => destination.UserSpecificId, options => options.MapFrom(dto => dto.Id))
        .ForMember(destination => destination.Id, options => options.Ignore());
      CreateMap<ScrapeJob, ScrapeJobDto>()
        .ForMember(destination => destination.Id, options => options.MapFrom(sj => sj.UserSpecificId));

      CreateMap<ScrapeJobEventDto, ScrapeJobEvent>();
      CreateMap<ScrapeJobEvent, ScrapeJobEventDto>()
        .ForMember(destination => destination.TimeStamp, options => options.MapFrom(dto => dto.TimeStamp.ToString("yyyy-MM-dd HH:mm")));

    }
  }
}
