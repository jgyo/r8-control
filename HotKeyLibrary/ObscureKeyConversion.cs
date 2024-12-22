using System;
using System.Linq;
using System.Windows.Input;

namespace HotKeyLibrary
{
    public class ObscureKeyConversion
    {
        private static readonly Dictionary<string, Key> humanToKey = [];
        private static readonly Dictionary<Key, string> keyToHuman = [];

        static ObscureKeyConversion()
        {
            keyToHuman.Add(Key.Capital, "CapsLock");
            keyToHuman.Add(Key.Next, "PageDown");
            keyToHuman.Add(Key.Oem1, "Semicolon");
            keyToHuman.Add(Key.Oem2, "Question");
            keyToHuman.Add(Key.Oem3, "BackQuote");
            keyToHuman.Add(Key.Oem4, "OpenBracket");
            keyToHuman.Add(Key.Oem5, "Backslash");
            keyToHuman.Add(Key.Oem6, "CloseBracket");
            keyToHuman.Add(Key.Oem7, "Quote");
            keyToHuman.Add(Key.OemComma, "Comma");
            keyToHuman.Add(Key.OemMinus, "Minus");
            keyToHuman.Add(Key.OemPeriod, "Period");
            keyToHuman.Add(Key.OemPlus, "Plus");
            keyToHuman.Add(Key.Prior, "PageUp");
            keyToHuman.Add(Key.Snapshot, "PrintScreen");

            humanToKey.Add("CapsLock", Key.Capital);
            humanToKey.Add("PageDown", Key.Next);
            humanToKey.Add("Semicolon", Key.Oem1);
            humanToKey.Add("Question", Key.Oem2);
            humanToKey.Add("BackQuote", Key.Oem3);
            humanToKey.Add("OpenBracket", Key.Oem4);
            humanToKey.Add("Backslash", Key.Oem5);
            humanToKey.Add("CloseBracket", Key.Oem6);
            humanToKey.Add("Quote", Key.Oem7);
            humanToKey.Add("Comma", Key.OemComma);
            humanToKey.Add("Minus", Key.OemMinus);
            humanToKey.Add("Period", Key.OemPeriod);
            humanToKey.Add("Plus", Key.OemPlus);
            humanToKey.Add("PageUp", Key.Prior);
            humanToKey.Add("PrintScreen", Key.Snapshot);
        }

        public static string GetHumanName(Key key)
        {
            if(keyToHuman.TryGetValue(key, out string? value))
                return value;

            return key.ToString();
        }

        public static Key GetKeyFromName(string name)
        {
            if(humanToKey.TryGetValue(name, out Key value))
                return value;

            return (Key) Enum.Parse(typeof(Key), name);
        }
    }
}
