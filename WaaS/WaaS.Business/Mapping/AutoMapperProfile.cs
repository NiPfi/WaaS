using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WaaS.Business.Dtos;
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

      CreateMap<ScrapeJobDto, ScrapeJob>();
      CreateMap<ScrapeJob, ScrapeJobDto>();

      CreateMap<ScrapeJobEventDto, ScrapeJobEvent>();
      CreateMap<ScrapeJobEvent, ScrapeJobEventDto>()
        .ForMember(x => x.TimeStamp, o => o.MapFrom(s => s.TimeStamp.ToString("yyyy-MM-dd HH:mm")));

    }
  }
}
