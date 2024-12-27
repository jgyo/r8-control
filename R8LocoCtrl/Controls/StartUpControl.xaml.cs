using HotKeyLibrary;
using R8LocoCtrl.Interface;
using R8LocoCtrl.ViewModel;
using System;
using System.Linq;
using System.Windows.Controls;

namespace R8LocoCtrl.Controls
{
    /// <summary>
    /// Interaction logic for StartUpControl.xaml
    /// </summary>
    public partial class StartUpControl : UserControl
    {
        public StartUpControl()
        {
            InitializeComponent();
            CommandRegistry.Instance.SubscribeToCommand(Commands.IsolationSwitch, IsoSwitch);
            CommandRegistry.Instance.SubscribeToCommand(Commands.TrainBrakeCutout, TBCutOutSwitch);
            CommandRegistry.Instance.SubscribeToCommand(Commands.ServiceSelector, SsSwitch);
            CommandRegistry.Instance.SubscribeToCommand(Commands.MuHLSwitch, MuHLSwitch);
            CommandRegistry.Instance.SubscribeToCommand(Commands.EngineStartStop, EngineSwitch);
        }

        private void EngineSwitch()
        {
            EngineButton.IsChecked = !EngineButton.IsChecked;

            var actions = DataContext as Run8ActionsViewModel;
            actions!.EngineStartCommand?.Execute([false, (bool)EngineButton.IsChecked!]);
        }

        private void IsoSwitch()
        {
            if(IsoStart.IsChecked == true)
            {
                IsoIsolate.IsChecked = true;
            }
            else if(IsoIsolate.IsChecked == true)
            {
                IsoRun.IsChecked = true;
            }
            else
            {
                IsoStart.IsChecked = true;
            }
        }

        private void MuHLSwitch()
        {
            if(MUHLSM.IsChecked == true)
            {
                MUHLSHL.IsChecked = true;
            }
            else if(MUHLSHL.IsChecked == true)
            {
                MUHLSHT.IsChecked = true;
            }
            else if(MUHLSHT.IsChecked == true)
            {
                MUHLLHL.IsChecked = true;
            }
            else if(MUHLLHL.IsChecked == true)
            {
                MUHLLHT.IsChecked = true;
            }
            else
            {
                MUHLSHL.IsChecked = true;
            }
        }

        private void SsSwitch()
        {
            if(SsSw1.IsChecked == true)
            {
                SsSw2.IsChecked = true;
            }
            else if(SsSw2.IsChecked == true)
            {
                SsFS.IsChecked = true;
            }
            else if(SsFS.IsChecked == true)
            {
                SsRoad.IsChecked = true;
            }
            else
            {
                SsSw1.IsChecked = true;
            }
        }

        private void TBCutOutSwitch()
        {
            if(TBCutOut.IsChecked == true)
            {
                TBFreight.IsChecked = true;
            }
            else if(TBFreight.IsChecked == true)
            {
                TBPassenger.IsChecked = true;
            }
            else
            {
                TBCutOut.IsChecked = true;
            }
        }
    }
}
