//-----------------------------------------------------------------------
// <copyright file="EotGauge.xaml.cs" company="Xcoder Software">
//     Author: Gil Yoder
//     Copyright (c) Xcoder Software. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace R8LocoCtrl.Gauges
{
    /// <summary>
    /// Interaction logic for EotGauge.xaml
    /// </summary>
    public partial class EotGauge : UserControl
    {

        public static readonly DependencyProperty ErrorProperty =
    DependencyProperty.Register("Error", typeof(bool), typeof(EotGauge), new PropertyMetadata(false));
        public static readonly DependencyProperty IdProperty =
    DependencyProperty.Register("Id", typeof(int), typeof(EotGauge), new PropertyMetadata(0));
        public static readonly DependencyProperty IsBeaconOnProperty =
    DependencyProperty.Register("IsBeaconOn", typeof(bool), typeof(EotGauge), new PropertyMetadata(false));
        public static readonly DependencyProperty IsCommTestingProperty =
    DependencyProperty.Register("IsCommTesting", typeof(bool), typeof(EotGauge), new PropertyMetadata(false));
        public static readonly DependencyProperty IsMovingProperty =
    DependencyProperty.Register("IsMoving", typeof(bool), typeof(EotGauge), new PropertyMetadata(false));

        public EotGauge()
        {
            InitializeComponent();
        }

        public bool Error
        {
            get { return (bool)GetValue(ErrorProperty); }
            set { SetValue(ErrorProperty, value); }
        }
        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        public bool IsBeaconOn
        {
            get { return (bool)GetValue(IsBeaconOnProperty); }
            set { SetValue(IsBeaconOnProperty, value); }
        }
        public bool IsCommTesting
        {
            get { return (bool)GetValue(IsCommTestingProperty); }
            set { SetValue(IsCommTestingProperty, value); }
        }
        public bool IsMoving
        {
            get { return (bool)GetValue(IsMovingProperty); }
            set { SetValue(IsMovingProperty, value); }
        }

    }
}
