using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Common.Logging;

namespace NCmdLiner.SolutionCreator.Library.Common.UI
{
    /// <summary>
    /// Asynchronous  command
    /// 
    /// Credits: http://enumeratethis.com/2012/06/14/asynchronous-commands-in-metro-wpf-silverlight/
    /// 
    /// </summary>
    public class AsyncCommand : ICommand
    {
        bool _isExecuting;
        private readonly Func<Task> _execute;
        private readonly Func<bool>_canExecute;
        private IEventsHelper _eventsHelper;

        private IEventsHelper EventsHelper => _eventsHelper??(_eventsHelper = new EventsHelper(LogManager.GetLogger<EventsHelper>()));

        public AsyncCommand(Func<Task> execute) : this(execute, () => true)
        {
            _execute = execute;
        }

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return !_isExecuting && _canExecute();
        }

        public async void Execute(object parameter)
        {
            _isExecuting = true;
            try
            {
                await _execute();
            }
            finally
            {
                _isExecuting = false;
                CommandManager.InvalidateRequerySuggested();
            }            
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
