using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace HotKeyLibrary
{
    public class HotKey : INotifyPropertyChanged
    {
        private bool isUnique;

        public HotKey(string keyString)
        {
            var parts = keyString.Split('+', StringSplitOptions.TrimEntries);

            this.Key = ObscureKeyConversion.GetKeyFromName(parts[^1]);
            var modifiers = Modifiers.None;
            for(int i = 0; i < parts.Length - 1; i++)
            {
                modifiers |= parts[i] switch
                {
                    "LCtrl" => Modifiers.LeftCtrl,
                    "RCtrl" => Modifiers.RightCtrl,
                    "Ctrl" => Modifiers.Ctrl,
                    "LAlt" => Modifiers.LeftAlt,
                    "RAlt" => Modifiers.RightAlt,
                    "Alt" => Modifiers.Alt,
                    "LWin" => Modifiers.LeftWin,
                    "RWin" => Modifiers.RightWin,
                    "Win" => Modifiers.Win,
                    "LShift" => Modifiers.LeftShift,
                    "RShift" => Modifiers.RightShift,
                    "Shift" => Modifiers.Shift,
                    _ => throw new ArgumentException($"{parts[i]} is not a valid modifier."),
                };
            }

            this.Modifiers = modifiers;
            IsUnique = true;
        }

        public HotKey(Key key, Modifiers modifiers)
        {
            Key = key;
            Modifiers = modifiers;
            IsUnique = true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsSideDependent
        {
            get
            {
                return (Modifiers &
                        (Modifiers.LeftAlt |
                            Modifiers.RightAlt |
                            Modifiers.LeftCtrl |
                            Modifiers.RightCtrl |
                            Modifiers.LeftShift |
                            Modifiers.RightShift |
                            Modifiers.LeftWin |
                            Modifiers.RightWin)) !=
                    Modifiers.None;
            }
        }

        public bool IsUnique
        {
            get
            {
                return isUnique;
            }
            set
            {
                if(isUnique == value)
                {
                    return;
                }

                isUnique = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsUnique)));
            }
        }

        public Key Key { get; }

        public string KeyStr
        {
            get
            {
                return this.ToString();
            }
        }

        public Modifiers Modifiers { get; }

        public override string ToString()
        {
            var str = new StringBuilder();

            if(Modifiers.HasFlag(Modifiers.LeftCtrl))
                str.Append("LCtrl + ");
            if(Modifiers.HasFlag(Modifiers.RightCtrl))
                str.Append("RCtrl + ");
            if(Modifiers.HasFlag(Modifiers.Ctrl))
                str.Append("Ctrl + ");
            if(Modifiers.HasFlag(Modifiers.LeftShift))
                str.Append("LShift + ");
            if(Modifiers.HasFlag(Modifiers.RightShift))
                str.Append("RShift + ");
            if(Modifiers.HasFlag(Modifiers.Shift))
                str.Append("Shift + ");
            if(Modifiers.HasFlag(Modifiers.LeftAlt))
                str.Append("LAlt + ");
            if(Modifiers.HasFlag(Modifiers.RightAlt))
                str.Append("RAlt + ");
            if(Modifiers.HasFlag(Modifiers.Alt))
                str.Append("Alt + ");
            if(Modifiers.HasFlag(Modifiers.LeftWin))
                str.Append("LWin + ");
            if(Modifiers.HasFlag(Modifiers.RightWin))
                str.Append("RWin + ");
            if(Modifiers.HasFlag(Modifiers.Win))
                str.Append("Win + ");

            str.Append(ObscureKeyConversion.GetHumanName(Key));

            return str.ToString();
        }
    }
}