using AutoMapper;
using MovieCRUD.Contracts.V1.Responses;
using MovieCRUD.Domain;

namespace MovieCRUD.Contracts.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Movie, MovieResponse>();
        }
    }
}