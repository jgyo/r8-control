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
        public readonly Dictionary<string, Action<bool>> RegisteredUpAndDownKeyCommands = [];

        public static CommandRegistry Instance = new CommandRegistry();

        /// <summary>
        /// Polls the key.
        /// </summary>
        /// <param name="explicitHK">The explicit (left or right) hotkey.</param>
        /// <param name="generalHK">The generic hotkey.</param>
        /// <param name="isDownKey">If true, the key is down.</param>
        public void PollKey(HotKey explicitHK, HotKey generalHK, bool isDownKey)
        {
            // Get a command bound to the key
            if(!CommandByHotKeys.TryGetValue(explicitHK.KeyStr, out string? command))
            {
                if(!CommandByHotKeys.TryGetValue(generalHK.KeyStr, out string? command2))
                {
                    return;
                }
                else
                {
                    command = command2;
                }
            }

            // if the command has a registered action, call it.
            if(isDownKey && RegisteredDownKeyCommands.TryGetValue(command, out Action? value))
            {
                value();
                return;
            }

            if(RegisteredUpAndDownKeyCommands.TryGetValue(command, out Action<bool>? action))
            {
                action(isDownKey);
            }
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
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void SubscribeToCommand(string command, Action action)
        {
            if(string.IsNullOrEmpty(command))
                throw new ArgumentNullException(nameof(command));

            if(RegisteredDownKeyCommands.ContainsKey(command))
                throw new ArgumentException($"{command} is already registered.");

            RegisteredDownKeyCommands.Add(command, action);
        }

        /// <summary>
        /// Subscribes an action to command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void SubscribeToCommand(string command, Action<bool> action)
        {
            if(string.IsNullOrEmpty(command))
                throw new ArgumentNullException(nameof(command));

            if(RegisteredUpAndDownKeyCommands.ContainsKey(command))
                throw new ArgumentException($"{command} is already registered.");

            RegisteredUpAndDownKeyCommands.Add(command, action);
        }

        /// <summary>
        /// Removes a subscription for a command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <exception cref="Exception">Raised if the command cannot be removed.</exception>
        public void UnsubscribeToCommand(string command)
        {
            try
            {
                if(RegisteredDownKeyCommands.ContainsKey(command))
                    RegisteredDownKeyCommands.Remove(command);
                else
                    RegisteredUpAndDownKeyCommands.Remove(command);
            }
            catch(Exception ex)
            {
                throw new Exception($"Unable to remove the {command} command key.", ex);
            }
        }
    }
}
