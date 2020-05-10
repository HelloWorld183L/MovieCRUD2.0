using AutoMapper;
using MovieCRUD.Contracts.V1.Responses;
using MovieCRUD.Domain;

namespace MovieCRUD.Api.MappingProfiles
{
    public class DomainResponseProfile : Profile
    {
        public DomainResponseProfile()
        {
            CreateMap<Movie, MovieResponse>().ReverseMap();
        }
    }
}