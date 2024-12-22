using System;
using System.Linq;

namespace HotKeyLibrary
{
    public class NamedCommand : Command
    {
        public NamedCommand(string name, Action action) : base(action)
        {
            Name = name;
        }

        public NamedCommand(string name, Action<object?> parameterizedAction) : base(parameterizedAction)
        {
            Name = name;
        }

        public NamedCommand(string name, Action action, Func<bool> canExecuteFunc) : base(action, canExecuteFunc)
        {
            Name = name;
        }

        public NamedCommand(
            string name,
            Action<object?> parameterizedAction,
            Func<object?, bool> parameterizedCanExecuteFunc) : base(parameterizedAction, parameterizedCanExecuteFunc)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
