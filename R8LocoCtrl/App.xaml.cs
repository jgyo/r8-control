//-----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Xcoder Software">
//     Author: Gil Yoder
//     Copyright (c) Xcoder Software. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Windows;

namespace R8LocoCtrl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var SFKEY = "License Key Here";
            Syncfusion.Licensing.SyncfusionLicenseProvider
                .RegisterLicense(SFKEY);
        }
    }
}
