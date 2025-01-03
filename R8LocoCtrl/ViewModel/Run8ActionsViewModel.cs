﻿//-----------------------------------------------------------------------
// <copyright file="Run8ActionsViewModel.cs" company="Xcoder Software">
//     Author: Gil Yoder
//     Copyright (c) Xcoder Software. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using CommunityToolkit.Mvvm.Input;
using HotKeyLibrary;
using R8LocoCtrl.Interface;
using R8LocoCtrl.Tools;
using SimpleUdp;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace R8LocoCtrl.ViewModel
{
    public class Run8ActionsViewModel : ViewModelBase
    {
        private RelayCommand<bool>? _AutoABCommand = null;
        private RelayCommand<bool>? _AutoCBCommand = null;
        private RelayCommand<bool>? _AutoEOTCommand = null;
        private RelayCommand<bool>? _AutoMUCommand = null;
        private RelayCommand<bool>? _AutoStartTrainCommand = null;
        private RelayCommand<bool>? _ControlSwitchCommand = null;
        private RelayCommand<bool>? _DPUFenceDecreaseCommand = null;
        private RelayCommand<bool>? _DPUFenceIncreaseCommand = null;
        private RelayCommand<DTMFCommandParameter>? _DTMFToneCommand = null;
        private RelayCommand<bool>? _DynBrakeSetupCommand = null;
        private RelayCommand<bool>? _DynBrakeSwitchCommand = null;
        private RelayCommand<bool[]>? _EngineStartCommand = null;
        private RelayCommand<bool>? _EngRunSwitchCommand = null;
        private RelayCommand<bool>? _GenFieldSwitchCommand = null;
        private RelayCommand<bool>? _HEPCommand = null;
        private RelayCommand<IsolationSwitchValues>? _IsolationSwitchCommand = null;
        private RelayCommand<MUHLSwitchValues>? _MUHeadlightSwitchCommand = null;
        private RelayCommand<bool>? _SendAlerterCommand = null;
        private RelayCommand<bool>? _SendBellCommand = null;
        private RelayCommand<bool>? _SendCabLightSwitchCommand = null;
        private RelayCommand<bool>? _SendDistanceCountDownCommand = null;
        private RelayCommand<bool>? _SendDistanceCountUpCommand = null;
        private RelayCommand<bool>? _SendEOTEmgStopCommand = null;
        private RelayCommand<HeadlightValues>? _SendFrontHeadlightCommand = null;
        private RelayCommand<WiperSwitchValues>? _SendWipersCommand = null;
        private RelayCommand<bool>? _SendGaugeLightSwitchCommand = null;
        private RelayCommand<bool>? _SendHornCommand = null;
        private RelayCommand? _SendHornSequenceCommand = null;
        private RelayCommand<bool>? _SendIndyBailOffCommand = null;
        private RelayCommand<bool>? _SendRadioChannelModeCommand = null;
        private RelayCommand<bool>? _SendRadioDTMFModeCommand = null;
        private RelayCommand<bool>? _SendRadioVolumeDecreaseCommand = null;
        private RelayCommand<bool>? _SendRadioVolumeIncreaseCommand = null;
        private RelayCommand<bool>? _SendRadioVolumeMuteCommand = null;
        private RelayCommand<HeadlightValues>? _SendRearHeadlightCommand = null;
        private RelayCommand<bool>? _SendReleaseParkingCommand = null;
        private RelayCommand<bool>? _SendSanderCommand = null;
        private RelayCommand<bool>? _SendSetParkingCommand = null;
        private RelayCommand<bool>? _SendSlowSpeedDecrementCommand = null;
        private RelayCommand<bool>? _SendSlowSpeedIncrementCommand = null;
        private RelayCommand<bool>? _SendSlowSpeedOnOffCommand = null;
        private RelayCommand<bool>? _SendStepLightSwitchCommand = null;
        private RelayCommand<ServiceSelectorSwitchValues>? _ServiceSelectorCommand = null;
        private RelayCommand<bool>? _DPUThrottleDecreaseCommand = null;
        private RelayCommand<bool>? _DPUThrottleIncreaseCommand = null;
        private RelayCommand<bool[]>? _TractionMotorsCommand = null;
        private RelayCommand<TrainBrakeCutoutValues>? _TrainBrakeCutOutCommand = null;
        private CommandRegistry commandRegistry;
        private int dynamicBrakes;
        private int dynBrakeLC;
        private int indyBrakeLC;
        private int indyBrakes;
        private bool isDynamicBrakesEnabled = true;
        private bool isReverserEnabled = true;
        private bool isThrottleEnabled = true;
        private ReverserPositions reverserPosition = ReverserPositions.Neutral;
        private Run8SenderClient? senderClient;
        private int throttleValue;
        private int trainBrakeLC;
        private int trainBrakes;

        public Run8ActionsViewModel()
        {
            commandRegistry = CommandRegistry.Instance;

            commandRegistry.SubscribeToCommand(Commands.AlerterReset, SendAlerterExecute);
            commandRegistry.SubscribeToCommand(Commands.AutoABTrain, AutoABExecute);
            commandRegistry.SubscribeToCommand(Commands.AutoCBTrain, AutoABExecute);
            commandRegistry.SubscribeToCommand(Commands.AutoEOTTrain, AutoEOTExecute);
            commandRegistry.SubscribeToCommand(Commands.AutoMUTrain, AutoMUExecute);
            commandRegistry.SubscribeToCommand(Commands.AutoStartTrain, AutoStartTrainExecute);
            commandRegistry.SubscribeToCommand(Commands.BellOnOff, SendBellExecute);
            commandRegistry.SubscribeToCommand(Commands.CB_Control, ControlSwitchExecute);
            commandRegistry.SubscribeToCommand(Commands.CB_DynBrake, DynBrakeSwitchExecute);
            commandRegistry.SubscribeToCommand(Commands.CB_EngRun, EngRunSwitchExecute);
            commandRegistry.SubscribeToCommand(Commands.CB_GenField, GenFieldSwitchExecute);
            commandRegistry.SubscribeToCommand(Commands.DistanceCounterDown, SendDistanceCountDownExecute);
            commandRegistry.SubscribeToCommand(Commands.DistanceCounterUp, SendDistanceCountUpExecute);
            commandRegistry.SubscribeToCommand(Commands.DPUDynBrakeSetup, DynBrakeSetupExecute);
            commandRegistry.SubscribeToCommand(Commands.DPUFenceDecrease, DPUFenceDecreaseExecute);
            commandRegistry.SubscribeToCommand(Commands.DPUFenceIncrease, DPUFenceIncreaseExecute);
            commandRegistry.SubscribeToCommand(Commands.DTMF0, DTMF0);
            commandRegistry.SubscribeToCommand(Commands.DTMF1, DTMF1);
            commandRegistry.SubscribeToCommand(Commands.DTMF2, DTMF2);
            commandRegistry.SubscribeToCommand(Commands.DTMF3, DTMF3);
            commandRegistry.SubscribeToCommand(Commands.DTMF4, DTMF4);
            commandRegistry.SubscribeToCommand(Commands.DTMF5, DTMF5);
            commandRegistry.SubscribeToCommand(Commands.DTMF6, DTMF6);
            commandRegistry.SubscribeToCommand(Commands.DTMF7, DTMF7);
            commandRegistry.SubscribeToCommand(Commands.DTMF8, DTMF8);
            commandRegistry.SubscribeToCommand(Commands.DTMF9, DTMF9);
            commandRegistry.SubscribeToCommand(Commands.DTMFPound, DTMFPound);
            commandRegistry.SubscribeToCommand(Commands.DTMFStar, DTMFStar);
            commandRegistry.SubscribeToCommand(Commands.EOTEmgStop, SendEOTEmgStopExecute);
            commandRegistry.SubscribeToCommand(Commands.HEPSwitch, HEPExecute);
            commandRegistry.SubscribeToCommand(Commands.Horn, SendHornExecute);
            commandRegistry.SubscribeToCommand(Commands.HornSequencer, SendHornSequenceExecute);
            commandRegistry.SubscribeToCommand(Commands.IndyBrakeBail, SendIndyBailOffExecute);
            commandRegistry.SubscribeToCommand(Commands.ParkingBrakeReset, SendReleaseParkingExecute);
            commandRegistry.SubscribeToCommand(Commands.ParkingBrakeSet, SendSetParkingExecute);
            commandRegistry.SubscribeToCommand(Commands.RadioChannelMode, SendRadioChannelModeExecute);
            commandRegistry.SubscribeToCommand(Commands.RadioDTMFMode, SendRadioDTMFModeExecute);
            commandRegistry.SubscribeToCommand(Commands.RadioVolDecrease, SendRadioVolumeDecreaseExecute);
            commandRegistry.SubscribeToCommand(Commands.RadioVolIncrease, SendRadioVolumeIncreaseExecute);
            commandRegistry.SubscribeToCommand(Commands.RadioVolMute, SendRadioVolumeMuteExecute);
            commandRegistry.SubscribeToCommand(Commands.SanderToggle, SenderSanderExecute);
            commandRegistry.SubscribeToCommand(Commands.SlowSpeedDecrease, SendSlowSpeedDecrementExecute);
            commandRegistry.SubscribeToCommand(Commands.SlowSpeedIncrease, SendSlowSpeedIncrementExecute);
            commandRegistry.SubscribeToCommand(Commands.DPUThrottleDecrease, DPUThrottleDecreaseExecute);
            commandRegistry.SubscribeToCommand(Commands.DPUThrottleIncrease, DPUThrottleIncreaseExecute);
        }

        public RelayCommand<bool> AutoABCommand
        {
            get
            {
                _AutoABCommand ??= new RelayCommand<bool>(AutoABExecute);
                return _AutoABCommand;
            }
        }

        public RelayCommand<bool> AutoCBCommand
        {
            get
            {
                _AutoCBCommand ??= new RelayCommand<bool>(AutoCBExecute);
                return _AutoCBCommand;
            }
        }

        public RelayCommand<bool> AutoEOTCommand
        {
            get
            {
                _AutoEOTCommand ??= new RelayCommand<bool>(AutoEOTExecute);
                return _AutoEOTCommand;
            }
        }

        public RelayCommand<bool> AutoMUCommand
        {
            get
            {
                _AutoMUCommand ??= new RelayCommand<bool>(AutoMUExecute);
                return _AutoMUCommand;
            }
        }

        public RelayCommand<bool> AutoStartTrainCommand
        {
            get
            {
                _AutoStartTrainCommand ??= new RelayCommand<bool>(AutoStartTrainExecute);
                return _AutoStartTrainCommand;
            }
        }

        public RelayCommand<bool> ControlSwitchCommand
        {
            get
            {
                _ControlSwitchCommand ??= new RelayCommand<bool>(ControlSwitchExecute);
                return _ControlSwitchCommand;
            }
        }

        public RelayCommand<bool> DPUFenceDecreaseCommand
        {
            get
            {
                _DPUFenceDecreaseCommand ??= new RelayCommand<bool>(DPUFenceDecreaseExecute);
                return _DPUFenceDecreaseCommand;
            }
        }

        public RelayCommand<bool> DPUFenceIncreaseCommand
        {
            get
            {
                _DPUFenceIncreaseCommand ??= new RelayCommand<bool>(DPUFenceIncreaseExecute);
                return _DPUFenceIncreaseCommand;
            }
        }

        public RelayCommand<DTMFCommandParameter> DTMFToneCommand
        {
            get
            {
                _DTMFToneCommand ??= new RelayCommand<DTMFCommandParameter>(DTMFToneExecute!);
                return _DTMFToneCommand;
            }
        }

        public int DynamicBrakes
        {
            get
            {
                return dynamicBrakes;
            }
            set
            {
                SetProperty(ref dynamicBrakes, value);
            }
        }

        public int DynBrakeLC
        {
            get
            {
                return dynBrakeLC;
            }
            set
            {
                SetProperty(ref dynBrakeLC, value);
            }
        }

        public RelayCommand<bool> DynBrakeSetupCommand
        {
            get
            {
                _DynBrakeSetupCommand ??= new RelayCommand<bool>(DynBrakeSetupExecute);
                return _DynBrakeSetupCommand;
            }
        }

        public RelayCommand<bool> DynBrakeSwitchCommand
        {
            get
            {
                _DynBrakeSwitchCommand ??= new RelayCommand<bool>(DynBrakeSwitchExecute);
                return _DynBrakeSwitchCommand;
            }
        }

        public RelayCommand<bool[]> EngineStartCommand
        {
            get
            {
                _EngineStartCommand ??= new RelayCommand<bool[]>(EngineStartExecute);
                return _EngineStartCommand;
            }
        }

        public RelayCommand<bool> EngRunSwitchCommand
        {
            get
            {
                _EngRunSwitchCommand ??= new RelayCommand<bool>(EngRunSwitchExecute);
                return _EngRunSwitchCommand;
            }
        }

        public RelayCommand<bool> GenFieldSwitchCommand
        {
            get
            {
                _GenFieldSwitchCommand ??= new RelayCommand<bool>(GenFieldSwitchExecute);
                return _GenFieldSwitchCommand;
            }
        }

        public RelayCommand<bool> HEPCommand
        {
            get
            {
                _HEPCommand ??= new RelayCommand<bool>(HEPExecute);
                return _HEPCommand;
            }
        }

        public int IndyBrakeLC
        {
            get
            {
                return indyBrakeLC;
            }
            set
            {
                SetProperty(ref indyBrakeLC, value);
            }
        }

        public int IndyBrakes
        {
            get
            {
                return indyBrakes;
            }
            set
            {
                SetProperty(ref indyBrakes, value);
            }
        }

        public bool IsDynamicBrakesEnabled
        {
            get
            {
                return isDynamicBrakesEnabled;
            }
            set
            {
                SetProperty(ref isDynamicBrakesEnabled, value);
            }
        }

        public RelayCommand<IsolationSwitchValues> IsolationSwitchCommand
        {
            get
            {
                _IsolationSwitchCommand ??= new RelayCommand<IsolationSwitchValues>(IsolationSwitchExecute);
                return _IsolationSwitchCommand;
            }
        }

        public bool IsReverserEnabled
        {
            get
            {
                return isReverserEnabled;
            }
            set
            {
                SetProperty(ref isReverserEnabled, value);
            }
        }

        public bool IsThrottleEnabled
        {
            get
            {
                return isThrottleEnabled;
            }
            set
            {
                SetProperty(ref isThrottleEnabled, value);
            }
        }

        public RelayCommand<MUHLSwitchValues> MUHeadlightSwitchCommand
        {
            get
            {
                _MUHeadlightSwitchCommand ??= new RelayCommand<MUHLSwitchValues>(MUHeadlightSwitchExecute);
                return _MUHeadlightSwitchCommand;
            }
        }

        public ReverserPositions ReverserPosition
        {
            get
            {
                return reverserPosition;
            }
            set
            {
                SetProperty(ref reverserPosition, value);
            }
        }

        public RelayCommand<bool> SendAlerterCommand
        {
            get
            {
                _SendAlerterCommand ??= new RelayCommand<bool>(SendAlerterExecute);
                return _SendAlerterCommand;
            }
        }

        public RelayCommand<bool> SendBellCommand
        {
            get
            {
                _SendBellCommand ??= new RelayCommand<bool>(SendBellExecute);
                return _SendBellCommand;
            }
        }

        public RelayCommand<bool> SendCabLightSwitchCommand
        {
            get
            {
                _SendCabLightSwitchCommand ??= new RelayCommand<bool>(SendCabLightSwitchExecute);
                return _SendCabLightSwitchCommand;
            }
        }

        public RelayCommand<bool> SendDistanceCountDownCommand
        {
            get
            {
                _SendDistanceCountDownCommand ??= new RelayCommand<bool>(SendDistanceCountDownExecute);
                return _SendDistanceCountDownCommand;
            }
        }

        public RelayCommand<bool> SendDistanceCountUpCommand
        {
            get
            {
                _SendDistanceCountUpCommand ??= new RelayCommand<bool>(SendDistanceCountUpExecute);
                return _SendDistanceCountUpCommand;
            }
        }

        public RelayCommand<bool> SendEOTEmgStopCommand
        {
            get
            {
                _SendEOTEmgStopCommand ??= new RelayCommand<bool>(SendEOTEmgStopExecute);
                return _SendEOTEmgStopCommand;
            }
        }

        /// <summary>
        /// Gets the command that sends the front headlight value when its radio buttons change.
        /// </summary>
        public RelayCommand<HeadlightValues> SendFrontHeadlightCommand
        {
            get
            {
                _SendFrontHeadlightCommand ??= new RelayCommand<HeadlightValues>(SendFrontHeadlightExecute);
                return _SendFrontHeadlightCommand;
            }
        }

        public RelayCommand<WiperSwitchValues> SendWipersCommand
        {
            get
            {
                _SendWipersCommand ??= new RelayCommand<WiperSwitchValues>(SendWiperExecute);
                return _SendWipersCommand;
            }
        }

        public RelayCommand<bool> SendGaugeLightSwitchCommand
        {
            get
            {
                _SendGaugeLightSwitchCommand ??= new RelayCommand<bool>(SendGaugeLightSwitchExecute);
                return _SendGaugeLightSwitchCommand;
            }
        }

        public RelayCommand<bool> SendHornCommand
        {
            get
            {
                _SendHornCommand ??= new RelayCommand<bool>(SendHornExecute);
                return _SendHornCommand;
            }
        }

        public RelayCommand SendHornSequenceCommand
        {
            get
            {
                _SendHornSequenceCommand ??= new RelayCommand(SendHornSequenceExecute);
                return _SendHornSequenceCommand;
            }
        }

        public RelayCommand<bool> SendIndyBailOffCommand
        {
            get
            {
                _SendIndyBailOffCommand ??= new RelayCommand<bool>(SendIndyBailOffExecute);
                return _SendIndyBailOffCommand;
            }
        }

        public RelayCommand<bool> SendRadioChannelModeCommand
        {
            get
            {
                _SendRadioChannelModeCommand ??= new RelayCommand<bool>(SendRadioChannelModeExecute);
                return _SendRadioChannelModeCommand;
            }
        }

        public RelayCommand<bool> SendRadioDTMFModeCommand
        {
            get
            {
                _SendRadioDTMFModeCommand ??= new RelayCommand<bool>(SendRadioDTMFModeExecute);
                return _SendRadioDTMFModeCommand;
            }
        }

        public RelayCommand<bool> SendRadioVolumeDecreaseCommand
        {
            get
            {
                _SendRadioVolumeDecreaseCommand ??= new RelayCommand<bool>(SendRadioVolumeDecreaseExecute);
                return _SendRadioVolumeDecreaseCommand;
            }
        }

        public RelayCommand<bool> SendRadioVolumeIncreaseCommand
        {
            get
            {
                _SendRadioVolumeIncreaseCommand ??= new RelayCommand<bool>(SendRadioVolumeIncreaseExecute);
                return _SendRadioVolumeIncreaseCommand;
            }
        }

        public RelayCommand<bool> SendRadioVolumeMuteCommand
        {
            get
            {
                _SendRadioVolumeMuteCommand ??= new RelayCommand<bool>(SendRadioVolumeMuteExecute);
                return _SendRadioVolumeMuteCommand;
            }
        }

        /// <summary>
        /// Gets the value that sets the rear headlights value when its radio buttons change.
        /// </summary>
        public RelayCommand<HeadlightValues> SendRearHeadlightCommand
        {
            get
            {
                _SendRearHeadlightCommand ??= new RelayCommand<HeadlightValues>(SendRearHeadlightExecute);
                return _SendRearHeadlightCommand;
            }
        }

        public RelayCommand<bool> SendReleaseParkingCommand
        {
            get
            {
                _SendReleaseParkingCommand ??= new RelayCommand<bool>(SendReleaseParkingExecute);
                return _SendReleaseParkingCommand;
            }
        }

        public RelayCommand<bool> SendSanderCommand
        {
            get
            {
                _SendSanderCommand ??= new RelayCommand<bool>(SenderSanderExecute);
                return _SendSanderCommand;
            }
        }

        public RelayCommand<bool> SendSetParkingCommand
        {
            get
            {
                _SendSetParkingCommand ??= new RelayCommand<bool>(SendSetParkingExecute);
                return _SendSetParkingCommand;
            }
        }

        public RelayCommand<bool> SendSlowSpeedDecrementCommand
        {
            get
            {
                _SendSlowSpeedDecrementCommand ??= new RelayCommand<bool>(SendSlowSpeedDecrementExecute);
                return _SendSlowSpeedDecrementCommand;
            }
        }

        public RelayCommand<bool> SendSlowSpeedIncrementCommand
        {
            get
            {
                _SendSlowSpeedIncrementCommand ??= new RelayCommand<bool>(SendSlowSpeedIncrementExecute);
                return _SendSlowSpeedIncrementCommand;
            }
        }

        public RelayCommand<bool> SendSlowSpeedOnOffCommand
        {
            get
            {
                _SendSlowSpeedOnOffCommand ??= new RelayCommand<bool>(SendSlowSpeedOnOffExecute!);
                return _SendSlowSpeedOnOffCommand;
            }
        }

        public RelayCommand<bool> SendStepLightSwitchCommand
        {
            get
            {
                _SendStepLightSwitchCommand ??= new RelayCommand<bool>(SendStepLightSwitchExecute);
                return _SendStepLightSwitchCommand;
            }
        }

        public RelayCommand<ServiceSelectorSwitchValues> ServiceSelectorCommand
        {
            get
            {
                _ServiceSelectorCommand ??= new RelayCommand<ServiceSelectorSwitchValues>(ServiceSelectorExecute);
                return _ServiceSelectorCommand;
            }
        }

        public RelayCommand<bool> DPUThrottleDecreaseCommand
        {
            get
            {
                _DPUThrottleDecreaseCommand ??= new RelayCommand<bool>(DPUThrottleDecreaseExecute);
                return _DPUThrottleDecreaseCommand;
            }
        }

        public RelayCommand<bool> DPUThrottleIncreaseCommand
        {
            get
            {
                _DPUThrottleIncreaseCommand ??= new RelayCommand<bool>(DPUThrottleIncreaseExecute);
                return _DPUThrottleIncreaseCommand;
            }
        }

        public int ThrottleValue
        {
            get
            {
                return throttleValue;
            }
            set
            {
                SetProperty(ref throttleValue, value);
            }
        }

        public RelayCommand<bool[]> TractionMotorsCommand
        {
            get
            {
                _TractionMotorsCommand ??= new RelayCommand<bool[]>(TractionMotorsExecute!);
                return _TractionMotorsCommand;
            }
        }

        public RelayCommand<TrainBrakeCutoutValues> TrainBrakeCutOutCommand
        {
            get
            {
                _TrainBrakeCutOutCommand ??= new RelayCommand<TrainBrakeCutoutValues>(TrainBrakeCutOutExecute);
                return _TrainBrakeCutOutCommand;
            }
        }

        public int TrainBrakeLC
        {
            get
            {
                return trainBrakeLC;
            }
            set
            {
                SetProperty(ref trainBrakeLC, value);
            }
        }

        public int TrainBrakes
        {
            get
            {
                return trainBrakes;
            }
            set
            {
                SetProperty(ref trainBrakes, value);
            }
        }

        private void DTMF0(bool isButtonPressed)
        {
            DTMFToneExecute(
                new DTMFCommandParameter { IsButtonPressed = isButtonPressed, Tone = DTMFToneValues.DTMF0 });
        }

        private void DTMF1(bool isButtonPressed)
        {
            DTMFToneExecute(
                new DTMFCommandParameter { IsButtonPressed = isButtonPressed, Tone = DTMFToneValues.DTMF1 });
        }

        private void DTMF2(bool isButtonPressed)
        {
            DTMFToneExecute(
                new DTMFCommandParameter { IsButtonPressed = isButtonPressed, Tone = DTMFToneValues.DTMF2 });
        }

        private void DTMF3(bool isButtonPressed)
        {
            DTMFToneExecute(
                new DTMFCommandParameter { IsButtonPressed = isButtonPressed, Tone = DTMFToneValues.DTMF3 });
        }

        private void DTMF4(bool isButtonPressed)
        {
            DTMFToneExecute(
                new DTMFCommandParameter { IsButtonPressed = isButtonPressed, Tone = DTMFToneValues.DTMF4 });
        }

        private void DTMF5(bool isButtonPressed)
        {
            DTMFToneExecute(
                new DTMFCommandParameter { IsButtonPressed = isButtonPressed, Tone = DTMFToneValues.DTMF5 });
        }

        private void DTMF6(bool isButtonPressed)
        {
            DTMFToneExecute(
                new DTMFCommandParameter { IsButtonPressed = isButtonPressed, Tone = DTMFToneValues.DTMF6 });
        }

        private void DTMF7(bool isButtonPressed)
        {
            DTMFToneExecute(
                new DTMFCommandParameter { IsButtonPressed = isButtonPressed, Tone = DTMFToneValues.DTMF7 });
        }

        private void DTMF8(bool isButtonPressed)
        {
            DTMFToneExecute(
                new DTMFCommandParameter { IsButtonPressed = isButtonPressed, Tone = DTMFToneValues.DTMF8 });
        }

        private void DTMF9(bool isButtonPressed)
        {
            DTMFToneExecute(
                new DTMFCommandParameter { IsButtonPressed = isButtonPressed, Tone = DTMFToneValues.DTMF9 });
        }

        private void DTMFPound(bool isButtonPressed)
        {
            DTMFToneExecute(
                new DTMFCommandParameter { IsButtonPressed = isButtonPressed, Tone = DTMFToneValues.DTMFPound });
        }

        private void DTMFStar(bool isButtonPressed)
        {
            DTMFToneExecute(
                new DTMFCommandParameter { IsButtonPressed = isButtonPressed, Tone = DTMFToneValues.DTMFStar });
        }

        protected virtual void AutoABExecute(bool buttonIsPressed)
        {
            senderClient?.SendAutoABTrain(buttonIsPressed);
        }

        protected virtual void AutoCBExecute(bool buttonIsPressed)
        {
            senderClient?.SendAutoCBTrain(buttonIsPressed);
        }

        protected virtual void AutoEOTExecute(bool buttonIsPressed)
        {
            senderClient?.SendAutoEOTTrain(buttonIsPressed);
        }

        protected virtual void AutoMUExecute(bool buttonIsPressed)
        {
            senderClient?.SendAutoMUTrain(buttonIsPressed);
        }

        protected virtual void AutoStartTrainExecute(bool buttonIsPressed)
        {
            senderClient?.SendAutoStartTrain(buttonIsPressed);
        }

        protected virtual void ControlSwitchExecute(bool parameter)
        {
            senderClient?.SendControlSwitch(parameter);
        }

        protected virtual void DPUFenceDecreaseExecute(bool parameter)
        {
            senderClient?.SendDPUFenceDecrease(parameter);
        }

        protected virtual void DPUFenceIncreaseExecute(bool parameter)
        {
            senderClient?.SendDPUFenceIncrease(parameter);
        }

        // Todo: Not a bool param
        protected virtual void DTMFToneExecute(DTMFCommandParameter parameter)
        {
            if(parameter == null)
                return;
            senderClient?.SendDTMFTone(parameter.Tone, parameter.IsButtonPressed);
        }

        protected virtual void DynBrakeSetupExecute(bool parameter)
        {
            senderClient?.SendDynBrakeSetup(parameter);
        }

        protected virtual void DynBrakeSwitchExecute(bool parameter)
        {
            senderClient?.SendDynBrakeSwitch(parameter);
        }

        // Todo: This is not a bool param
        protected virtual void EngineStartExecute(bool[]? values)
        {
            ArgumentNullException.ThrowIfNull(values);
            var isKeyDown = values[0];
            var isChecked = values[1];

            if(isChecked)
                senderClient?.SendEngineStart(isKeyDown);
            else
                senderClient?.SendEngineStop(isKeyDown);
        }

        protected virtual void EngRunSwitchExecute(bool parameter)
        {
            senderClient?.SendEngRunSwitch(parameter);
        }

        protected virtual void GenFieldSwitchExecute(bool parameter)
        {
            senderClient?.SendGenFieldSwitch(parameter);
        }

        protected virtual void HEPExecute(bool parameter)
        {
            senderClient?.SendHEPSwitch(parameter);
        }

        // No hot key
        protected virtual void IsolationSwitchExecute(IsolationSwitchValues parameter)
        {
            senderClient?.SendIsolationSwitch(parameter);
        }

        // No hot key
        protected virtual void MUHeadlightSwitchExecute(MUHLSwitchValues parameter)
        {
            senderClient?.SendMUHLSwitch(parameter);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            switch(propertyName)
            {
                case "ReverserPosition":
                    senderClient?.SendReverserLever(ReverserPosition);
                    break;
                case "ThrottleValue":
                    if(ThrottleValue == 0)
                    {
                        IsDynamicBrakesEnabled = true;
                        if(DynamicBrakes < 0.01)
                        {
                            IsReverserEnabled = true;
                        }
                    }
                    else
                    {
                        IsDynamicBrakesEnabled = false;
                        IsReverserEnabled = false;
                    }
                    senderClient?.SendThrottleLever((byte)ThrottleValue);
                    break;
                case "DynamicBrakes":
                    if(DynamicBrakes == 0)
                    {
                        IsThrottleEnabled = true;
                        if(ThrottleValue < 0.01)
                        {
                            IsReverserEnabled = true;
                        }
                    }
                    else
                    {
                        IsThrottleEnabled = false;
                        IsReverserEnabled = false;
                    }

                    senderClient?.SendDynBrakeLever((byte)DynamicBrakes);
                    break;
                case "TrainBrakes":
                    senderClient?.SendTrainBrakeLever((byte)TrainBrakes);
                    break;

                case "IndyBrakes":
                    senderClient?.SendIndyBrakeLever((byte)IndyBrakes);
                    break;
            }

            base.OnPropertyChanged(propertyName);
        }

        protected virtual void SendAlerterExecute(bool parameter)
        {
            senderClient?.SendAlerter(parameter);
        }

        protected virtual void SendBellExecute(bool parameter)
        {
            senderClient?.SendBell(parameter);
        }

        protected virtual void SendCabLightSwitchExecute(bool parameter)
        {
            senderClient?.SendCabLightSwitch(parameter);
        }

        protected virtual void SendDistanceCountDownExecute(bool isPressed)
        {
            senderClient?.SendDistanceCounter(false, isPressed);
        }

        protected virtual void SendDistanceCountUpExecute(bool isPressed)
        {
            senderClient?.SendDistanceCounter(true, isPressed);
        }

        protected virtual void SendEOTEmgStopExecute(bool parameter)
        {
            senderClient?.SendEOTEmgStop(parameter);
        }

        protected virtual void SenderSanderExecute(bool parameter)
        {
            senderClient?.SendSander(parameter);
        }


        /// <summary>
        /// Sets the front headlight to the value specified.
        /// </summary>
        /// <param name="parameter">The specified value.</param>
        protected virtual void SendFrontHeadlightExecute(HeadlightValues parameter)
        {
            senderClient?.SendHeadlightFront(parameter);
        }

        protected virtual void SendWiperExecute(WiperSwitchValues parameter)
        {
            senderClient?.SendWiperSwitch(parameter);
        }

        protected virtual void SendGaugeLightSwitchExecute(bool parameter)
        {
            senderClient?.SendGaugeLightSwitch(parameter);
        }

        protected virtual void SendHornExecute(bool parameter)
        {
            senderClient?.SendHorn(parameter);
        }

        protected virtual async void SendHornSequenceExecute()
        {
            await senderClient!.SendHornSequencerAsync();
        }

        protected virtual void SendIndyBailOffExecute(bool parameter)
        {
            senderClient?.SendIndyBailOff(parameter);
        }

        protected virtual void SendRadioChannelModeExecute(bool parameter)
        {
            senderClient?.SendRadioChannelMode(parameter);
        }

        protected virtual void SendRadioDTMFModeExecute(bool parameter)
        {
            senderClient?.SendRadioDTMFMode(parameter);
        }

        protected virtual void SendRadioVolumeDecreaseExecute(bool parameter)
        {
            senderClient?.SendRadioVolumeDecrease(parameter);
        }

        protected virtual void SendRadioVolumeIncreaseExecute(bool parameter)
        {
            senderClient?.SendRadioVolumeIncrease(parameter);
        }

        protected virtual void SendRadioVolumeMuteExecute(bool parameter)
        {
            senderClient?.SendRadioVolumeMute(parameter);
        }

        // Todo: Params
        protected virtual void SendRearHeadlightExecute(HeadlightValues parameter)
        {
            senderClient?.SendHeadlightRear(parameter);
        }

        protected virtual void SendReleaseParkingExecute(bool parameter)
        {
            senderClient?.SendParkingBrakeRelease(parameter);
        }

        protected virtual void SendSetParkingExecute(bool parameter)
        {
            senderClient?.SendParkingBrakeSet(parameter);
        }

        protected virtual void SendSlowSpeedDecrementExecute(bool parameter)
        {
            senderClient?.SendSlowSpeedDecrement(parameter);
        }

        protected virtual void SendSlowSpeedIncrementExecute(bool parameter)
        {
            senderClient?.SendSlowSpeedIncrement(parameter);
        }

        protected virtual void SendSlowSpeedOnOffExecute(bool parameter)
        {
            senderClient?.SendSlowSpeedToggle(parameter);
        }

        protected virtual void SendStepLightSwitchExecute(bool parameter)
        {
            senderClient?.SendStepLightSwitch(parameter);
        }

        // Todo: Params
        protected virtual void ServiceSelectorExecute(ServiceSelectorSwitchValues parameter)
        {
            senderClient?.SendServiceSelectorSwitch(parameter);
        }

        protected virtual void DPUThrottleDecreaseExecute(bool parameter)
        {
            senderClient?.SendDPUThrottleDecrease(parameter);
        }

        protected virtual void DPUThrottleIncreaseExecute(bool parameter)
        {
            senderClient?.SendDPUThrottleIncrease(parameter);
        }

        // Todo: params
        protected virtual void TractionMotorsExecute(bool[] parameter)
        {
            TractionMotors motors = TractionMotors.flag;
            motors |= parameter[0] ? TractionMotors.tm1 : (TractionMotors)0;
            motors |= parameter[1] ? TractionMotors.tm2 : (TractionMotors)0;
            motors |= parameter[2] ? TractionMotors.tm3 : (TractionMotors)0;
            motors |= parameter[3] ? TractionMotors.tm4 : (TractionMotors)0;
            motors |= parameter[4] ? TractionMotors.tm5 : (TractionMotors)0;
            motors |= parameter[5] ? TractionMotors.tm6 : (TractionMotors)0;

            senderClient?.SendTractionMotors(motors);
        }

        // Todo: Params
        protected virtual void TrainBrakeCutOutExecute(TrainBrakeCutoutValues parameter)
        {
            senderClient?.SendTrainBrakeCutoutValve(parameter);
        }

        internal void SetEndPoint(UdpEndpoint? endPoint)
        {
            if(senderClient == null)
            {
                senderClient = new Run8SenderClient();
            }

            senderClient.Endpoint = endPoint;
        }
    }
}
