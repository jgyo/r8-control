//-----------------------------------------------------------------------
// <copyright file="SpeedometerSettingsViewModel.cs" company="Xcoder Software">
//     Author: Gil Yoder
//     Copyright (c) Xcoder Software. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Linq;

namespace R8LocoCtrl.ViewModel
{
    public class SpeedometerSettingsViewModel : ViewModelBase
    {

        private int maxCautionSpeed;
        private int maxSafeSpeed;
        private int maxSpeedometerSpeed;
        private int pressureReference;

        public int MaxCautionSpeed
        {
            get => maxCautionSpeed; set
            {
                if (maxCautionSpeed == value)
                {
                    return;
                }

                maxCautionSpeed = value;
                OnPropertyChanged();
            }
        }
        public int MaxSafeSpeed
        {
            get => maxSafeSpeed; set
            {
                if (maxSafeSpeed == value)
                {
                    return;
                }

                maxSafeSpeed = value;
                OnPropertyChanged();
            }
        }
        public int MaxSpeedometerSpeed
        {
            get => maxSpeedometerSpeed; set
            {
                if (maxSpeedometerSpeed == value)
                {
                    return;
                }

                maxSpeedometerSpeed = value;
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

        private bool sSPointerVisible;
        public bool SSPointerVisible
        {
            get => sSPointerVisible;
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
    }
}
