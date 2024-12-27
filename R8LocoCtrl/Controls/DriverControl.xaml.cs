using HotKeyLibrary;
using R8LocoCtrl.Interface;
using R8LocoCtrl.ViewModel;
using System;
using System.Linq;
using System.Windows.Controls;

namespace R8LocoCtrl.Controls
{
    /// <summary>
    /// Interaction logic for DriverControl.xaml
    /// </summary>
    public partial class DriverControl : UserControl
    {
        public DriverControl()
        {
            InitializeComponent();
            CommandRegistry.Instance.SubscribeToCommand(Commands.WiperControl, SwitchWiper);
            CommandRegistry.Instance.SubscribeToCommand(Commands.ReverserLeverUp, ReverserUp);
            CommandRegistry.Instance.SubscribeToCommand(Commands.ReverserLeverDown, ReverserDown);
            CommandRegistry.Instance.SubscribeToCommand(Commands.TrainBrakeApplyLarge, LargeTrainBrakeApply);
            CommandRegistry.Instance.SubscribeToCommand(Commands.TrainBrakeApplySmall, SmallTrainBrakeApply);
            CommandRegistry.Instance.SubscribeToCommand(Commands.TrainBrakeReleaseLarge, LargeTrainBrakeRelease);
            CommandRegistry.Instance.SubscribeToCommand(Commands.TrainBrakeReleaseSmall, SmallTrainBrakeRelease);
            CommandRegistry.Instance.SubscribeToCommand(Commands.TrainBrakeReleaseFull, FullTrainBrakeRelease);
            CommandRegistry.Instance.SubscribeToCommand(Commands.IndyBrakeDecreaseLarge, LargeIndyBrakeRelease);
            CommandRegistry.Instance.SubscribeToCommand(Commands.IndyBrakeDecreaseSmall, SmallIndyBrakeRelease);
            CommandRegistry.Instance.SubscribeToCommand(Commands.IndyBrakeFull, FullIndyBrakesApply);
            CommandRegistry.Instance.SubscribeToCommand(Commands.IndyBrakeIncreaseLarge, LargeIndyBrakeApply);
            CommandRegistry.Instance.SubscribeToCommand(Commands.IndyBrakeIncreaseSmall, SmallIndyBrakeApply);
            CommandRegistry.Instance.SubscribeToCommand(Commands.IndyBrakeOff, FullIndyBrakeRelease);
            CommandRegistry.Instance.SubscribeToCommand(Commands.DynBrakeDecreaseLarge, LargeDynBrakeRelease);
            CommandRegistry.Instance.SubscribeToCommand(Commands.DynBrakeDecreaseSmall, SmallDynBrakeRelease);
            CommandRegistry.Instance.SubscribeToCommand(Commands.DynBrakeFull, FullDynBrakesApply);
            CommandRegistry.Instance.SubscribeToCommand(Commands.DynBrakeIncreaseLarge, LargeDynBrakeApply);
            CommandRegistry.Instance.SubscribeToCommand(Commands.DynBrakeIncreaseSmall, SmallDynBrakeApply);
            CommandRegistry.Instance.SubscribeToCommand(Commands.DynBrakeOff, FullDynBrakeRelease);
            CommandRegistry.Instance.SubscribeToCommand(Commands.ThrottleDecrease, ThrottleDecreaseApply);
            CommandRegistry.Instance.SubscribeToCommand(Commands.ThrottleIncrease, ThrottleIncreaseApply);
        }

        private void FullDynBrakeRelease()
        {
            if (DynBrakes.IsEnabled == false)
                return;

            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }

            actionViewModel.DynamicBrakes = 0;
        }

        private void FullDynBrakesApply()
        {
            if (DynBrakes.IsEnabled == false)
                return;

            var actionViewModel = DataContext as Run8ActionsViewModel;
            if (actionViewModel == null)
            {
                return;
            }

            actionViewModel.DynamicBrakes = 255;
        }

        private void FullIndyBrakeRelease()
        {

            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }

