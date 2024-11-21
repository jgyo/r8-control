//-----------------------------------------------------------------------
// <copyright file="ProgramProperties.xaml.cs" company="Xcoder Software">
//     Author: Gil Yoder
//     Copyright (c) Xcoder Software. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using R8LocoCtrl.ViewModel;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace R8LocoCtrl.Controls
{
    /// <summary>
    /// Interaction logic for ProgramProperties.xaml
    /// </summary>
    public partial class ProgramProperties : UserControl
    {
        private ProgramPropertiesViewModel? properties;

        public ProgramProperties()
        {
            InitializeComponent();
            this.DataContextChanged += ProgramProperties_DataContextChanged;
            properties = (ProgramPropertiesViewModel)DataContext;
            SetupProperties();
        }

        private void ProgramProperties_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            properties = (ProgramPropertiesViewModel)e.NewValue;
            SetupProperties();
        }

        private void SetupProperties()
        {
            if (properties == null) { return; }

            if (this.FindResource("speedoViewModel") is not SpeedometerSettingsViewModel speedo)
            {
                throw new ApplicationException("Run8ListenerClient could not be found.");
            }

            speedo.MaxCautionSpeed = properties.MaximumCautionSpeed;
            speedo.MaxSafeSpeed = properties.MaximumSafeSpeed;
            speedo.MaxSpeedometerSpeed = properties.MaximumSpeedometerSpeed;
            speedo.PressureReference = properties.PressureReference;
        }
    }
}
