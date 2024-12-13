//-----------------------------------------------------------------------
// <copyright file="ProgramPropertiesViewModel.cs" company="Xcoder Software">
//     Author: Gil Yoder
//     Copyright (c) Xcoder Software. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using CommunityToolkit.Mvvm.Input;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace R8LocoCtrl.ViewModel
{
    public class ProgramPropertiesViewModel : ViewModelBase
    {

        private const string CONSIST_ED = "External Consist Editor.exe";
        private const string DISPATCHER = "Dispatcher.NET.exe";
        private const string EA_FILE = "ExternalAssistantForRun8.exe";
        private const string RUN8_FILE = "Run-8 Train Simulator V3.exe";

        RelayCommand<string>? _ExecuteAppCommand = null;
        private RelayCommand? _SubmitPropertiesCommand = null;
        private string? consistEdPath;
        private string? dispatcherPath;
        private int dynBrakeLC;
        private string? eAPath;
        private string? gradeMapPath;
        private int indyBrakeLC;
        private bool isConsistEdPathValid;
        private bool isDispatcherPathValid;
        private bool isEAPathValid;
        private bool isRun8PathValid;
        private int maximumCautionSpeed;
        private int maximumSafeSpeed;
        private int maximumSpeedometerSpeed;
        private int pressureReference;
        private RelayCommand? resetPropertiesCommand = null;
        private string? run8Path;
        private bool sSPointerVisible;
        private int trainBrakeLC;

        public ProgramPropertiesViewModel()
        {
            ResetProperties();
        }

        public event EventHandler<ProgramPropertiesViewModel>? SubmitProperties;

        public string? ConsistEdFilePath
        {
            get
            {
                if (IsConsistEdPathValid)
                    return Path.Combine(ConsistEdPath!, CONSIST_ED);
                return null;
            }
        }
        public string? ConsistEdPath
        {
            get => consistEdPath;
            set
            {
                if (consistEdPath == value)
                {
                    return;
                }

                consistEdPath = value;
                OnPropertyChanged();
                IsConsistEdPathValid = CheckExePath(ConsistEdPath, CONSIST_ED);
                OnPropertyChanged("ConsistEdFilePath");
            }
        }
        public string? DispatcherFilePath
        {
            get
            {
                if (IsDispatcherPathValid)
                    return Path.Combine(DispatcherPath!, DISPATCHER);
                return null;
            }
        }
        public string? DispatcherPath
        {
            get => dispatcherPath;
            set
            {
                if (dispatcherPath == value)
                {
                    return;
                }

                dispatcherPath = value;
                OnPropertyChanged();
                IsDispatcherPathValid = CheckExePath(DispatcherPath, DISPATCHER);
                OnPropertyChanged("DispatcherFilePath");
            }
        }
        public int DynBrakeLC
        {
            get => dynBrakeLC;
            set => SetProperty(ref dynBrakeLC, value);
        }
        public string? EAFilePath
        {
            get
            {
                if (IsEAPathValid)
                    return Path.Combine(EAPath!, EA_FILE);
                return null;
            }
        }
        public string? EAPath
        {
            get => eAPath;
            set
            {
                if (eAPath == value)
                {
                    return;
                }

                eAPath = value;
                OnPropertyChanged();
                IsEAPathValid = CheckExePath(EAPath, EA_FILE);
                OnPropertyChanged("EAFilePath");
            }
        }
        public RelayCommand<string> ExecuteAppCommand
        {
            get
            {
                _ExecuteAppCommand ??= new RelayCommand<string>(ExecuteAppExecute);
                return _ExecuteAppCommand;
            }
        }
        public string? GradeMapPath
        {
            get { return gradeMapPath; }
            set
            {
                if (gradeMapPath == value)
                {
                    return;
                }

                gradeMapPath = value;
                OnPropertyChanged();
            }
        }
        public int IndyBrakeLC
        {
            get => indyBrakeLC;
            set => SetProperty(ref indyBrakeLC, value);
        }
        public bool IsConsistEdPathValid
        {
            get => isConsistEdPathValid;
            set
            {
                if (isConsistEdPathValid == value)
                {
                    return;
                }

                isConsistEdPathValid = value;
                OnPropertyChanged();
            }
        }
        public bool IsDispatcherPathValid
        {
            get => isDispatcherPathValid;
            set
            {
                if (isDispatcherPathValid == value)
                {
                    return;
                }

                isDispatcherPathValid = value;
                OnPropertyChanged();
            }
        }
        public bool IsEAPathValid
        {
            get => isEAPathValid;
            set
            {
                if (isEAPathValid == value)
                {
                    return;
                }

                isEAPathValid = value;
                OnPropertyChanged();
            }
        }
        public bool IsRun8PathValid
        {
            get => isRun8PathValid;
            set
            {
                if (isRun8PathValid == value)
                {
                    return;
                }

                isRun8PathValid = value;
                OnPropertyChanged();
            }
        }
        public int MaximumCautionSpeed
        {
            get { return maximumCautionSpeed; }
            set
            {
                if (maximumCautionSpeed == value)
                {
                    return;
                }

                maximumCautionSpeed = value;
                OnPropertyChanged();
            }
        }
        public int MaximumSafeSpeed
        {
            get { return maximumSafeSpeed; }
            set
            {
                if (maximumSafeSpeed == value)
                {
                    return;
                }

                maximumSafeSpeed = value;
                OnPropertyChanged();
            }
        }
        public int MaximumSpeedometerSpeed
        {
            get { return maximumSpeedometerSpeed; }
            set
            {
                if (maximumSpeedometerSpeed == value)
                {
                    return;
                }

                maximumSpeedometerSpeed = value;
                OnPropertyChanged();
            }
        }
        public int PressureReference
        {
            get => pressureReference;
            set
            {
                if (pressureReference == value)
                {
                    return;
                }

                pressureReference = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand ResetPropertiesCommand
        {
            get
            {
                resetPropertiesCommand ??= new RelayCommand(ResetPropertiesExecute);
                return resetPropertiesCommand;
            }
        }
        public string? Run8FilePath
        {
            get
            {
                if (IsRun8PathValid)
                    return Path.Combine(Run8Path!, RUN8_FILE);
                return null;
            }
        }
        public string? Run8Path
        {
            get { return run8Path; }
            set
            {
                if (run8Path == value)
                {
                    return;
                }

                run8Path = value;
                OnPropertyChanged();
                IsRun8PathValid = CheckExePath(Run8Path, RUN8_FILE);
                OnPropertyChanged("Run8FilePath");
                if (IsRun8PathValid)
                {
                    GradeMapPath = Path.Combine(run8Path!, "maps");
                    return;
                }

                GradeMapPath = null;
            }
        }
        public bool SSPointerVisible
        {
            get { return sSPointerVisible; }
            set
            {
                if (sSPointerVisible == value)
                {
                    return;
                }

                sSPointerVisible = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand SubmitPropertiesCommand
        {
            get
            {
                _SubmitPropertiesCommand ??= new RelayCommand(SubmitPropertiesExecute);
                return _SubmitPropertiesCommand;
            }
        }
        public int TrainBrakeLC
        {
            get => trainBrakeLC;
            set => SetProperty(ref trainBrakeLC, value);
        }

        private bool CheckExePath(string? path, string exeName)
        {
            bool isValid = false;
            try
            {
                if (path == null) return false;
                if (!Directory.Exists(path)) return false;
                var files = Directory.EnumerateFiles(path, "*.exe");
                if (!files.Any()) return false;
                isValid = files.Any(m => m.Contains(exeName, StringComparison.CurrentCultureIgnoreCase));
            }
            catch (Exception)
            {
            }

            return isValid;
        }
        private void ResetProperties()
        {
            Run8Path = CR8ID.Default.Run8Path;
            MaximumCautionSpeed = CR8ID.Default.MaxCautionSpeed;
            MaximumSafeSpeed = CR8ID.Default.MaxSafeSpeed;
            MaximumSpeedometerSpeed = CR8ID.Default.MaxSpeedOSpeed;
            PressureReference = CR8ID.Default.PressureReference;
            SSPointerVisible = CR8ID.Default.SSPointerVisible;

            TrainBrakeLC = CR8ID.Default.TrainBrakeLC;
            DynBrakeLC = CR8ID.Default.DynBrakeLC;
            IndyBrakeLC = CR8ID.Default.IndyBrakeLC;
        }

        protected virtual void ExecuteAppExecute(string? filepath)
        {
            if (filepath == null)
                return;

            try
            {
                var pi = new ProcessStartInfo(filepath)
                {
                    UseShellExecute = false,
                    WorkingDirectory = Path.GetDirectoryName(filepath)
                };

                Process.Start(pi);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Exception");
            }
        }
        protected virtual void ResetPropertiesExecute()
        {
            ResetProperties();
        }
        protected virtual void SubmitPropertiesExecute()
        {
            CR8ID.Default.Run8Path = Run8Path;
            CR8ID.Default.MaxCautionSpeed = MaximumCautionSpeed;
            CR8ID.Default.MaxSafeSpeed = MaximumSafeSpeed;
            CR8ID.Default.MaxSpeedOSpeed = MaximumSpeedometerSpeed;
            CR8ID.Default.PressureReference = PressureReference;
            CR8ID.Default.EAPath = EAPath;
            CR8ID.Default.DispatcherPath = DispatcherPath;
            CR8ID.Default.ConsistEdPath = ConsistEdPath;
            CR8ID.Default.SSPointerVisible = SSPointerVisible;

            CR8ID.Default.TrainBrakeLC = TrainBrakeLC;
            CR8ID.Default.DynBrakeLC = DynBrakeLC;
            CR8ID.Default.IndyBrakeLC = IndyBrakeLC;

            CR8ID.Default.Save();

            SubmitProperties?.Invoke(this, this);
        }

    }
}
