using AutoMapper;
using MovieCRUD.Contracts.V1.Requests;
using MovieCRUD.Domain.DomainObjects;

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