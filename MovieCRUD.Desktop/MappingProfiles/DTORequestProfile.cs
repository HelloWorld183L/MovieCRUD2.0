using AutoMapper;
using MovieCRUD.Desktop.Models.DTOs;
using MovieCRUD.Movies.Requests;

namespace MovieCRUD.Desktop.MappingProfiles
{
    public class DTORequestProfile : Profile
    {
        public DTORequestProfile()
        {
            CreateMap<MovieDTO, CreateMovieRequest>().ReverseMap();
            CreateMap<MovieDTO, EditMovieRequest>().ReverseMap();
        }
    }
}
