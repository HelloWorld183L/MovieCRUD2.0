using AutoMapper;
using MovieCRUD.Domain;
using MovieCRUD.Domain.Movies;
using MovieCRUD.Movies.Responses;
using System.Collections.Generic;

namespace MovieCRUD.Movies.Tests.MappingProfiles
{
    public class DomainResponseProfile : Profile
    {
        public DomainResponseProfile()
        {
            CreateMap<Movie, MovieResponse>().ReverseMap();
        }
    }
}