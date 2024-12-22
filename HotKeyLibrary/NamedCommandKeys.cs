using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace HotKeyLibrary
{
    public class NamedCommandKeys : INotifyPropertyChanging, INotifyPropertyChanged, INotifyDataErrorInfo, IDataErrorInfo
    {
        private HotKey? altKey;
        private HotKey? key;

        public NamedCommandKeys(string name)
        {
            Name = name;
            Error = string.Empty;
            ValidateKeys();
        }

        public NamedCommandKeys(NamedCommandKeys key1)
        {
            Name = key1.Name;
            AltKey = key1.AltKey;
            Key = key1.Key;
            Error = string.Empty;
            ValidateKeys();
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public event PropertyChangedEventHandler? PropertyChanged;

        public event PropertyChangingEventHandler? PropertyChanging;

        public string this[string columnName]
        {
            get
            {
                if(string.IsNullOrEmpty(columnName))
                {
                    return string.Empty;
                }

                if(columnName == "Key" && Key != null && Key.IsUnique == false)
                    return "Primary key is not unique.";

                if(columnName == "AltKey" && AltKey != null && AltKey.IsUnique == false)
                    return "Alternate key is not unique.";

                return string.Empty;
            }
        }

        public HotKey? AltKey
        {
            get
            {
                return altKey;
            }
            set
            {
                SetProperty(ref altKey, value);
            }
        }

        public string Error { get; private set; }

        public bool HasErrors { get; private set; }

        public HotKey? Key
        {
            get
            {
                return key;
            }
            set
            {
                SetProperty(ref key, value);
            }
        }

        public string Name { get; }

        public static List<NamedCommandKeys>? WorkingCommandKeys { get; set; }

        private void Key_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "IsUnique")
            {
                ValidateKeys();
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Key)));
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(AltKey)));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            string? keyStr;
            List<NamedCommandKeys>? commandKeys;
            List<NamedCommandKeys>? commandAltKeys;

            switch(propertyName)
            {
                case "Key":
                    if(Key == null)
                        break;

                    Key.PropertyChanged += Key_PropertyChanged;

                    if(WorkingCommandKeys == null)
                        break;

                    keyStr = Key.KeyStr;
                    commandKeys = WorkingCommandKeys.Where(m => m.Key != null && m.Key.KeyStr == keyStr).ToList();
                    commandAltKeys = WorkingCommandKeys.Where(m => m.AltKey != null && m.AltKey.KeyStr == keyStr)
                        .ToList();

                    if(commandKeys.Count + commandAltKeys.Count > 1)
                    {
                        foreach(var item in commandKeys)
                        {
                            if(item.Key == null)
                                continue;
                            item.Key.IsUnique = false;
                        }

                        foreach(var item in commandAltKeys)
                        {
                            if(item.AltKey == null)
                                continue;
                            item.AltKey.IsUnique = false;
                        }
                    }
                    break;
                case "AltKey":
                    if(AltKey == null)
                        break;

                    AltKey.PropertyChanged += Key_PropertyChanged;

                    if(WorkingCommandKeys == null)
                        break;

                    keyStr = AltKey.KeyStr;
                    commandAltKeys = WorkingCommandKeys.Where(m => m.AltKey != null && m.AltKey.KeyStr == keyStr)
                        .ToList();
                    commandKeys = WorkingCommandKeys.Where(m => m.Key != null && m.Key.KeyStr == keyStr).ToList();

                    if(commandKeys.Count + commandAltKeys.Count > 1)
                    {
                        foreach(var item in commandKeys)
                        {
                            if(item.Key == null)
                                continue;
                            item.Key.IsUnique = false;
                        }

                        foreach(var item in commandAltKeys)
                        {
                            if(item.AltKey == null)
                                continue;
                            item.AltKey.IsUnique = false;
                        }
                    }
                    break;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnPropertyChanging(string propertyName)
        {
            string? keyStr;
            List<NamedCommandKeys>? commandKeys;
            List<NamedCommandKeys>? commandAltKeys;

            switch(propertyName)
            {
                case "Key":
                    if(Key == null || WorkingCommandKeys == null)
                        break;

                    Key.PropertyChanged -= Key_PropertyChanged;

                    keyStr = Key.KeyStr;
                    commandKeys = WorkingCommandKeys.Where(m => m.Key != null && m.Key != Key && m.Key.KeyStr == keyStr)
                        .ToList();
                    commandAltKeys = WorkingCommandKeys.Where(m => m.AltKey != null && m.AltKey.KeyStr == keyStr)
                        .ToList();

                    if(commandKeys.Count + commandAltKeys.Count < 2)
                    {
                        foreach(var item in commandKeys)
                        {
                            if(item.Key == null)
                                continue;
                            item.Key.IsUnique = true;
                        }

                        foreach(var item in commandAltKeys)
                        {
                            if(item.AltKey == null)
                                continue;
                            item.AltKey.IsUnique = true;
                        }
                    }
                    break;
                case "AltKey":
                    if(AltKey == null || WorkingCommandKeys == null)
                        break;

                    AltKey.PropertyChanged -= Key_PropertyChanged;

                    keyStr = AltKey.KeyStr;
                    commandAltKeys = WorkingCommandKeys.Where(
                        m => m.AltKey != null && m.AltKey != AltKey && m.AltKey.KeyStr == keyStr)
                        .ToList();
                    commandKeys = WorkingCommandKeys.Where(m => m.Key != null && m.Key.KeyStr == keyStr).ToList();

                    if(commandKeys.Count + commandAltKeys.Count < 2)
                    {
                        foreach(var item in commandKeys)
                        {
                            if(item.Key == null)
                                continue;
                            item.Key.IsUnique = true;
                        }

                        foreach(var item in commandAltKeys)
                        {
                            if(item.AltKey == null)
                                continue;
                            item.AltKey.IsUnique = true;
                        }
                    }
                    break;
            }

            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        private void ValidateKeys()
        {
            var keyHasErrors = (Key != null && !Key.IsUnique);
            var altKeyHasErrors = (AltKey != null && !AltKey.IsUnique);
            HasErrors = keyHasErrors || altKeyHasErrors;

            var sb = new StringBuilder();
            if(keyHasErrors)
                sb.Append("The primary key");
            if(altKeyHasErrors)
            {
                if(keyHasErrors)
                    sb.Append(" and the alternative key");
                else
                    sb.Append("The alternative key");
            }
            if(keyHasErrors && altKeyHasErrors)
            {
                sb.Append(" are not unique.");
            }
            else if(keyHasErrors || altKeyHasErrors)
            {
                sb.Append(" is not unique.");
            }

            Error = sb.ToString();
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if(EqualityComparer<T>.Default.Equals(storage, value))
                return false;

            this.OnPropertyChanging(propertyName);
            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            return this[propertyName ?? string.Empty];
        }
    }
}
