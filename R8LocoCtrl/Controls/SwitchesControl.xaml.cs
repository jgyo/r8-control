using HotKeyLibrary;
using R8LocoCtrl.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace R8LocoCtrl.Controls
{
    /// <summary>
    /// Interaction logic for SwitchesControl.xaml
    /// </summary>
    public partial class SwitchesControl : UserControl
    {
        public SwitchesControl()
        {
            InitializeComponent();
            CommandRegistry.Instance.SubscribeToCommand(Commands.SlowSpeedToggle, ToggleSlowSpeedControl);
        }

        private void ToggleSlowSpeedControl()
        {
            SlowSpeedOnOffButton.IsChecked = !SlowSpeedOnOffButton.IsChecked;
        }
    }
}
