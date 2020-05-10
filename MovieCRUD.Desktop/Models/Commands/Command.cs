using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieCRUD.Desktop.Models
{
    public class Command : ICommand
    {
        private readonly Action<object> _executeMethod;
        private readonly Predicate<object> _canExecuteMethod;

        public event EventHandler CanExecuteChanged;

        public Command(Action<object> executeMethod, Predicate<object> canExecuteMethod)
        {
            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(object param)
        {
            if (_canExecuteMethod != null)
            {
                return _canExecuteMethod(param);
            }
            return false;
        }

        public void Execute(object param)
        {
            if (_executeMethod != null)
            {
                _executeMethod(param);
            }
        }
    }
}
