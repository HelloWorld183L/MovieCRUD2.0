using System.Windows.Input;
using AutoMapper;
using MovieCRUD.Desktop.Models;
using MovieCRUD.Desktop.Models.DTOs;
using Stylet;
using System.Collections.Generic;
using MovieCRUD.Infrastructure.Logging;
using MovieCRUD.Movies.Responses;
using MovieCRUD.Movies.Clients;
using MovieCRUD.SharedKernel;

namespace MovieCRUD.Desktop.ViewModels
{
    public class MovieCRUDViewModel : Screen
    {
        public ICommand LoadMovieDataCommand { get; private set; }
        public ICommand CreateMovieCommand { get; private set; }
        public ICommand SignOutCommand { get; private set; }
        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }
        public ICommand DeleteMovieCommand { get; private set; }
        public ICommand OpenEditMovieViewCommand { get; private set; }
        public ICommand OpenMovieDetailsViewCommand { get; private set; }
        public MovieDTO SelectedMovie { get; set; }
        private IEnumerable<MovieDTO> _movieDTOs;
        public IEnumerable<MovieDTO> Movies 
        {
            get => _movieDTOs;
            set
            {
                SetAndNotify(ref _movieDTOs, value);
                _logger.LogInfo("Raised PropertyChanged event within a Movies setter");
            }
        }
        private IMovieApiClient _movieApiClient;
        private IWindowManager _windowManager;
        private IMapper _mapper;
        private readonly ILogger _logger;

        public MovieCRUDViewModel(IMovieApiClient apiClient, IWindowManager windowManager, IMapper mapper, ILogger logger)
        {
            _movieApiClient = apiClient;
            _windowManager = windowManager;
            _mapper = mapper;
            _logger = logger;
            SetUpCommands();
        }

        private void SetUpCommands()
        {
            LoadMovieDataCommand = new Command(GetMoviesAsync, (obj) => true);
            CreateMovieCommand = new Command(OpenCreateMovieWindow, (obj) => true);
            SignOutCommand = new Command(SignOut, (obj) => true);
            UndoCommand = new Command(Undo, (obj) => true);
            RedoCommand = new Command(Redo, (obj) => true);
            DeleteMovieCommand = new Command(DeleteMovie, (obj) => true);
            OpenEditMovieViewCommand = new Command(OpenEditMovieWindow, (obj) => true);
            OpenMovieDetailsViewCommand = new Command(OpenMovieDetailsView, (obj) => true);
        }

        private async void GetMoviesAsync(object _)
        {
            var movies = await _movieApiClient.GetAllMoviesAsync(PaginationQuery.CreateQuery(1, 10));

            Movies = MapResponsesToDTOs(movies);
        }

        public void OpenCreateMovieWindow(object _)
        {
            _windowManager.ShowWindow(new CreateMovieViewModel(_movieApiClient, _mapper, _logger));
            _logger.LogInfo("Displayed the CreateMovieView");
        }

        public void SignOut(object _)
        {
            _windowManager.ShowWindow(new LoginViewModel());
            _logger.LogInfo("Displayed the LoginView");
        }

        public void Undo(object _)
        {

        }

        public void Redo(object _)
        {

        }

        public async void DeleteMovie(object _)
        {
            if (SelectedMovie != null)
            {
                await _movieApiClient.DeleteMovieAsync(SelectedMovie.Id);

                LoadMovieDataCommand.Execute(new object());
            }
        }

        public void OpenEditMovieWindow(object _)
        {
            if (SelectedMovie != null)
            {
                _windowManager.ShowWindow(new EditMovieViewModel(SelectedMovie, _movieApiClient, _mapper, _logger));
                _logger.LogInfo("Displayed EditMovieView");
            }
        }

        public void OpenMovieDetailsView(object _)
        {
            if (SelectedMovie != null)
            {
                _windowManager.ShowWindow(new MovieDetailsViewModel(SelectedMovie));
                _logger.LogInfo("Displayed MovieDetailsView");
            }
        }

        private IEnumerable<MovieDTO> MapResponsesToDTOs(IEnumerable<MovieResponse> movieResponses)
        {
            var movieDTOs = new List<MovieDTO>();

            foreach (var movieResponse in movieResponses)
            {
                var mappedMovie = _mapper.Map<MovieDTO>(movieResponse);
                _logger.LogInfo("Mapped a MovieResponse object to a MovieDTO object");
                movieDTOs.Add(mappedMovie);
            }

            return movieDTOs;
        }
    }
}