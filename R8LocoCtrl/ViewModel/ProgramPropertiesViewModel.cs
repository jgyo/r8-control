//-----------------------------------------------------------------------
// <copyright file="ProgramPropertiesViewModel.cs" company="Xcoder Software">
//     Author: Gil Yoder
//     Copyright (c) Xcoder Software. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using CommunityToolkit.Mvvm.Input;
using System;
using System.IO;
using System.Linq;

namespace R8LocoCtrl.ViewModel
{
    public class ProgramPropertiesViewModel : ViewModelBase
    {

        private string? gradeMapPath;
        private bool isGradeMapPathValid;
        private int maximumCautionSpeed;
        private int maximumSafeSpeed;
        private int maximumSpeedometerSpeed;
        private int pressureReference;

        public ProgramPropertiesViewModel()
        {
            ResetProperties();
        }

        public event EventHandler<ProgramPropertiesViewModel>? SubmitProperties;

        private void CheckGradeMapPath()
        {
            bool isValid = false;
            try
            {
                if(GradeMapPath == null) return;
                if(!Directory.Exists(GradeMapPath)) return;
                var files = Directory.EnumerateFiles(GradeMapPath, "run8*.pdf");
                if(!files.Any()) return;
                isValid = files.Any(m => m.Contains("map.pdf", StringComparison.CurrentCultureIgnoreCase));
            }
            catch (Exception)
            {
            }
            finally
            {
                IsGradeMapPathValid = isValid;
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
                CheckGradeMapPath();
            }
        }

        public bool IsGradeMapPathValid
        {
            get => isGradeMapPathValid;
            set
            {
                if (isGradeMapPathValid == value)
                {
                    return;
                }

                isGradeMapPathValid = value;
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

        private void ResetProperties()
        {
            GradeMapPath = CR8ID.Default.GradeMapPath;
            MaximumCautionSpeed = CR8ID.Default.MaxCautionSpeed;
            MaximumSafeSpeed = CR8ID.Default.MaxSafeSpeed;
            MaximumSpeedometerSpeed = CR8ID.Default.MaxSpeedOSpeed;
            PressureReference = CR8ID.Default.PressureReference;
        }

        #region SubmitPropertiesCommand        
        private RelayCommand? _SubmitPropertiesCommand = null;
        public RelayCommand SubmitPropertiesCommand
        {
            get
            {
                _SubmitPropertiesCommand ??= new RelayCommand(SubmitPropertiesExecute);
                return _SubmitPropertiesCommand;
            }
        }

        protected virtual void SubmitPropertiesExecute()
        {
            CR8ID.Default.GradeMapPath = GradeMapPath;
            CR8ID.Default.MaxCautionSpeed = MaximumCautionSpeed;
            CR8ID.Default.MaxSafeSpeed = MaximumSafeSpeed;
            CR8ID.Default.MaxSpeedOSpeed = MaximumSpeedometerSpeed;
            CR8ID.Default.PressureReference = PressureReference;

            CR8ID.Default.Save();

            SubmitProperties?.Invoke(this, this);

        }
        #endregion SubmitPropertiesCommand


        #region ResetPropertiesCommand        
        private RelayCommand? resetPropertiesCommand = null;
        public RelayCommand ResetPropertiesCommand
        {
            get
            {
                resetPropertiesCommand ??= new RelayCommand(ResetPropertiesExecute);
                return resetPropertiesCommand;
            }
        }

        protected virtual void ResetPropertiesExecute()
        {
            ResetProperties();
        }
        #endregion ResetPropertiesCommand


    }
}
