using AutoMapper;
using MovieCRUD.Contracts.V1.Responses;
using MovieCRUD.Domain;

namespace MovieCRUD.Contracts.MappingProfiles
{
    public class ResponseToDomainProfile : Profile
    {
        public ResponseToDomainProfile()
        {
            CreateMap<MovieResponse, Movie>();
        }
    }
}