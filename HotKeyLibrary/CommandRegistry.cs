using System;
using System.Linq;

namespace HotKeyLibrary
{
    public class CommandRegistry
    {
        private readonly Dictionary<string, string> CommandByHotKeys = [];
        private readonly Dictionary<string, Action> RegisteredDownKeyCommands = [];
        private readonly Dictionary<string, Action> RegisteredUpKeyCommands = [];

        public void PollKey(HotKey expHK, HotKey genHK, bool isDownKey)
        {
            if(!CommandByHotKeys.TryGetValue(expHK.KeyStr, out string? command))
            {
                if(!CommandByHotKeys.TryGetValue(genHK.KeyStr, out string? command2))
                {
                    return;
                }
                else
                {
                    command = command2;
                }
            }

            Dictionary<string, Action> registry;

            if(isDownKey)
            {
                registry = RegisteredDownKeyCommands;
            }
            else
            {
                registry = RegisteredUpKeyCommands;
            }

            if(registry.TryGetValue(command, out Action? action))
                action();
        }

        public void RefreshHotKeyList(List<NamedCommandKeys> keys)
        {
            CommandByHotKeys.Clear();
            foreach(NamedCommandKeys key in keys)
            {
                if(key.Key != null)
                    CommandByHotKeys.Add(key.Key.KeyStr, key.Name);
                if(key.AltKey != null)
                    CommandByHotKeys.Add(key.AltKey.KeyStr, key.Name);
            }
        }

        public void SubscribeToCommand(string command, Action action, bool isDownKey)
        {
            if(string.IsNullOrEmpty(command))
                throw new ArgumentNullException(nameof(command));

            Dictionary<string, Action> registry;

            if(isDownKey)
            {
                registry = RegisteredDownKeyCommands;
            }
            else
            {
                registry = RegisteredUpKeyCommands;
            }

            if(registry.ContainsKey(command))
                throw new ArgumentException($"{command} is already registered.");

            registry.Add(command, action);
        }

        public void UnsubscribeToCommand(string command, bool isDownKey)
        {
            Dictionary<string, Action> registry;

            if(isDownKey)
            {
                registry = RegisteredDownKeyCommands;
            }
            else
            {
                registry = RegisteredUpKeyCommands;
            }

            registry.Remove(command);
        }
    }
}