            actionViewModel.IndyBrakes = 0;
        }

        private void FullIndyBrakesApply()
        {
            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }

            actionViewModel.IndyBrakes = 255;
        }

        private void FullTrainBrakeRelease()
        {
            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }

            actionViewModel.TrainBrakes = 0;
        }

        private void LargeDynBrakeApply()
        {
            if (DynBrakes.IsEnabled == false)
                return;

            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }

            var change = actionViewModel.DynBrakeLC;
            var newValue = actionViewModel.DynamicBrakes + change;
            actionViewModel.DynamicBrakes = Math.Min(newValue, 255);
        }

        private void LargeDynBrakeRelease()
        {
            if (DynBrakes.IsEnabled == false)
                return;

            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }

            var change = actionViewModel.DynBrakeLC;
            var newValue = actionViewModel.DynamicBrakes - change;
            actionViewModel.DynamicBrakes = Math.Max(newValue, 0);
        }

        private void LargeIndyBrakeApply()
        {
            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }

            var change = actionViewModel.IndyBrakeLC;
            var newValue = actionViewModel.IndyBrakes + change;
            actionViewModel.IndyBrakes = Math.Min(255, newValue);
        }

        private void LargeIndyBrakeRelease()
        {
            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }

            var change = actionViewModel.IndyBrakeLC;
            var newValue = actionViewModel.IndyBrakes - change;
            actionViewModel.IndyBrakes = Math.Max(0, newValue);
        }

        private void LargeTrainBrakeApply()
        {
            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }

            var change = actionViewModel.TrainBrakeLC;
            actionViewModel.TrainBrakes = Math.Min(actionViewModel.TrainBrakes + change, 255);
        }

        private void LargeTrainBrakeRelease()
        {
            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }

            var change = actionViewModel.TrainBrakeLC;
            actionViewModel.TrainBrakes = Math.Max(actionViewModel.TrainBrakes - change, 0);
        }

        private void ReverserDown()
        {
            if(ReverserSlider.IsEnabled == false)
                return;

            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }

            if(actionViewModel.ReverserPosition == ReverserPositions.Neutral)
                actionViewModel.ReverserPosition = ReverserPositions.Reverse;
            else if(actionViewModel.ReverserPosition == ReverserPositions.Forward)
                actionViewModel.ReverserPosition = ReverserPositions.Neutral;
        }

        private void ReverserUp()
        {
            if (ReverserSlider.IsEnabled == false)
                return;

            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }

            if(actionViewModel.ReverserPosition == ReverserPositions.Neutral)
                actionViewModel.ReverserPosition = ReverserPositions.Forward;
            else if(actionViewModel.ReverserPosition == ReverserPositions.Reverse)
                actionViewModel.ReverserPosition = ReverserPositions.Neutral;
        }

        private void SmallDynBrakeApply()
        {
            if (DynBrakes.IsEnabled == false)
                return;

            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }

            if(actionViewModel.DynamicBrakes < 255)
                actionViewModel.DynamicBrakes++;
        }

        private void SmallDynBrakeRelease()
        {
            if (DynBrakes.IsEnabled == false)
                return;

            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }

            if(actionViewModel.DynamicBrakes > 0)
                actionViewModel.DynamicBrakes--;
        }

        private void SmallIndyBrakeApply()
        {
            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }

            if(actionViewModel.IndyBrakes < 255)
                actionViewModel.IndyBrakes++;
        }

        private void SmallIndyBrakeRelease()
        {
            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }

            if(actionViewModel.IndyBrakes > 0)
                actionViewModel.IndyBrakes--;
        }

        private void SmallTrainBrakeApply()
        {
            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }
            if(actionViewModel.TrainBrakes < 255)
                actionViewModel.TrainBrakes++;
        }

        private void SmallTrainBrakeRelease()
        {
            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }

            if(actionViewModel.TrainBrakes > 0)
                actionViewModel.TrainBrakes--;
        }

        private void SwitchWiper()
        {
            if(WipersOff.IsChecked == true)
            {
                WipersLow.IsChecked = true;
            }
            else if(WipersLow.IsChecked == true)
            {
                WipersMid.IsChecked = true;
            }
            else if(WipersMid.IsChecked == true)
            {
                WipersFull.IsChecked = true;
            }
            else
            {
                WipersOff.IsChecked = true;
            }
        }

        private void ThrottleDecreaseApply()
        {
            if(ThrottleSlider.IsEnabled == false)
                return;

            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }

            if(actionViewModel.ThrottleValue == 0)
                return;

            actionViewModel.ThrottleValue--;
        }

        private void ThrottleIncreaseApply()
        {
            if (ThrottleSlider.IsEnabled == false)
                return;

            var actionViewModel = DataContext as Run8ActionsViewModel;
            if(actionViewModel == null)
            {
                return;
            }

            if(actionViewModel.ThrottleValue == 8)
                return;

            actionViewModel.ThrottleValue++;
        }
    }
}
