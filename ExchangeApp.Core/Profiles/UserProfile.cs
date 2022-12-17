using AutoMapper;
using ExchangeApp.Communication.ViewObjects.User;
using ExchangeApp.Core.Entities;

namespace ExchangeApp.Core.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserVO>()
                .ReverseMap();
        }
    }
}
