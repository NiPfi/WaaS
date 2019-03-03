using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WaaS.Business.Dtos;
using WaaS.Business.Entities;

namespace WaaS.Business.Mapping
{
  public class AutoMapperProfile: Profile
  {
    public AutoMapperProfile()
    {
      CreateMap<IdentityUser, UserDto>();
      CreateMap<UserDto, IdentityUser>();
    }
  }
}
