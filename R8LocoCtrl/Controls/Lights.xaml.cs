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
    /// Interaction logic for Lights.xaml
    /// </summary>
    public partial class Lights : UserControl
    {
        public Lights()
        {
            InitializeComponent();
            CommandRegistry.Instance.SubscribeToCommand(Commands.HeadlightFrontSwitchDown, FrontHLSwitchDown);
            CommandRegistry.Instance.SubscribeToCommand(Commands.HeadlightFrontSwitchUp, FrontHLSwitchUp);
            CommandRegistry.Instance.SubscribeToCommand(Commands.HeadlightRearSwitchDown, RearHLSwitchDown);
            CommandRegistry.Instance.SubscribeToCommand(Commands.HeadlightRearSwitchUp, RearHLSwitchUp);
            CommandRegistry.Instance.SubscribeToCommand(Commands.CabLightSwitch, CabLightSwitch);
            CommandRegistry.Instance.SubscribeToCommand(Commands.StepLightSwitch, StepLightSwitch);
            CommandRegistry.Instance.SubscribeToCommand(Commands.GaugeLightSwitch, GaugeLightSwitch);
        }

        private void CabLightSwitch()
        {
            CabLightButton.IsChecked = !CabLightButton.IsChecked;
        }

        private void FrontHLSwitchDown()
        {
            if(FrontHLFull.IsChecked == true)
            {
                FrontHLDim.IsChecked = true;
                return;
            }
            if(FrontHLDim.IsChecked == true)
            {
                FrontHLOff.IsChecked = true;
            }
        }

        private void RearHLSwitchUp()
        {
            if (RearHLDim.IsChecked == true)
            {
                RearHLFull.IsChecked = true;
                return;
            }

            if (RearHLOff.IsChecked == true)
            {
                RearHLDim.IsChecked = true;
            }
        }

        private void FrontHLSwitchUp()
        {
            if (FrontHLDim.IsChecked == true)
            {
                FrontHLFull.IsChecked = true;
                return;
            }

            if (FrontHLOff.IsChecked == true)
            {
                FrontHLDim.IsChecked = true;
            }
        }

        private void RearHLSwitchDown()
        {
            if(RearHLFull.IsChecked == true)
            {
                RearHLDim.IsChecked= true;
                return;
            }
            if(RearHLDim.IsChecked == true)
            {
                RearHLOff.IsChecked = true;
            }            
        }
        private void GaugeLightSwitch()
        {
            GaugeLightButton.IsChecked = !GaugeLightButton.IsChecked;
        }
        private void StepLightSwitch()
        {
            StepLightButton.IsChecked= !StepLightButton.IsChecked;
        }
    }
}
