//-----------------------------------------------------------------------
// <copyright file="UDPlistener.cs" company="Xcoder Software">
//     Author: Gil Yoder
//     Copyright (c) Xcoder Software. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using SimpleUdp;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace R8LocoCtrl.Interface
{
    public class Run8ListenerClient : INotifyPropertyChanged, IDisposable
    {

        private bool _disposed;
        private double accelerationMphMin;
        private byte airflowCfm;
        private int ammeter;
        private byte brakeCylinderPsi;
        private int brakePipe1Psi;
        private int brakePipe2Psi;
        private bool connected;
        private int dieselRpm;
        private byte eotBrakePipe;
        private int eotIdCode;
        private EotStatus eotStatusBits;
        private int eqReservoirPsi;
        private string errorMessage = string.Empty;
        private int fuelLevel;
        private bool hasError;
        private bool isMessageNew = false;
        private UdpEndpoint? listener;
        private int listenPort = 18889;
        private LocoStatusBits locoStatusBits1;
        private BrakeStatusBits locoStatusBits2;
        private int mainReservoirPsi;
        private SignalInstructions nextSignalInstruction;
        private SignalInstructions previousSignalInstruction;
        private ReverserPositions reverserPosition;
        private double speedMph;
        private byte throttleNotch;
        private int waterTemp;

        public Run8ListenerClient() : this (18889) {}

        public Run8ListenerClient(int listenerPort)
        {
            listenPort = listenerPort;
            var timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            DateAndTime = DateTime.Now;
            Connected = lastDatagram > DateAndTime.Subtract(TimeSpan.FromSeconds(5));
        }

        private DateTime dateAndTime = DateTime.MinValue;
        public DateTime DateAndTime
        {
            get => dateAndTime;
            set
            {
                if (dateAndTime == value)
                {
                    return;
                }

                dateAndTime = value;
                OnPropertyChanged();
            }
        }

        private DateTime lastDatagram;

        public event PropertyChangedEventHandler? PropertyChanged;

        public double AccelerationMphMin
        {
            get => accelerationMphMin;
            private set
            {
                if (accelerationMphMin == value)
                {
                    return;
                }

                accelerationMphMin = value;
                OnPropertyChanged();
            }
        }
        public byte AirflowCfm
        {
            get => airflowCfm;
            private set
            {
                if (airflowCfm == value)
                {
                    return;
                }

                airflowCfm = value;
                OnPropertyChanged();
            }
        }
        public int Ammeter
        {
            get => ammeter;
            private set
            {
                if (ammeter == value)
                {
                    return;
                }

                ammeter = value;
                OnPropertyChanged();
            }
        }
        public byte BrakeCylinderPsi
        {
            get => brakeCylinderPsi;
            private set
            {
                if (brakeCylinderPsi == value)
                {
                    return;
                }

                brakeCylinderPsi = value;
                OnPropertyChanged();
            }
        }
        public int BrakePipe1Psi
        {
            get => brakePipe1Psi;
            private set
            {
                if (brakePipe1Psi == value)
                {
                    return;
                }

                brakePipe1Psi = value;
                OnPropertyChanged();
            }
        }
        public int BrakePipe2Psi
        {
            get => brakePipe2Psi;
            private set
            {
                if (brakePipe2Psi == value)
                {
                    return;
                }

                brakePipe2Psi = value;
                OnPropertyChanged();
            }
        }
        public BrakeStatusBits BrakeStatus
        {
            get => locoStatusBits2;
            private set
            {
                if (locoStatusBits2 == value)
                {
                    return;
                }

                locoStatusBits2 = value;
                OnPropertyChanged();
            }
        }

        public bool Connected
        {
            get => connected;
            set
            {
                if (connected == value)
                {
                    return;
                }

                connected = value;
                OnPropertyChanged();
            }
        }
        public int DieselRpm
        {
            get => dieselRpm;
            private set
            {
                if (dieselRpm == value)
                {
                    return;
                }

                dieselRpm = value;
                OnPropertyChanged();
            }
        }
        public byte EotBrakePipe
        {
            get => eotBrakePipe;
            private set
            {
                if (eotBrakePipe == value)
                {
                    return;
                }

                eotBrakePipe = value;
                OnPropertyChanged();
            }
        }
        public int EotIdCode
        {
            get => eotIdCode;
            private set
            {
                if (eotIdCode == value)
                {
                    return;
                }

                eotIdCode = value;
                OnPropertyChanged();
            }
        }
        public EotStatus EotStatusBits
        {
            get => eotStatusBits;
            private set
            {
                if (eotStatusBits == value)
                {
                    return;
                }

                eotStatusBits = value;
                OnPropertyChanged();
            }
        }
        public int EqReservoirPsi
        {
            get => eqReservoirPsi;
            private set
            {
                if (eqReservoirPsi == value)
                {
                    return;
                }

                eqReservoirPsi = value;
                OnPropertyChanged();
            }
        }
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                if (errorMessage == value)
                {
                    return;
                }

                errorMessage = value;
                OnPropertyChanged();
            }
        }

        public int FuelLevel
        {
            get => fuelLevel;
            private set
            {
                if (fuelLevel == value)
                {
                    return;
                }

                fuelLevel = value;
                OnPropertyChanged();
            }
        }
        public bool HasError
        {
            get => hasError;
            set
            {
                if (hasError == value)
                {
                    return;
                }

                hasError = value;
                OnPropertyChanged();
            }
        }
        public bool IsMessageNew
        {
            get => isMessageNew;
            set
            {
                if (isMessageNew == value)
                {
                    return;
                }

                isMessageNew = value;
                OnPropertyChanged();
            }
        }
        public LocoStatusBits LocoStatus
        {
            get => locoStatusBits1;
            private set
            {
                if (locoStatusBits1 == value)
                {
                    return;
                }

                locoStatusBits1 = value;
                OnPropertyChanged();
            }
        }
        public int MainReservoirPsi
        {
            get => mainReservoirPsi;
            private set
            {
                if (mainReservoirPsi == value)
                {
                    return;
                }

                mainReservoirPsi = value;
                OnPropertyChanged();
            }
        }
        public SignalInstructions NextSignalInstruction
        {
            get => nextSignalInstruction;
            private set
            {
                if (nextSignalInstruction == value)
                {
                    return;
                }

                nextSignalInstruction = value;
                OnPropertyChanged();
            }
        }
        public SignalInstructions PreviousSignalInstruction
        {
            get => previousSignalInstruction;
            private set
            {
                if (previousSignalInstruction == value)
                {
                    return;
                }

                previousSignalInstruction = value;
                OnPropertyChanged();
            }
        }
        public ReverserPositions ReverserPosition
        {
            get => reverserPosition;
            private set
            {
                if (reverserPosition == value)
                {
                    return;
                }

                reverserPosition = value;
                OnPropertyChanged();
            }
        }
        public double SpeedMph
        {
            get => speedMph;
            set
            {
                if (speedMph == value)
                {
                    return;
                }

                speedMph = value;
                OnPropertyChanged();
            }
        }
        public byte ThrottleNotch
        {
            get => throttleNotch;
            private set
            {
                if (throttleNotch == value)
                {
                    return;
                }

                throttleNotch = value;
                OnPropertyChanged();
            }
        }
        public int WaterTemp
        {
            get => waterTemp;
            private set
            {
                if (waterTemp == value)
                {
                    return;
                }

                waterTemp = value;
                OnPropertyChanged();
            }
        }

        public UdpEndpoint? EndPoint => listener;

        private void Listener_DatagramReceived(object? sender, Datagram e)
        {
            lastDatagram = DateTime.Now;

            var bytes = e.Data;
            var crc = bytes[0];
            if (crc != 97)
            {
                // Wrong header
                HasError = true;
                ErrorMessage = $"Wrong header value: {crc}";
                return;
            }
            for (int i = 1; i < bytes.Length - 1; i++)
            {
                crc ^= bytes[i];
            }
            if (crc != bytes[bytes.Length - 1])
            {
                // Bad CRC
                HasError = true;
                ErrorMessage = "CRC error detected.";
                return;
            }

            HasError = false;

            // Speed
            double speed = 0;
            if (bytes[1] >= 128)
            {
                speed = (bytes[1] - 128) / 10d;
            }
            else
            {
                speed = bytes[1];
            }

            SpeedMph = speed;

            // Main psi
            MainReservoirPsi = bytes[2];

            // ER psi
            EqReservoirPsi = bytes[3];

            // BP psi
            BrakePipe1Psi = bytes[4];
            BrakePipe2Psi = bytes[5];

            // BC psi
            BrakeCylinderPsi = bytes[6];

            // CFM

            AirflowCfm = bytes[7];

            // Throttle notch

            ThrottleNotch = bytes[8];

            // Rear psi
            EotBrakePipe = bytes[9];

            // EOT Status
            if (bytes[10] == 255)
            {
                EotStatusBits = EotStatus.EotDoesNotExist;
            }
            else
            {
                EotStatusBits = (EotStatus)bytes[10];
            }

            // EOT ID Code
            EotIdCode = bytes[11];
            EotIdCode |= bytes[12] << 8;
            EotIdCode |= bytes[13] << 16;
            EotIdCode |= bytes[14] << 24;

            // Loco Status
            LocoStatus = (LocoStatusBits)bytes[15];


            // Ammeter (Usually amps / 10)
            Ammeter = bytes[16] * 10;

            // Last and next signals
            PreviousSignalInstruction = (SignalInstructions)bytes[17];
            NextSignalInstruction = (SignalInstructions)bytes[18];

            //Acceleration in mph/min from +-100 mph/min
            AccelerationMphMin = ((bytes[19] / 255d) * 2d - 1 + 0.004) * 100;

            // Reverser Position
            ReverserPosition = bytes[20] == 0
                ? ReverserPositions.Reverse
                : bytes[20] == 255 ? ReverserPositions.Forward : ReverserPositions.Neutral;

            // Water Temp F
            WaterTemp = bytes[21];

            // Diesel RPM
            DieselRpm = bytes[22] * 10;

            BrakeStatus = (BrakeStatusBits)bytes[23];

            // Fuel Level (0 empty to 255 full)
            FuelLevel = bytes[24];
        }

        private void Listener_EndpointDetected(object? sender, EndpointMetadata e)
        { Connected = true; }

        private void Listener_ServerStopped(object? sender, EventArgs e) { Connected = false; }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                listener?.Dispose();
                listener = null;
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void StartListener()
        {
            listener = new UdpEndpoint("127.0.0.1", listenPort);
            listener.DatagramReceived += Listener_DatagramReceived;
            listener.EndpointDetected += Listener_EndpointDetected;
            listener.ServerStopped += Listener_ServerStopped;
        }

    }
}
