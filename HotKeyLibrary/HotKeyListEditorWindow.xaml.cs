using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace HotKeyLibrary
{
    /// <summary>
    /// Interaction logic for HotKeyListEditorWindow.xaml
    /// </summary>
    public partial class HotKeyListEditorWindow : Window
    {
        public static readonly DependencyProperty CurrentKeysProperty =
    DependencyProperty.Register(
            "CurrentKeys",
            typeof(List<NamedCommandKeys>),
            typeof(HotKeyListEditorWindow),
            new PropertyMetadata(null));
        public static readonly DependencyProperty DefaultKeysProperty =
    DependencyProperty.Register(
            "DefaultKeys",
            typeof(List<NamedCommandKeys>),
            typeof(HotKeyListEditorWindow),
            new PropertyMetadata(null));

        public HotKeyListEditorWindow(List<NamedCommandKeys> defaultKeys, List<NamedCommandKeys> currentKeys)
        {
            InitializeComponent();

            DefaultKeys = defaultKeys;
            CurrentKeys = currentKeys;
        }

        public List<NamedCommandKeys> CurrentKeys
        {
            get
            {
                return (List<NamedCommandKeys>)GetValue(CurrentKeysProperty);
            }
            set
            {
                SetValue(CurrentKeysProperty, value);
            }
        }

        public List<NamedCommandKeys> DefaultKeys
        {
            get
            {
                return (List<NamedCommandKeys>)GetValue(DefaultKeysProperty);
            }
            set
            {
                SetValue(DefaultKeysProperty, value);
            }
        }

        private void Editor_CancelButtonClicked(object sender, EventArgs e)
        {
            // No changes wanted. Close the window
            this.DialogResult = false;
            this.Close();
        }

        private void Editor_SubmitButtonClicked(object sender, EventArgs e)
        {
            // User wishes to save changes
            this.CurrentKeys = this.editor.WorkingCommandKeys;
            this.DialogResult = true;
            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if(editor.HasChanges &&
                DialogResult != true &&
                MessageBox.Show(
                    "Do you really want to close the Hotkey Editor without saving your changes?",
                    "Window Closing",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) ==
                MessageBoxResult.No)
            {
                e.Cancel = true;
            }

            base.OnClosing(e);
        }
    }
}
