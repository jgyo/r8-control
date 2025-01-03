using R8LocoCtrl.ViewModel;
using System;
using System.Linq;
using System.Windows;

namespace R8LocoCtrl
{
    /// <summary>
    /// Interaction logic for SetupWindow.xaml
    /// </summary>
    public partial class SetupWindow : Window
    {
        private ProgramPropertiesViewModel progProperties;

        public SetupWindow()
        {
            InitializeComponent();

            progProperties = (ProgramPropertiesViewModel)this.FindResource("programProperties");
            DataContext = progProperties;
            progProperties.SubmitProperties += ProgProperties_SubmitProperties;
        }

        private void ProgProperties_SubmitProperties(object? sender, ProgramPropertiesViewModel e)
        {
            this.Close();
        }
    }
}
