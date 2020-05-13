using AutoMapper;
using MovieCRUD.DTOs;
using MovieCRUD.Movies.Requests;

namespace MovieCRUD.MappingProfiles
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