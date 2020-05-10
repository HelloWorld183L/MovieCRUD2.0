using Stylet;
using System.Linq;
using System.Windows;

namespace MovieCRUD.Desktop.Models
{
    public class WindowManagerConfig : IWindowManagerConfig
    {
        public Application Application { get; private set; }

        public Window GetActiveWindow() => Application.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive) ?? Application.MainWindow;
    }
}
