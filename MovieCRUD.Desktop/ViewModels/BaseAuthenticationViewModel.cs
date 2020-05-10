using MovieCRUD.Desktop.Models;
using Stylet;
using System.Windows.Input;

namespace MovieCRUD.Desktop.ViewModels
{
    public class BaseAuthenticationViewModel : Screen
    {
        public ICommand SubmitAuthenticationDetailsCommand { get; private set; }
        
        public BaseAuthenticationViewModel()
        {
            SetUpCommands();
        }

        private void SetUpCommands()
        {
            SubmitAuthenticationDetailsCommand = new Command(SubmitAuthenticationDetails, (obj) => true);
        }

        public void SubmitAuthenticationDetails(object _)
        {

        }
    }
}
