using AutoMapper;
using System.Security.Claims;

namespace MovieCRUD.Authentication.MappingProfiles
{
    public class ClaimProfile : Profile
    {
        public ClaimProfile()
        {
            CreateMap<Domain.Authentication.Claim, Claim>().ReverseMap();
        }
    }
}