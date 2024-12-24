using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotKeyLibrary
{
    /// <summary>
    /// Interaction logic for HotKeyEditorControl.xaml
    /// </summary>
    public partial class HotKeyEditorControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty CurrentKeysProperty =
    DependencyProperty.Register(
            "CurrentKeys",
            typeof(List<NamedCommandKeys>),
            typeof(HotKeyEditorControl),
            new PropertyMetadata(null));
        public static readonly DependencyProperty HotKeyProperty =
    DependencyProperty.Register(
            "HotKey",
            typeof(HotKey),
            typeof(HotKeyEditorControl),
            new FrameworkPropertyMetadata(default(HotKey), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty SideDependentProperty =
    DependencyProperty.Register(
            "SideDependent",
            typeof(bool),
            typeof(HotKeyEditorControl),
            new PropertyMetadata(false));
        private Modifiers modifiersState = Modifiers.None;

        public HotKeyEditorControl()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public HotKey? HotKey
        {
            get
            {
                return (HotKey)GetValue(HotKeyProperty);
            }
            set
            {
                SetValue(HotKeyProperty, value);
            }
        }

        public bool SideDependent
        {
            get
            {
                return (bool)GetValue(SideDependentProperty);
            }
            set
            {
                SetValue(SideDependentProperty, value);
            }
        }

        private void CheckBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            HotKeyTextBox.Focus();
        }

        private void HotKeyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            var modifiers = Keyboard.Modifiers;
            var key = e.Key;

            // when Alt is pressed, SystemKey (instead of Key) is used instead
            if(key == Key.System)
                key = e.SystemKey;

            // Handle delete, backspace and escape if no modifiers used
            if(modifiers == ModifierKeys.None && (key == Key.Escape))
            {
                HotKey = null;
                modifiersState = Modifiers.None;
                return;
            }

            if(key == Key.Clear || key == Key.OemClear || key == Key.Apps || key == Key.LWin || key == Key.RWin)
                return;

            if(key == Key.LeftCtrl ||
                key == Key.RightCtrl ||
                key == Key.LeftAlt ||
                key == Key.RightAlt ||
                key == Key.LeftShift ||
                key == Key.RightShift)
            {
                switch(key)
                {
                    case Key.LeftCtrl:
                        modifiersState |= SideDependent ? Modifiers.LeftCtrl : Modifiers.Ctrl;
                        break;
                    case Key.RightCtrl:
                        modifiersState |= SideDependent ? Modifiers.RightCtrl : Modifiers.Ctrl;
                        break;
                    case Key.LeftAlt:
                        modifiersState |= SideDependent ? Modifiers.LeftAlt : Modifiers.Alt;
                        break;
                    case Key.RightAlt:
                        modifiersState |= SideDependent ? Modifiers.RightAlt : Modifiers.Alt;
                        break;
                    case Key.LeftShift:
                        modifiersState |= SideDependent ? Modifiers.LeftShift : Modifiers.Shift;
                        break;
                    case Key.RightShift:
                        modifiersState |= SideDependent ? Modifiers.RightShift : Modifiers.Shift;
                        break;
                }

                return;
            }

            // Update the value
            HotKey = new HotKey(key, modifiersState);
        }

        private void HotKeyTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            var key = e.Key;

            if(key == Key.System)
                key = e.SystemKey;

            switch(key)
            {
                case Key.LeftCtrl:
                    modifiersState &= Modifiers.All ^ (SideDependent ? Modifiers.LeftCtrl : Modifiers.Ctrl);
                    break;
                case Key.RightCtrl:
                    modifiersState &= Modifiers.All ^ (SideDependent ? Modifiers.RightCtrl : Modifiers.Ctrl);
                    break;
                case Key.LeftAlt:
                    modifiersState &= Modifiers.All ^ (SideDependent ? Modifiers.LeftAlt : Modifiers.Alt);
                    break;
                case Key.RightAlt:
                    modifiersState &= Modifiers.All ^ (SideDependent ? Modifiers.RightAlt : Modifiers.Alt);
                    break;
                case Key.LeftShift:
                    modifiersState &= Modifiers.All ^ (SideDependent ? Modifiers.LeftShift : Modifiers.Shift);
                    break;
                case Key.RightShift:
                    modifiersState &= Modifiers.All ^ (SideDependent ? Modifiers.RightShift : Modifiers.Shift);
                    break;
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if(e.Property.Name == nameof(HotKey))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HotKey)));
            }

            base.OnPropertyChanged(e);
        }
    }
}
