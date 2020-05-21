using AutoMapper;
using System.Security.Claims;

namespace MovieCRUD.Authentication.MappingProfiles
{
    public class ClaimsProfile : Profile
    {
        public ClaimsProfile()
        {
            CreateMap<Claim, Domain.Authentication.Claim>().ReverseMap();
        }
    }
}