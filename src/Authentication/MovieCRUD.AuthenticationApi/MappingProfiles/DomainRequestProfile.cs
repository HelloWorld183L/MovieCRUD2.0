using AutoMapper;
using MovieCRUD.Authentication.Requests;
using MovieCRUD.Domain.Authentication;

namespace MovieCRUD.Authentication.MappingProfiles
{
    public class DomainRequestProfile : Profile
    {
        public DomainRequestProfile()
        {
            CreateMap<RegisterExternalRequest, User>()
                .ForMember(user => user.Username, act => act.MapFrom(request => request.Email));
        }
    }
}