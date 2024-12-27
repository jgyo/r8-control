using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotKeyLibrary
{
    /// <summary>
    /// Interaction logic for HotKeyListEditorControl.xaml
    /// </summary>
    public partial class HotKeyListEditorControl : UserControl, INotifyPropertyChanged
    {
        /// <summary>
        /// The current command keys property.
        /// </summary>
        public static readonly DependencyProperty CurrentCommandKeysProperty =
    DependencyProperty.Register(
            "CurrentCommandKeys",
            typeof(List<NamedCommandKeys>),
            typeof(HotKeyListEditorControl),
            new PropertyMetadata(null));
        /// <summary>
        /// The default command keys property.
        /// </summary>
        public static readonly DependencyProperty DefaultCommandKeysProperty =
    DependencyProperty.Register(
            "DefaultCommandKeys",
            typeof(List<NamedCommandKeys>),
            typeof(HotKeyListEditorControl),
            new PropertyMetadata(null));
        /// <summary>
        /// Working command keys property.
        /// </summary>
        public static readonly DependencyProperty WorkingCommandKeysProperty =
    DependencyProperty.Register(
            "WorkingCommandKeys",
            typeof(List<NamedCommandKeys>),
            typeof(HotKeyListEditorControl),
            new PropertyMetadata(null));
        /// <summary>
        /// True if the data has errors.
        /// </summary>
        private bool dataHasErrors;
        /// <summary>
        /// True if the data has changes.
        /// </summary>
        private bool hasChanges;
        /// <summary>
        /// Is key item selected.
        /// </summary>
        private bool isKeyItemSelected;
        /// <summary>
        /// Last key box with focus.
        /// </summary>
        private HotKeyEditorControl? lastKeyBoxWithFocus;
        /// <summary>
        /// Working command keys has errors.
        /// </summary>
        private bool workingCommandKeysHasErrors;

        /// <summary>
        /// Initializes a new instance of the <see cref="HotKeyListEditorControl"/> class.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the current command keys.
        /// </summary>
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
        /// <summary>
        /// Gets  a value indicating whether the data has errors.
        /// </summary>
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
        /// <summary>
        /// Gets or sets the default command keys.
        /// </summary>
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
        /// <summary>
        /// Gets a value indicating whether the data has changes.
        /// </summary>
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
        /// <summary>
        /// Gets or sets a value indicating whether a key item is selected.
        /// </summary>
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
        /// <summary>
        /// Gets or sets the working command keys.
        /// </summary>
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
        /// <summary>
        /// Gets a value indicating whether the working command keys has errors.
        /// </summary>
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

        /// <summary>
        /// Cancel button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CancelButtonClicked?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Clear button click event,
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        /// <exception cref="Exception"></exception>
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid.SelectedItem is not NamedCommandKeys namedKey)
            {
                throw new Exception("SelectedItem is null");
            }

            namedKey.Key = null;
            namedKey.AltKey = null;
        }
        /// <summary>
        /// Clear all button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var key in WorkingCommandKeys)
            {
                key.Key = null;
                key.AltKey = null;
            }
        }
        /// <summary>
        /// Default button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private void Default_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid.SelectedItem is not NamedCommandKeys namedKey)
            {
                throw new Exception("SelectedItem is null");
            }

            var index = this.dataGrid.SelectedIndex;

            if (index < 0 || index >= this.DefaultCommandKeys.Count)
            {
                throw new IndexOutOfRangeException("SelectedIndex is out of range.");
            }

            var defKey = DefaultCommandKeys[index];

            if (namedKey.Name != defKey.Name)
            {
                throw new InvalidOperationException($"Key names mismatch: {namedKey.Name} != {defKey.Name}");
            }

            namedKey.Key = defKey.Key;
            namedKey.AltKey = defKey.AltKey;
        }
        /// <summary>
        /// Default all button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        /// <exception cref="InvalidOperationException"></exception>
        private void DefaultAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < WorkingCommandKeys.Count; i++)
            {
                if (WorkingCommandKeys[i].Name != DefaultCommandKeys[i].Name)
                {
                    throw new InvalidOperationException(
                        $"Name mismatch: {WorkingCommandKeys[i].Name} != {DefaultCommandKeys[i].Name}");
                }

                WorkingCommandKeys[i].Key = DefaultCommandKeys[i].Key;
                WorkingCommandKeys[i].AltKey = DefaultCommandKeys[i].AltKey;
            }
        }
        /// <summary>
        /// Hots key box got focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        private void HotKeyBoxGotFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            lastKeyBoxWithFocus = (HotKeyEditorControl)sender;
        }
        /// <summary>
        /// Keys property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        private void Key_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (WorkingCommandKeys == null)
            {
                WorkingCommandKeysHasErrors = false;
                return;
            }

            WorkingCommandKeysHasErrors = WorkingCommandKeys.Where(
                m => (m.Key != null && !m.Key.IsUnique) || (m.AltKey != null && !m.AltKey.IsUnique))
                .Any();

            HasChanges = true;
        }
        /// <summary>
        /// On data grid selection changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        private void OnDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsKeyItemSelected = dataGrid.SelectedIndex >= 0;
            if (lastKeyBoxWithFocus == null)
                primaryKey.HotKeyTextBox.Focus();
            else
                lastKeyBoxWithFocus.HotKeyTextBox.Focus();
        }
        /// <summary>
        /// Reset button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            if (this.dataGrid.SelectedItem is not NamedCommandKeys namedKey)
            {
                throw new Exception("SelectedItem is null");
            }

            var index = this.dataGrid.SelectedIndex;

            if (index < 0 || index >= this.CurrentCommandKeys.Count)
            {
                throw new IndexOutOfRangeException("SelectedIndex is out of range.");
            }

            var curKey = CurrentCommandKeys[index];

            if (namedKey.Name != curKey.Name)
            {
                throw new InvalidOperationException($"Key names mismatch: {namedKey.Name} != {curKey.Name}");
            }

            namedKey.Key = curKey.Key;
            namedKey.AltKey = curKey.AltKey;
        }
        /// <summary>
        /// Reset all button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        /// <exception cref="InvalidOperationException"></exception>
        private void ResetAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < WorkingCommandKeys.Count; i++)
            {
                if (WorkingCommandKeys[i].Name != CurrentCommandKeys[i].Name)
                {
                    throw new InvalidOperationException(
                        $"Name mismatch: {WorkingCommandKeys[i].Name} != {CurrentCommandKeys[i].Name}");
                }

                WorkingCommandKeys[i].Key = CurrentCommandKeys[i].Key;
                WorkingCommandKeys[i].AltKey = CurrentCommandKeys[i].AltKey;
            }
        }
        /// <summary>
        /// Set the property.
        /// </summary>
        /// <param name="field">If true, is key item selected                    .</param>
        /// <param name="value">If true, value.</param>
        /// <param name="propertyName">The property name.</param>
        private void SetProperty(ref bool field, bool value, [CallerMemberName] string? propertyName = null)
        {
            if (field == value)
            {
                return;
            }

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Submit button click event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            CurrentCommandKeys.Clear();
            CurrentCommandKeys.AddRange(WorkingCommandKeys);
            SubmitButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// On property changed.
        /// </summary>
        /// <param name="e">The E.</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "CurrentCommandKeys")
            {
                var working = new List<NamedCommandKeys>(CurrentCommandKeys.Count);
                foreach (NamedCommandKeys key in CurrentCommandKeys)
                {
                    working.Add(new NamedCommandKeys(key));
                }

                WorkingCommandKeys = working;
            }

            if (e.Property.Name == "WorkingCommandKeys")
            {
                NamedCommandKeys.WorkingCommandKeys = WorkingCommandKeys;
            }

            base.OnPropertyChanged(e);
        }

    }
}
