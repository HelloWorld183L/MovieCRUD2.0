using AutoMapper;
using MovieCRUD.Desktop.Models;
using MovieCRUD.Desktop.Enums;
using MovieCRUD.Infrastructure.Network;
using Stylet;
using System;
using System.Windows.Input;
using MovieCRUD.Contracts.V1.Requests;
using MovieCRUD.Desktop.Models.DTOs;
using MovieCRUD.Infrastructure.Logging;

namespace MovieCRUD.Desktop.ViewModels
{
    public class CreateMovieViewModel : Screen
    {
        public ICommand SaveChangesCommand { get; private set; }
        public MovieDTO NewMovie { get; set; }
        public Array Ratings { get; private set; }
        private readonly MovieApiClient _apiClient;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CreateMovieViewModel(MovieApiClient apiClient, IMapper mapper, ILogger logger)
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
