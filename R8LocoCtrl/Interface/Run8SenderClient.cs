//-----------------------------------------------------------------------
// <copyright file="Run8SenderClient.cs" company="Xcoder Software">
//     Author: Gil Yoder
//     Copyright (c) Xcoder Software. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using SimpleUdp;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace R8LocoCtrl.Interface
{
    public class Run8SenderClient(int sendPort = 18888) : INotifyPropertyChanged
    {
        private readonly byte[] dataBytes = new byte[5];
        private bool muteAudio;

        public event PropertyChangedEventHandler? PropertyChanged;
        public UdpEndpoint? Endpoint
        {
            get; set;
        }
        public bool MuteAudio
        {
            get => muteAudio;
            set
            {
                if (muteAudio == value)
                {
                    return;
                }

                muteAudio = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
        public void SendAlerter(bool buttonIsPressed)
        {
            SendMessage(1, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendAutoABTrain(bool buttonIsPressed)
        {
            SendMessage(48, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendAutoCBTrain(bool buttonIsPressed)
        {
            SendMessage(47, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendAutoEOTTrain(bool buttonIsPressed)
        {
            SendMessage(49, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendAutoMUTrain(bool buttonIsPressed)
        {
            SendMessage(46, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendAutoStartTrain(bool buttonIsPressed)
        {
            SendMessage(45, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendBell(bool buttonIsPressed)
        {
            SendMessage(2, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendCabLightSwitch(bool isOn)
        {
            SendMessage(41, (byte)(isOn ? 1 : 0));
        }
        public void SendCircuitBreaker(CircuitBreakers value, bool buttonIsPressed)
        {
            SendMessage((int)value, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendControlSwitch(bool isOn)
        {
            SendMessage(37, (byte)(isOn ? 1 : 0));
        }
        public void SendDistanceCounter(bool countUp, bool buttonIsPressed)
        {
            if (buttonIsPressed)
            {
                SendMessage(3, (byte)(countUp ? 1 : 2));
            }
            else
            {
                SendMessage(3, (byte)0);
            }
        }
        public void SendDPUFenceDecrease(bool buttonIsPressed)
        {
            SendMessage(62, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendDPUFenceIncrease(bool buttonIsPressed)
        {
            SendMessage(61, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendDPUThrottleDecrease(bool buttonIsPressed)
        {
            SendMessage(59, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendDPUThrottleIncrease(bool buttonIsPressed)
        {
            SendMessage(58, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendDTMFTone(DTMFToneValues value, bool buttonIsPressed)
        {
            SendMessage((int)value, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendDynBrakeLever(byte value)
        {
            SendMessage(4, value);
        }
        public void SendDynBrakeSetup(bool buttonIsPressed)
        {
            SendMessage(60, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendDynBrakeSwitch(bool isOn)
        {
            SendMessage(38, (byte)(isOn ? 1 : 0));
        }
        public void SendEngineStart(bool buttonIsPressed)
        {
            SendMessage(50, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendEngineStop(bool buttonIsPressed)
        {
            SendMessage(51, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendEngRunSwitch(bool isOn)
        {
            SendMessage(39, (byte)(isOn ? 1 : 0));
        }
        public void SendEOTEmgStop(bool buttonIsPressed)
        {
            SendMessage(44, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendGaugeLightSwitch(bool buttonIsPressed)
        {
            SendMessage(43, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendGenFieldSwitch(bool isOn)
        {
            SendMessage(40, (byte)(isOn ? 1 : 0));
        }
        public void SendHeadlightFront(HeadlightValues value)
        {
            SendMessage(5, (byte)value);
        }
        public void SendHeadlightRear(HeadlightValues value)
        {
            SendMessage(6, (byte)value);
        }
        public void SendHEPSwitch(bool isOn)
        {
            SendMessage(52, (byte)(isOn ? 1 : 0));
        }
        public void SendHorn(bool buttonIsPressed)
        {
            SendMessage(8, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendIndyBailOff(bool buttonIsPressed)
        {
            SendMessage(10, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendIndyBrakeLever(byte value)
        {
            SendMessage(9, value);
        }
        public void SendIsolationSwitch(IsolationSwitchValues value)
        {
            SendMessage(11, (byte)value);
        }
        public void SendLightSwitch(LightSwitches value, bool buttonIsPressed)
        {
            SendMessage((int)value, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendMessage(int message, byte data)
        {
            dataBytes[0] = (byte)(MuteAudio ? 96 : 224);
            dataBytes[1] = (byte)(message >> 8);
            dataBytes[2] = (byte)(message & 0xff);
            dataBytes[3] = data;
            dataBytes[4] = 0;
            for (int i = 0; i < dataBytes.Length-1; i++)
            {
                dataBytes[4] ^= dataBytes[i];
            }

            // Transmit dataBytes by UDP now.
            Endpoint?.Send("127.0.0.1", sendPort, dataBytes);
        }
        public void SendMUHLSwitch(MUHLSwitchValues value)
        {
            SendMessage(7, (byte)value);
        }
        public void SendParkingBrakeRelease(bool buttonIsPressed)
        {
            SendMessage(13, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendParkingBrakeSet(bool buttonIsPressed)
        {
            SendMessage(12, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendRadioChannelMode(bool buttonIsPressed)
        {
            SendMessage(35, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendRadioDTMFMode(bool buttonIsPressed)
        {
            SendMessage(36, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendRadioVolumeDecrease(bool buttonIsPressed)
        {
            SendMessage(33, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendRadioVolumeIncrease(bool buttonIsPressed)
        {
            SendMessage(32, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendRadioVolumeMute(bool buttonIsPressed)
        {
            SendMessage(34, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendReverserLever(ReverserPositions position)
        {
            switch (position)
            {
                case ReverserPositions.Neutral:
                    SendMessage(14, 128);
                    break;
                case ReverserPositions.Reverse:
                    SendMessage(14, 0);
                    break;
                case ReverserPositions.Forward:
                    SendMessage(14, 255);
                    break;
            }
        }
        public void SendSander(bool buttonIsPressed)
        {
            SendMessage(15, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendServiceSelectorSwitch(ServiceSelectorSwitchValues value)
        {
            SendMessage(54, (byte)value);
        }
        public void SendSlowSpeedDecrement(bool buttonIsPressed)
        {
            SendMessage(57, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendSlowSpeedIncrement(bool buttonIsPressed)
        {
            SendMessage(56, (byte)(buttonIsPressed ? 1 : 0));
        }
        public void SendSlowSpeedToggle(bool isOn)
        {
            SendMessage(55, (byte)(isOn ? 1 : 0));
        }
        public void SendStepLightSwitch(bool isOn)
        {
            SendMessage(42, (byte)(isOn ? 1 : 0));
        }
        /// <summary>
        /// Value should be from 0 to 8 inclusive.
        /// </summary>
        public void SendThrottleLever(byte value)
        {
            SendMessage(16, value);
        }
        public void SendTractionMotors(TractionMotors motors)
        {
            SendMessage(17, (byte)(motors | TractionMotors.flag));
        }
        public void SendTrainBrakeCutoutValve(TrainBrakeCutoutValues value)
        {
            SendMessage(53, (byte)value);
        }
        public void SendTrainBrakeLever(byte value)
        {
            SendMessage(18, value);
        }
        public void SendWiperSwitch(WiperSwitchValues value)
        {
            SendMessage(19, (byte)value);
        }

    }
}