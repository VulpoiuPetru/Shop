using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FirstProject.Helpers
{
    internal class RelayCommand : ICommand
    {
        private Action<object> commandTask;
        private Predicate<object> canExecuteTask;
        public RelayCommand(Action<object> commandTask, Predicate<object> canExecuteTask)
        {
            this.commandTask = commandTask;
            this.canExecuteTask = canExecuteTask;
        }
        public RelayCommand(Action<object> workToDo) : this(workToDo, DefaultCanExecute)
        {
            commandTask = workToDo;
        }
        private static bool DefaultCanExecute(object parameter)
        {
            return true;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            commandTask(parameter);
        }
    }
}
