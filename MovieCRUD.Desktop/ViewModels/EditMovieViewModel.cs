using AutoMapper;
using MovieCRUD.Contracts.V1.Requests;
using MovieCRUD.Desktop.Enums;
using MovieCRUD.Desktop.Models;
using MovieCRUD.Desktop.Models.DTOs;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.Infrastructure.Network;
using Stylet;
using System;
using System.Windows.Input;

namespace MovieCRUD.Desktop.ViewModels
{
    public class EditMovieViewModel : Screen
    {
        public ICommand SaveChangesCommand { get; private set; }
        public MovieDTO EditedMovie { get; set; }
        public Array Ratings { get; private set; }
        private MovieApiClient _movieApiClient;
        private IMapper _mapper;
        private ILogger _logger;

        public EditMovieViewModel(MovieDTO originalMovie, MovieApiClient apiClient, IMapper mapper, ILogger logger)
        {
            EditedMovie = originalMovie;
            Ratings = Enum.GetValues(typeof(Rating));
            _mapper = mapper;
            _movieApiClient = apiClient;
            SaveChangesCommand = new Command(EditMovie, (obj) => true);
            _logger = logger;
        }

        public async void EditMovie(object _)
        {
            var editMovieRequest = _mapper.Map<EditMovieRequest>(EditedMovie);
            _logger.LogInfo("Mapped a MovieDTO to EditMovieRequest");

            await _movieApiClient.EditMovieAsync(editMovieRequest);
        }
    }
}