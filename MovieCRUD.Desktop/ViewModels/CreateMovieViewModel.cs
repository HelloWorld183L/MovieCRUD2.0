using AutoMapper;
using MovieCRUD.Desktop.Models;
using Stylet;
using System;
using System.Windows.Input;
using MovieCRUD.Desktop.Models.DTOs;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.Movies.Clients;
using MovieCRUD.Movies.Requests;
using MovieCRUD.SharedKernel;

namespace MovieCRUD.Desktop.ViewModels
{
    public class CreateMovieViewModel : Screen
    {
        public ICommand SaveChangesCommand { get; private set; }
        public MovieDTO NewMovie { get; set; }
        public Array Ratings { get; private set; }
        private readonly IMovieApiClient _apiClient;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CreateMovieViewModel(IMovieApiClient apiClient, IMapper mapper, ILogger logger)
        {
            NewMovie = new MovieDTO();
            _apiClient = apiClient;
            _mapper = mapper;
            _logger = logger;
            SaveChangesCommand = new Command(CreateMovie, (obj) => true);
            Ratings = Enum.GetValues(typeof(Rating));
            _logger.LogInfo("Retrieved the ratings from MovieCRUD.Desktop.Enums.Rating");
        }

        public async void CreateMovie(object _)
        {
            var createMovieRequest = _mapper.Map<CreateMovieRequest>(NewMovie);
            _logger.LogInfo("Mapped MovieDTO to CreateMovieRequest");

            await _apiClient.CreateMovieAsync(createMovieRequest);
        }
    }
}
