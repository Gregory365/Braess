namespace Braess.ViewModel.Tools
{
    using System;
    using System.Windows.Input;

    public class RelayCommand : ICommand
    {
        private readonly Action commandAction;
        private readonly Func<bool> canExecute;

        public RelayCommand(Action commandAction, Func<bool> canExecute = null)
        {
            this.commandAction = commandAction;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute is null)
            {
                return true;
            }

            return canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            commandAction.Invoke();
        }
    }
}