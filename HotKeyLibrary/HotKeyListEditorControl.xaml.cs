using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace HotKeyLibrary
{
    /// <summary>
    /// Interaction logic for HotKeyListEditorControl.xaml
    /// </summary>
    public partial class HotKeyListEditorControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty CurrentCommandKeysProperty =
    DependencyProperty.Register(
            "CurrentCommandKeys",
            typeof(List<NamedCommandKeys>),
            typeof(HotKeyListEditorControl),
            new PropertyMetadata(null));
        public static readonly DependencyProperty DefaultCommandKeysProperty =
    DependencyProperty.Register(
            "DefaultCommandKeys",
            typeof(List<NamedCommandKeys>),
            typeof(HotKeyListEditorControl),
            new PropertyMetadata(null));
        public static readonly DependencyProperty WorkingCommandKeysProperty =
    DependencyProperty.Register(
            "WorkingCommandKeys",
            typeof(List<NamedCommandKeys>),
            typeof(HotKeyListEditorControl),
            new PropertyMetadata(null));
        private bool dataHasErrors;
        private bool hasChanges;
        private bool isKeyItemSelected;
        private bool workingCommandKeysHasErrors;

        public HotKeyListEditorControl()
        {
            InitializeComponent();

            this.dataGrid.SelectionChanged += OnDataGridSelectionChanged;

            this.primaryKey.PropertyChanged += Key_PropertyChanged;
            this.altKey.PropertyChanged += Key_PropertyChanged;
            HasChanges = false;
        }

        public event EventHandler? CancelButtonClicked;

        public event PropertyChangedEventHandler? PropertyChanged;

        public event EventHandler? SubmitButtonClicked;

        public List<NamedCommandKeys> CurrentCommandKeys
        {
            get
            {
                return (List<NamedCommandKeys>)GetValue(CurrentCommandKeysProperty);
            }
            set
            {
                SetValue(CurrentCommandKeysProperty, value);
            }
        }

        public bool DataHasErrors
        {
            get
            {
                return dataHasErrors;
            }
            private set
            {
                SetProperty(ref dataHasErrors, value);
            }
        }

        public List<NamedCommandKeys> DefaultCommandKeys
        {
            get
            {
                return (List<NamedCommandKeys>)GetValue(DefaultCommandKeysProperty);
            }
            set
            {
                SetValue(DefaultCommandKeysProperty, value);
            }
        }

        public bool HasChanges
        {
            get
            {
                return hasChanges;
            }
            private set
            {
                SetProperty(ref hasChanges, value);
            }
        }

        public bool IsKeyItemSelected
        {
            get
            {
                return isKeyItemSelected;
            }
            set
            {
                SetProperty(ref isKeyItemSelected, value);
            }
        }

        public List<NamedCommandKeys> WorkingCommandKeys
        {
            get
            {
                return (List<NamedCommandKeys>)GetValue(WorkingCommandKeysProperty);
            }
            set
            {
                SetValue(WorkingCommandKeysProperty, value);
            }
        }

        public bool WorkingCommandKeysHasErrors
        {
            get
            {
                return workingCommandKeysHasErrors;
            }
            private set
            {
                SetProperty(ref workingCommandKeysHasErrors, value);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CancelButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            if(this.dataGrid.SelectedItem is not NamedCommandKeys namedKey)
            {
                throw new Exception("SelectedItem is null");
            }

            namedKey.Key = null;
            namedKey.AltKey = null;
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            foreach(var key in WorkingCommandKeys)
            {
                key.Key = null;
                key.AltKey = null;
            }
        }

        private void Default_Click(object sender, RoutedEventArgs e)
        {
            if(this.dataGrid.SelectedItem is not NamedCommandKeys namedKey)
            {
                throw new Exception("SelectedItem is null");
            }

            var index = this.dataGrid.SelectedIndex;

            if(index < 0 || index >= this.DefaultCommandKeys.Count)
            {
                throw new IndexOutOfRangeException("SelectedIndex is out of range.");
            }

            var defKey = DefaultCommandKeys[index];

            if(namedKey.Name != defKey.Name)
            {
                throw new InvalidOperationException($"Key names mismatch: {namedKey.Name} != {defKey.Name}");
            }

            namedKey.Key = defKey.Key;
            namedKey.AltKey = defKey.AltKey;
        }

        private void DefaultAll_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < WorkingCommandKeys.Count; i++)
            {
                if(WorkingCommandKeys[i].Name != DefaultCommandKeys[i].Name)
                {
                    throw new InvalidOperationException(
                        $"Name mismatch: {WorkingCommandKeys[i].Name} != {DefaultCommandKeys[i].Name}");
                }

                WorkingCommandKeys[i].Key = DefaultCommandKeys[i].Key;
                WorkingCommandKeys[i].AltKey = DefaultCommandKeys[i].AltKey;
            }
        }

        private void Key_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if(WorkingCommandKeys == null)
            {
                WorkingCommandKeysHasErrors = false;
                return;
            }

            WorkingCommandKeysHasErrors = WorkingCommandKeys.Where(
                m => (m.Key != null && !m.Key.IsUnique) || (m.AltKey != null && !m.AltKey.IsUnique))
                .Any();

            HasChanges = true;
        }

        private void OnDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsKeyItemSelected = dataGrid.SelectedIndex >= 0;
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            if(this.dataGrid.SelectedItem is not NamedCommandKeys namedKey)
            {
                throw new Exception("SelectedItem is null");
            }

            var index = this.dataGrid.SelectedIndex;

            if(index < 0 || index >= this.CurrentCommandKeys.Count)
            {
                throw new IndexOutOfRangeException("SelectedIndex is out of range.");
            }

            var curKey = CurrentCommandKeys[index];

            if(namedKey.Name != curKey.Name)
            {
                throw new InvalidOperationException($"Key names mismatch: {namedKey.Name} != {curKey.Name}");
            }

            namedKey.Key = curKey.Key;
            namedKey.AltKey = curKey.AltKey;
        }

        private void ResetAll_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < WorkingCommandKeys.Count; i++)
            {
                if(WorkingCommandKeys[i].Name != CurrentCommandKeys[i].Name)
                {
                    throw new InvalidOperationException(
                        $"Name mismatch: {WorkingCommandKeys[i].Name} != {CurrentCommandKeys[i].Name}");
                }

                WorkingCommandKeys[i].Key = CurrentCommandKeys[i].Key;
                WorkingCommandKeys[i].AltKey = CurrentCommandKeys[i].AltKey;
            }
        }

        private void SetProperty(ref bool isKeyItemSelected, bool value, [CallerMemberName] string? propertyName = null)
        {
            if(isKeyItemSelected == value)
            {
                return;
            }

            isKeyItemSelected = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            CurrentCommandKeys.Clear();
            CurrentCommandKeys.AddRange(WorkingCommandKeys);
            SubmitButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if(e.Property.Name == "CurrentCommandKeys")
            {
                var working = new List<NamedCommandKeys>(CurrentCommandKeys.Count);
                foreach(NamedCommandKeys key in CurrentCommandKeys)
                {
                    working.Add(new NamedCommandKeys(key));
                }

                WorkingCommandKeys = working;
            }

            if(e.Property.Name == "WorkingCommandKeys")
            {
                NamedCommandKeys.WorkingCommandKeys = WorkingCommandKeys;
            }

            base.OnPropertyChanged(e);
        }
    }
}
