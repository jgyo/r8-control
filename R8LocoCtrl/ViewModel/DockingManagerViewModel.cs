//-----------------------------------------------------------------------
// <copyright file="DockingManagerViewModel.cs" company="Xcoder Software">
//     Author: Gil Yoder
//     Copyright (c) Xcoder Software. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using CommunityToolkit.Mvvm.Input;
using R8LocoCtrl.Interface;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace R8LocoCtrl.ViewModel
{
    public class DockingManagerViewModel : ViewModelBase
    {

        public event EventHandler<Run8Windows>? ActivateWindow;
        public event EventHandler? DefaultState;
        public event EventHandler<string[]>? LoadMap;
        public event EventHandler<string>? OpenGradeMapWindow;
        public event EventHandler<bool>? PersistState;

        #region OpenInBrowserCommand
        RelayCommand<string?>? _OpenInBrowserCommand = null;
        public RelayCommand<string?> OpenInBrowserCommand
        {
            get
            {
                _OpenInBrowserCommand ??= new RelayCommand<string?>(OpenInBrowserExecute);
                return _OpenInBrowserCommand;
            }
        }

        protected virtual void OpenInBrowserExecute(string? link)
        {
            if (string.IsNullOrEmpty(link)) throw new ArgumentNullException("link");
            try
            {
                _ = Process.Start(new ProcessStartInfo(link) { UseShellExecute = true });
            } catch
            {
                MessageBox.Show(
                    "Unable to open link. You may need to set up a default browser.",
                    "Opening Link Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        #endregion OpenInBrowserCommand

        #region RefreshMapsCommand
        private RelayCommand? _RefreshMapsCommand = null;
        public RelayCommand RefreshMapsCommand
        {
            get
            {
                _RefreshMapsCommand ??= new RelayCommand(RefreshMapsExecute);
                return _RefreshMapsCommand;
            }
        }

        private bool isPersistStateSet;
        public bool IsPersistStateSet
        {
            get => isPersistStateSet;
            set
            {
                if (isPersistStateSet == value)
                {
                    return;
                }

                isPersistStateSet = value;
                OnPropertyChanged();
                CR8ID.Default.PersistState = isPersistStateSet;
            }
        }

        protected virtual void RefreshMapsExecute()
        {
            var assemblyPath = Assembly.GetAssembly(this.GetType())?.Location;
            assemblyPath = Path.GetDirectoryName(assemblyPath);
            assemblyPath = Path.Combine(assemblyPath!, "downloads");
            if (!Directory.Exists(assemblyPath))
            {
                return;
            }

            foreach (var file in new DirectoryInfo(assemblyPath).EnumerateFiles("*.pdf"))
            {
                try
                {
                    file.Delete();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, $"Error deleting {file.Name}");
                }
            }
        }
        #endregion RefreshMapsCommand

        #region LoadMapCommand
        private RelayCommand<string>? _LoadMapCommand = null;
        public RelayCommand<string> LoadMapCommand
        {
            get
            {
                _LoadMapCommand ??= new RelayCommand<string>(LoadMapExecute!);
                return _LoadMapCommand;
            }
        }

        protected virtual void LoadMapExecute(string parameter)
        {
            var @params = parameter.Split('/');
            var filename = $"{@params[1]}.pdf";
            var webaddress = $"https://www.thedepotserver.com/maps/{@params[0]}/{filename}";
            var assemblyPath = Assembly.GetAssembly(this.GetType())?.Location;
            assemblyPath = Path.GetDirectoryName(assemblyPath);
            assemblyPath = Path.Combine(assemblyPath!, "downloads");
            if (!Directory.Exists(assemblyPath))
            {
                Directory.CreateDirectory(assemblyPath);
            }
            var filepath = Path.Combine(assemblyPath!, filename);
            string[] data = [ filename, webaddress, filepath ];

            LoadMap?.Invoke(this, data);
        }
        #endregion LoadMapCommand


        #region PersistStateCommand        
        private RelayCommand<bool>? _PersistStateCommand = null;
        public RelayCommand<bool> PersistStateCommand
        {
            get
            {
                _PersistStateCommand ??= new RelayCommand<bool>(PersistStateExecute);
                return _PersistStateCommand;
            }
        }

        protected virtual void PersistStateExecute(bool onOrOff)
        {
            PersistState?.Invoke(this, onOrOff);
        }
        #endregion PersistStateCommand

        #region ResetStateCommand        
        private RelayCommand? _DefaultStateCommand = null;
        public RelayCommand DefaultStateCommand
        {
            get
            {
                _DefaultStateCommand ??= new RelayCommand(DefaultStateExecute);
                return _DefaultStateCommand;
            }
        }

        protected virtual void DefaultStateExecute()
        {
            DefaultState?.Invoke(this, EventArgs.Empty);
        }
        #endregion ResetStateCommand

        #region OpenGradeMapCommand
        private RelayCommand<string>? _OpenGradeMapCommand = null;
        public RelayCommand<string> OpenGradeMapCommand
        {
            get
            {
                _OpenGradeMapCommand ??= new RelayCommand<string>(OpenGradeMapExecute!);
                return _OpenGradeMapCommand;
            }
        }

        protected virtual void OpenGradeMapExecute(string parameter)
        {
            OpenGradeMapWindow?.Invoke(this, parameter);
        }
        #endregion OpenGradeMapCommand

        #region ActivateWindowCommand
        private RelayCommand<Run8Windows>? _ActivateWindowCommand = null;
        public RelayCommand<Run8Windows> ActivateWindowCommand
        {
            get
            {
                _ActivateWindowCommand ??= new RelayCommand<Run8Windows>(ActivateWindowExecute);
                return _ActivateWindowCommand;
            }
        }

        protected virtual void ActivateWindowExecute(Run8Windows parameter)
        {
            ActivateWindow?.Invoke(this, parameter);
        }
        #endregion ActivateWindowCommand


    }
}
