using AutoMapper;
using MovieCRUD.Desktop.Models.DTOs;
using MovieCRUD.Movies.Requests;

namespace MovieCRUD.Desktop.Tests.MappingProfiles
{
    public class DTORequestProfile : Profile
    {
        public DTORequestProfile()
        {
            CreateMap<MovieDTO, MovieDTO>().ReverseMap();
            CreateMap<MovieDTO, EditMovieRequest>().ReverseMap();
        }
    }
}
