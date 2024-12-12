//-----------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Xcoder Software">
//     Author: Gil Yoder
//     Copyright (c) Xcoder Software. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using Octokit;
using R8LocoCtrl.Interface;
using R8LocoCtrl.ViewModel;
using Syncfusion.Windows.PdfViewer;
using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools.Controls;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;

namespace R8LocoCtrl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const double DEFAULT_HEIGHT = 540;
        const string DEFAULT_STATE_FILENAME = "DefaultState.xml";
        const double DEFAULT_WIDTH = 1230;
        const string PERSIST_STATE_FILENAME = "ClosingState.xml";
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

        [GeneratedRegex(@"^.*\\(?:(RUN 8|RUN8|))(?<Name>.*)(?:Grade.Map\.pdf)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.ExplicitCapture | RegexOptions.Singleline, "en-US")]
        private static partial Regex GradeMapRegEx();

        private static void SetProgramProperties(ProgramPropertiesViewModel properties)
        {
            properties.PressureReference = CR8ID.Default.PressureReference;
            properties.Run8Path = CR8ID.Default.Run8Path;
            properties.MaximumCautionSpeed = CR8ID.Default.MaxCautionSpeed;
            properties.MaximumSafeSpeed = CR8ID.Default.MaxSafeSpeed;
            properties.MaximumSpeedometerSpeed = CR8ID.Default.MaxSpeedOSpeed;
            properties.DispatcherPath = CR8ID.Default.DispatcherPath;
            properties.EAPath = CR8ID.Default.EAPath;
            properties.ConsistEdPath = CR8ID.Default.ConsistEdPath;
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
            if (!progProperties.IsRun8PathValid)
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

            var pdfViewer = new PdfViewerControl() { Name = filename.Replace('.', '_').Replace('-','_'), ItemSource = docStream };
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

        private void dockingManager_ChildrenCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
                return;

            if (e.OldItems == null) return;
            foreach (var child in e.OldItems)
            {
                var pdfViewer = child as PdfViewerControl;
                if (pdfViewer == null) continue;
                var file = pdfViewer.ItemSource as FileStream;
                file?.Close();
                file?.Dispose();
            }
        }

        private void DockingManager_CloseButtonClick(object sender, CloseButtonEventArgs e)
        {
            var content = (e.TargetItem as PdfViewerControl);
            if (content != null)
            {
                dockingManager.Children.Remove(content);
            }
        }

        private async void DockingManager_DockStateChanged(FrameworkElement sender, DockStateEventArgs e)
        {
            // Setting TopMost on the main window will put the main
            // window and all detached dockable windows on top of
            // all other displayed windows. However, if a window is
            // undocked and floated on the screen, it may be covered
            // by other windows from other programs.
            //
            // Resetting TopMost twice after a tear will put all
            // application windows back on top, but how to do that
            // programmatically? This even is the trick, but it fires
            // just a little too soon for that to work. Taking a
            // brief break, about 1/10 second, allows the tear to
            // complete and allows the trick to work.

            await Task.Delay(100);

            AlwaysOnTop.IsChecked = !AlwaysOnTop.IsChecked;
            AlwaysOnTop.IsChecked = !AlwaysOnTop.IsChecked;
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
        private async Task<Version> GetRepoVersion()
        {
            // Get releases from GitHub
            // Source: https://octokitnet.readthedocs.io/en/latest/getting-started/
            var client = new GitHubClient(new ProductHeaderValue("R8Control-app"));
            var releases = await client.Repository.Release.GetAll("jgyo", "R8-Control");

            return new Version(releases[0].TagName.Trim('v'));
        }

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

        private void MenuItemAdv_About(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void MenuItemAdv_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void MenuItemAdv_VersionCheck(object sender, RoutedEventArgs e)
        {
            var latestVersion = await GetRepoVersion();
            var localVersion = Assembly.GetExecutingAssembly().GetName().Version;

            var versionComparison = localVersion.CompareTo(latestVersion);
            if (versionComparison < 0)
            {
                var result = MessageBox.Show(
                    "There is a new version of R8 Control available.\r\n" +
                        $"Current version: {localVersion.ToString().Trim('0').Trim('.')}" +
                        "\r\n" +
                        $"Latest version: {latestVersion}" +
                        "\r\n" +
                        "Would you like to download the latest version now?",
                    "Version Information",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information);
                if(result == MessageBoxResult.Yes)
                {
                    try
                    {
                        Process.Start(
                            new ProcessStartInfo(
                                $"https://github.com/jgyo/r8-control/releases/download/v1.1.1/R8.Control.v{latestVersion}.exe")
                            {
                                UseShellExecute = true
                            });

                        MessageBox.Show(
                            $"Please check your browser for R8.Control.v{latestVersion}.exe and execute that program to install the latest version.",
                            "Version Information",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    catch
                    {
                        MessageBox.Show(
                            "I am unable to download the latest version. You may need to set up a default browser.",
                            "Downloading Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }

                }
            }
            else if (versionComparison > 0)
            {
                MessageBox.Show(
                    $"Your current version ({localVersion.ToString().Trim('0').Trim('.')}) is later than the latest version on GitHub ({latestVersion}). Well done! ☺",
                    "Version Information",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(
                    $"You already have the latest version ({latestVersion}). No upgrade is needed.",
                    "Version Information",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void ProgProperties_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsRun8PathValid")
            {
                ConfigureGradeMapMenu();
            }
        }

        private void ProgProperties_SubmitProperties(object? sender, ProgramPropertiesViewModel e)
        {
            SetSpeedometerProperties(e);
            DockingManager.SetState(this.Setup, DockState.Hidden);
        }
        private void SetSpeedometerProperties(ProgramPropertiesViewModel properties)
        {
            var speedometer = (SpeedometerSettingsViewModel)this.FindResource("speedoViewModel");
            speedometer.MaxSpeedometerSpeed = properties.MaximumSpeedometerSpeed;
            speedometer.PressureReference = properties.PressureReference;
            speedometer.MaxSafeSpeed = properties.MaximumSafeSpeed;
            speedometer.MaxCautionSpeed = properties.MaximumCautionSpeed;
            speedometer.SSPointerVisible = properties.SSPointerVisible;
        }
    }
}