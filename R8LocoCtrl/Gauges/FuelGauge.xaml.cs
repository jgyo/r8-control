//-----------------------------------------------------------------------
// <copyright file="FuelGauge.xaml.cs" company="Xcoder Software">
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
    /// Interaction logic for FuelGauge.xaml
    /// </summary>
    public partial class FuelGauge : UserControl
    {

        public static readonly DependencyProperty LevelProperty =
    DependencyProperty.Register("Level", typeof(int), typeof(FuelGauge), new PropertyMetadata(0));

        public FuelGauge()
        {
            InitializeComponent();
        }

        public int Level
        {
            get { return (int)GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "Level")
            {
                var width = this.Reference.ActualWidth;
                width = width * Level / 255;
                this.GaugeBar.Width = width;
            }

            base.OnPropertyChanged(e);
        }

    }
}
