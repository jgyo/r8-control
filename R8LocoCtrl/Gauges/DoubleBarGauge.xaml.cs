//-----------------------------------------------------------------------
// <copyright file="DoubleBarGauge.cs" company="Xcoder Software">
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
    /// Interaction logic for DoubleBarGauge.xaml
    /// </summary>
    public partial class DoubleBarGauge : UserControl
    {

        public static readonly DependencyProperty BottomBarValueProperty =
    DependencyProperty.Register("BottomBarValue", typeof(int), typeof(DoubleBarGauge), new PropertyMetadata(80));
        public static readonly DependencyProperty PipePressureReferenceProperty =
    DependencyProperty.Register("PipePressureReference", typeof(int), typeof(DoubleBarGauge), new PropertyMetadata(90));
        public static readonly DependencyProperty TopBarValueProperty =
    DependencyProperty.Register("TopBarValue", typeof(int), typeof(DoubleBarGauge), new PropertyMetadata(80));

        public DoubleBarGauge()
        {
            InitializeComponent();
        }

        public int BottomBarValue
        {
            get { return (int)GetValue(BottomBarValueProperty); }
            set { SetValue(BottomBarValueProperty, value); }
        }
        public int PipePressureReference
        {
            get { return (int)GetValue(PipePressureReferenceProperty); }
            set { SetValue(PipePressureReferenceProperty, value); }
        }
        public int TopBarValue
        {
            get { return (int)GetValue(TopBarValueProperty); }
            set { SetValue(TopBarValueProperty, value); }
        }

        private void MainGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged == false) { return; }
            SetBars(e.NewSize.Width);
        }
        private void SetBars()
        {
            SetBars(this.MainGrid.ActualWidth);
        }
        private void SetBars(double newWidth)
        {
            var factor = newWidth - newWidth / 9d;
            this.TopGaugeBar.Width = SetBars(TopBarValue, factor);
            this.BottomGaugeBar.Width = SetBars(BottomBarValue, factor);
            SetPressureReference();
        }
        private double SetBars(int barValue, double factor)
        {

            if (barValue < 40)
            {
                return 0.0;
            }

            barValue -= 40;
            return factor * barValue / 80d;
        }
        private void SetPressureReference()
        {
            var factor = this.MainGrid.ActualWidth - this.MainGrid.ActualWidth / 9d;
            var margin = (this.PipePressureReference - 40) * factor / 80d;
            if (margin < 0)
            {
                margin = 0;
            }
            var extra = this.MainGrid.ActualWidth / 18d;
            this.BreakPipeReferenceMark.Margin = new Thickness(margin + extra - 6, 0d, 0d, 0d);
            this.EqReferenceMark.Margin = new Thickness(margin + extra - 6, 0d, 0d, 0d);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            switch (e.Property.Name)
            {
                case "BottomBarValue":
                    SetBars();
                    break;
                case "TopBarValue":
                    SetBars();
                    break;
                case "PipePressureReference":
                    SetPressureReference();
                    break;
            }
            base.OnPropertyChanged(e);
        }

    }
}
