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
            // A license key from Syncfusion is required to compile and use
            // the Syncfusion components in the application. To keep the
            // license key secure we get the key from an environment
            // variable.
            var SFKEY = Environment.GetEnvironmentVariable("SFKEY");
            Syncfusion.Licensing.SyncfusionLicenseProvider
                .RegisterLicense(SFKEY);
        }
    }
}
