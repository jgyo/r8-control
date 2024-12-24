using System;
using System.Linq;

namespace HotKeyLibrary
{
    /// <summary>
    /// The command registry.
    /// </summary>
    public class CommandRegistry
    {
        private readonly Dictionary<string, string> CommandByHotKeys = [];
        private readonly Dictionary<string, Action> RegisteredDownKeyCommands = [];
        private readonly Dictionary<string, Action> RegisteredUpKeyCommands = [];

        /// <summary>
        /// Polls the key.
        /// </summary>
        /// <param name="expHK">The explicit (left or right) hotkey.</param>
        /// <param name="genHK">The generic hotkey.</param>
        /// <param name="isDownKey">If true, the key is down.</param>
        public void PollKey(HotKey expHK, HotKey genHK, bool isDownKey)
        {
            // Get a command bound to the key
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

            // if the command has a registered action, call it.
            if(registry.TryGetValue(command, out Action? action))
                action();
        }

        /// <summary>
        /// Refreshes the hotkey list with new bindings.
        /// </summary>
        /// <param name="keys">The keys.</param>
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

        /// <summary>
        /// Subscribes an action to a command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="action">The action.</param>
        /// <param name="isDownKey">If true, the subscription is for a key down event.</param>
        /// <exception cref="ArgumentNullException">Raised if command is null or empty.</exception>
        /// <exception cref="ArgumentException">Raised if the command is already registered.</exception>
        public void SubscribeToCommand(string command, Action action, bool isDownKey = true)
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

        /// <summary>
        /// Removes a subscription for a command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="isDownKey">If true, the subscription was for a key down event.</param>
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
