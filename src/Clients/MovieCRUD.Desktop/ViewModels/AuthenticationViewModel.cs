using Stylet;

namespace MovieCRUD.Desktop.ViewModels
{
    public class AuthenticationViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private readonly IWindowManager _windowManager;

        public AuthenticationViewModel() { }

        public AuthenticationViewModel(IWindowManager windowManager, LoginViewModel loginViewModel, RegisterViewModel registerViewModel)
        {
            _windowManager = windowManager;
            this.Items.Add(loginViewModel);
            this.Items.Add(registerViewModel);
            this.ActiveItem = registerViewModel;
        }
    }
}