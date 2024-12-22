using System;
using System.Linq;
using System.Windows.Input;

namespace HotKeyLibrary
{
    public class Command : ICommand
    {
        protected Action? action;
        protected Func<bool>? canExecuteFunc;
        protected Action<object?>? parameterizedAction;
        protected Func<object?, bool>? parameterizedCanExecuteFunc;

        public Command(Action action)
        {
            //  Set the action.
            this.action = action;
        }

        public Command(Action<object?> parameterizedAction)
        {
            //  Set the action.
            this.parameterizedAction = parameterizedAction;
        }

        public Command(Action action, Func<bool> canExecuteFunc)
        {
            this.action = action;
            this.canExecuteFunc = canExecuteFunc;
        }

        public Command(Action<object?> parameterizedAction, Func<object?, bool> parameterizedCanExecuteFunc)
        {
            this.parameterizedAction = parameterizedAction;
            this.parameterizedCanExecuteFunc = parameterizedCanExecuteFunc;
        }

        public event EventHandler? CanExecuteChanged;

        bool ICommand.CanExecute(object? parameter)
        {
            return InvokeCanExecuteFunction(parameter);
        }

        void ICommand.Execute(object? parameter)
        {
            InvokeAction(parameter);
        }

        private bool InvokeCanExecuteFunction(object? parameter)
        {
            if(canExecuteFunc != null)
                return canExecuteFunc.Invoke();
            if(parameterizedCanExecuteFunc != null)
                return parameterizedCanExecuteFunc(parameter);
            return true;
        }

        protected void InvokeAction(object? param)
        {
            Action? theAction = action;
            Action<object?>? theParameterizedAction = parameterizedAction;

            if(theAction != null)
                theAction();
            else
                theParameterizedAction?.Invoke(param);
        }
    }
}
