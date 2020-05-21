using AutoMapper;
using Microsoft.AspNet.Identity;
using MovieCRUD.Authentication.Models;

namespace MovieCRUD.Authentication.MappingProfiles
{
    public class LoginInfoProfile : Profile
    {
        public LoginInfoProfile()
        {
            CreateMap<UserLoginInfo, ExternalLoginData>().ReverseMap();
        }
    }
}