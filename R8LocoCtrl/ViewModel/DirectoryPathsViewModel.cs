//-----------------------------------------------------------------------
// <copyright file="DirectoryPathsViewModel.cs" company="Xcoder Software">
//     Author: Gil Yoder
//     Copyright (c) Xcoder Software. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace R8LocoCtrl.ViewModel
{
    public class DirectoryPathsViewModel : ViewModelBase
    {

        private string? rootPath;
        private string? workingPath;

        private string? RootPath
        {
            get => rootPath; set
            {
                if (rootPath == value)
                {
                    return;
                }

                rootPath = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> DirectoryPaths { get; private set; } = [];
        public string? WorkingPath
        {
            get => workingPath;
            set
            {
                if (workingPath == value)
                {
                    return;
                }

                workingPath = value;
                OnPropertyChanged();
            }
        }

        private void UpdatePaths()
        {
            // RootPath should always point to an existing directory
            if (RootPath == null) return;

            DirectoryPaths.Clear();
            DirectoryPaths.Add(RootPath);
            foreach (string path in
            Directory.GetDirectories(RootPath))
            {
                DirectoryPaths.Add(path);
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyName == "RootPath")
            {
                UpdatePaths();
            }
            else if (workingPath != null && propertyName == nameof(WorkingPath) && Path.GetFullPath(workingPath).Equals(workingPath, StringComparison.CurrentCultureIgnoreCase))
            {
                if (Directory.Exists(WorkingPath))
                {
                    RootPath = WorkingPath;
                }
            }
            base.OnPropertyChanged(propertyName);
        }

    }
}
