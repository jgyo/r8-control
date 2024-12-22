using System;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace HotKeyLibrary
{
    public class HotKeyValidationRule : ValidationRule
    {
        private static int GetKeyCount(string keyString)
        {
            if(string.IsNullOrEmpty(keyString) || NamedCommandKeys.CurrentCommands == null)
                return 0;

            return NamedCommandKeys.CurrentCommands.Where(m => m.Key != null && m.Key.KeyStr == keyString).Count() +
                NamedCommandKeys.CurrentCommands
                    .Where(m => m.AltKey != null && m.AltKey.KeyStr == keyString)
                    .Count();
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var command = value as HotKey;
            if(command == null)
            {
                throw new ArgumentException("value must be a NamedCommandKeys object.");
            }

            var keyString = command.KeyStr;
            int key1Count = GetKeyCount(keyString);

            if(key1Count > 1)
            {
                return new ValidationResult(false, "Not unique");
            }
            return ValidationResult.ValidResult;
        }
    }
}
