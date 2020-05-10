using AutoMapper;
using MovieCRUD.Contracts.V1.Requests;
using MovieCRUD.Domain;

namespace MovieCRUD.Api.MappingProfiles
{
    public class RequestDomainProfile : Profile
    {
        public RequestDomainProfile()
        {
            CreateMap<CreateMovieRequest, Movie>().ReverseMap();
            CreateMap<EditMovieRequest, Movie>().ReverseMap();
        }
    }
}