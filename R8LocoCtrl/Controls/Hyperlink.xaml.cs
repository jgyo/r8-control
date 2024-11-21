//-----------------------------------------------------------------------
// <copyright file="Hyperlink.xaml.cs" company="Xcoder Software">
//     Author: Gil Yoder
//     Copyright (c) Xcoder Software. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace R8LocoCtrl.Controls
{
    /// <summary>
    /// Interaction logic for Hyperlink.xaml
    /// </summary>
    public partial class Hyperlink : UserControl
    {

        public static readonly DependencyProperty LinkProperty =
    DependencyProperty.Register("Link", typeof(string), typeof(Hyperlink), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty TextProperty =
    DependencyProperty.Register("Text", typeof(string), typeof(Hyperlink), new PropertyMetadata(string.Empty));

        public Hyperlink()
        {
            InitializeComponent();
        }

        public string Link
        {
            get { return (string)GetValue(LinkProperty); }
            set { SetValue(LinkProperty, value); }
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(Link == null) return;
            Process.Start(new ProcessStartInfo(Link) { UseShellExecute = true });
        }
    }
}
