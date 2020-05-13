using AutoMapper;
using MovieCRUD.DTOs;
using MovieCRUD.Movies.Requests;

namespace MovieCRUD.Web.Tests.MappingProfiles
{
    public class DTORequestProfile : Profile
    {
        public DTORequestProfile()
        {
            CreateMap<MovieDTO, EditMovieRequest>().ReverseMap();
            CreateMap<MovieDTO, CreateMovieRequest>().ReverseMap();
        }
    }
}