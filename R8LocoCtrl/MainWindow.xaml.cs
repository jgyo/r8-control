//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Xcoder Software">
//     Author: Gil Yoder
//     Copyright (c) Xcoder Software. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using R8LocoCtrl.Interface;
using R8LocoCtrl.ViewModel;
using Syncfusion.Windows.PdfViewer;
using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools.Controls;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows;

namespace R8LocoCtrl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string DEFAULT_STATE_FILENAME = "DefaultState.xml";
        const string PERSIST_STATE_FILENAME = "ClosingState.xml";
        const double DEFAULT_WIDTH = 1230;
        const double DEFAULT_HEIGHT = 540;

        private readonly ProgramPropertiesViewModel progProperties;

        public MainWindow()
        {
            InitializeComponent();
            dockingManager.Loaded += DockingManager_Loaded;
            dockingManager.WindowClosing += DockingManager_WindowClosing;
            dockingManager.CloseButtonClick += DockingManager_CloseButtonClick;
            this.DataContextChanged += MainWindow_DataContextChanged;
            ConfigureDataContext();
            progProperties = (ProgramPropertiesViewModel)this.FindResource("programProperties");
            progProperties.SubmitProperties += ProgProperties_SubmitProperties;
            progProperties.PropertyChanged += ProgProperties_PropertyChanged;
            SetProgramProperties(progProperties);
            SetSpeedometerProperties(progProperties);
            ConfigureGradeMapMenu();

            if (CR8ID.Default.PersistState)
            {
                this.Top = CR8ID.Default.Top;
                this.Left = CR8ID.Default.Left;
                this.Height = CR8ID.Default.Height;
                this.Width = CR8ID.Default.Width;
                if (CR8ID.Default.Maximized)
                {
                    this.WindowState = WindowState.Maximized;
                }
            }

            var version = GetType().Assembly.GetName().Version!.ToString();
            sbVersion.Text = $"Version: {version}";
        }

        private void ConfigureDataContext()
        {
            if (DataContext is DockingManagerViewModel dmvm)
            {
                dmvm.ActivateWindow += Dmvm_ActivateWindow;
                dmvm.DefaultState += Dmvm_LoadDefaultState;
                dmvm.LoadMap += Dmvm_LoadMap;
                dmvm.OpenGradeMapWindow += Dmvm_OpenGradeMapWindow;
                dmvm.PersistState += Dmvm_PersistState;
                dmvm.IsPersistStateSet = CR8ID.Default.PersistState;
            }
        }

        private void ConfigureGradeMapMenu()
        {
            if (!progProperties.IsGradeMapPathValid)
            {
                this.GradeMapsMenu.Items.Clear();
                this.GradeMapsMenu.ToolTip = "Requires configuration in setup.";
                return;
            }

            this.GradeMapsMenu.ToolTip = null;
            var files = Directory.EnumerateFiles(progProperties.GradeMapPath!, "*.pdf");
            var dmvm = (DockingManagerViewModel)DataContext;
            foreach (var filePath in files)
            {
                bool foundMatch = false;
                try
                {
                    foundMatch = GradeMapRegEx().IsMatch(filePath);
                    if (foundMatch)
                    {
                        string? menuText = null;
                        menuText = GradeMapRegEx().Match(filePath).Groups["Name"].Value;
                        menuText = menuText.Replace('_', ' ').Trim();

                        var newMenuItem = new MenuItemAdv()
                        {
                            Header = menuText,
                            CommandParameter = filePath,
                            Command = dmvm.OpenGradeMapCommand
                        };
                        this.GradeMapsMenu.Items.Add(newMenuItem);
                    }
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void Dmvm_ActivateWindow(object? sender, Run8Windows e)
        {
            this.dockingManager.ActivateWindow(e.ToString());
        }

        private void Dmvm_LoadDefaultState(object? sender, EventArgs e)
        {
            this.dockingManager.LoadDockState(DEFAULT_STATE_FILENAME);
            this.Width = DEFAULT_WIDTH;
            this.Height = DEFAULT_HEIGHT;
            this.WindowState = WindowState.Normal;
        }

        private async void Dmvm_LoadMap(object? sender, string[] e)
        {
            var filename = e[0];
            var filepath = e[2];
            var webpath = e[1];

            if (!File.Exists(filepath))
            {
                // Try to download it
                try
                {
                    using var client = new HttpClient();
                    var content = (await client.GetAsync(webpath)).Content;

                    using var file = File.Create(filepath);
                    var stream = await content.ReadAsStreamAsync();
                    await stream.CopyToAsync(file);
                }
                catch
                {
                    MessageBox.Show(
                        $"Unable to download {filename}. Please check your internet connection.",
                        "HTTP Error");
                    return;
                }
            }

            var docStream = File.OpenRead(filepath);

            var pdfViewer = new PdfViewerControl() { Name = filename.Replace('.', '_'), ItemSource = docStream };
            dockingManager.Children.Add(pdfViewer);
            DockingManager.SetHeader(pdfViewer, filename);
            DockingManager.SetState(pdfViewer, DockState.Document);
        }

        private void Dmvm_OpenGradeMapWindow(object? sender, string filePath)
        {
            var filename = Path.GetFileName(filePath);
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filename);

            try
            {
                var docStream = File.OpenRead(filePath);

                var name = filename.Split('.')[0].Replace(' ', '_');

                var pdfViewer = new PdfViewerControl(){ Name = name, ItemSource = docStream };


                dockingManager.Children.Add(pdfViewer);
                DockingManager.SetHeader(pdfViewer, filename); // filename);
                DockingManager.SetState(pdfViewer, DockState.Document);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Grade Map File Error");
            }
        }

        private void Dmvm_PersistState(object? sender, bool e)
        {
            CR8ID.Default.PersistState = !e;
        }

        private void DockingManager_CloseButtonClick(object sender, CloseButtonEventArgs e)
        {
            var content = (e.TargetItem as PdfViewerControl);
            if (content != null)
            {
                dockingManager.Children.Remove(content);
            }
        }

        private void DockingManager_Loaded(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(DEFAULT_STATE_FILENAME))
                dockingManager.SaveDockState(DEFAULT_STATE_FILENAME);

            if (CR8ID.Default.PersistState && File.Exists(PERSIST_STATE_FILENAME))
                dockingManager.LoadDockState(PERSIST_STATE_FILENAME);
        }

        private void DockingManager_WindowClosing(object sender, WindowClosingEventArgs e)
        {
            var content = (e.TargetItem as PdfViewerControl);
            if (content != null)
            {
                dockingManager.Children.Remove(content);
            }
        }

        [GeneratedRegex(@"^.*\\(?:(RUN 8|RUN8|))(?<Name>.*)(?:Grade.Map\.pdf)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture | RegexOptions.Singleline, "en-US")]
        private static partial Regex GradeMapRegEx();

        private void MainWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                if (e.OldValue is DockingManagerViewModel dmvm)
                {
                    dmvm.ActivateWindow -= Dmvm_ActivateWindow;
                    dmvm.DefaultState -= Dmvm_LoadDefaultState;
                    dmvm.LoadMap -= Dmvm_LoadMap;
                }
            }

            ConfigureDataContext();
        }

        private void ProgProperties_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsGradeMapPathValid")
            {
                ConfigureGradeMapMenu();
            }
        }

        private void ProgProperties_SubmitProperties(object? sender, ProgramPropertiesViewModel e)
        {
            SetSpeedometerProperties(e);
            DockingManager.SetState(this.Setup, DockState.Hidden);
        }

        private static void SetProgramProperties(ProgramPropertiesViewModel properties)
        {
            properties.PressureReference = CR8ID.Default.PressureReference;
            properties.GradeMapPath = CR8ID.Default.GradeMapPath;
            properties.MaximumCautionSpeed = CR8ID.Default.MaxCautionSpeed;
            properties.MaximumSafeSpeed = CR8ID.Default.MaxSafeSpeed;
            properties.MaximumSpeedometerSpeed = CR8ID.Default.MaxSpeedOSpeed;
        }

        private void SetSpeedometerProperties(ProgramPropertiesViewModel properties)
        {
            var speedometer = (SpeedometerSettingsViewModel)this.FindResource("speedoViewModel");
            speedometer.MaxSpeedometerSpeed = properties.MaximumSpeedometerSpeed;
            speedometer.PressureReference = properties.PressureReference;
            speedometer.MaxSafeSpeed = properties.MaximumSafeSpeed;
            speedometer.MaxCautionSpeed = properties.MaximumCautionSpeed;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            CR8ID.Default.Top = this.Top;
            CR8ID.Default.Left = this.Left;
            CR8ID.Default.Width = this.Width;
            CR8ID.Default.Height = this.Height;
            CR8ID.Default.Maximized = this.WindowState == WindowState.Maximized;
            CR8ID.Default.Save();
            if (CR8ID.Default.PersistState)
                dockingManager.SaveDockState(PERSIST_STATE_FILENAME);

            base.OnClosing(e);
        }

        private void MenuItemAdv_About(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void MenuItemAdv_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}