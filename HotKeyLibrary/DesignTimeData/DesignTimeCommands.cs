using System;
using System.Linq;
using System.Windows.Input;

namespace HotKeyLibrary.DesignTimeData
{
    public class DesignTimeCommands
    {
        private const string COMMAND_NAMES = @"AlerterReset
    Bell
    DistanceCounterDown
    DistanceCounterUp
    DynBrakeIncreaseSmall
    DynBrakeIncreaseLarge
    DynBrakeDecreaseSmall
    DynBrakeDecreaseLarge
    HeadLightFront
    HeadLightRear
    MuHLSwitch
    Horn
    IndyBrakeIncreaseSmall
    IndyBrakeIncreaseLarge
    IndyBrakeDecreaseSmall
    IndyBrakeDecreaseLarge
    IndyBailOff
    IsolationSwitch
    ParkingBrakeSet
    ParkingBrakeReset
    ReverserLeverUp
    ReverserLeverDown
    SanderToggle
    ThrottleIncrease
    ThrottleDecrease
    TrainBrakeIncreaseSmall
    TrainBrakeIncreaseLarge
    TrainBrakeDecreaseSmall
    TrainBrakeDecreaseLarge
    TrainBrakeFullEmg
    WiperSwitch
    DTMF0
    DTMF1
    DTMF2
    DTMF3
    DTMF4
    DTMF5
    DTMF6
    DTMF7
    DTMF8
    DTMF9
    DTMFPound
    DTMFStart
    RadioVolIncrease
    RadioVolDecrease
    RadioVolMute
    RadioChannelMode
    RadioDTMFMode
    CBControl
    CBDynBrake
    CBEngRun
    CBGenField
    CabLightSwitch
    StepLightSwitch
    GaugeLightSwitch
    EOTEmgStop
    AutoStartTrain
    AutoMUTrain
    AutoCBTrain
    AutoABTrain
    AutoEOTTrain
    EngineStart
    EngineStop
    HEPSwitch
    TrainBrakeCutout
    ServiceSelector
    SlowSpeedToggle
    SlowSpeedIncrease
    SlowSpeedDecrease
    DPUThrottleIncrease
    DPUThrottleDecrease
    DPUDynBrakeSetup
    DPUFenceIncrease
    DPUFenceDecrease";

        private readonly List<Key> keys =
        [
            Key.A,
            Key.B,
            Key.C,
            Key.D,
            Key.E,
            Key.F,
            Key.G,
            Key.H,
            Key.I,
            Key.J,
            Key.K,
            Key.L,
            Key.M,
            Key.N,
            Key.O,
            Key.P,
            Key.Q,
            Key.R,
            Key.S,
            Key.T,
            Key.U,
            Key.V,
            Key.W,
            Key.X,
            Key.Y,
            Key.Z,
            Key.F1,
            Key.F2,
            Key.F3,
            Key.F4,
            Key.F5,
            Key.F6,
            Key.F7,
            Key.F8,
            Key.F9,
            Key.F10,
            Key.F11,
            Key.F12,
            Key.F13,
            Key.F14,
            Key.F15,
            Key.F16,
            Key.F17,
            Key.F18,
            Key.F19,
            Key.F20,
            Key.F21,
            Key.F22,
            Key.F23,
            Key.F24,
            Key.NumPad0,
            Key.NumPad1,
            Key.NumPad2,
            Key.NumPad3,
            Key.NumPad4,
            Key.NumPad5,
            Key.NumPad6,
            Key.NumPad7,
            Key.NumPad8,
            Key.NumPad9,
            Key.Multiply,
            Key.Add,
            Key.Divide
        ];
        private readonly List<Modifiers> modifiers =
        [
            Modifiers.None,
            Modifiers.Alt,
            Modifiers.Ctrl,
            Modifiers.Shift,
            Modifiers.Win,
            Modifiers.LeftAlt,
            Modifiers.LeftCtrl,
            Modifiers.LeftWin,
            Modifiers.LeftShift,
            Modifiers.RightAlt,
            Modifiers.RightCtrl,
            Modifiers.RightWin,
            Modifiers.RightShift
        ];

        public DesignTimeCommands()
        {
            var commandNames = COMMAND_NAMES.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            CommandNames = [];
            var rand = new Random();

            foreach(var commandName in commandNames)
            {
                var key = keys[rand.Next(keys.Count - 1)];
                var key1 = keys[rand.Next(keys.Count - 1)];
                var mods = modifiers[rand.Next(modifiers.Count - 1)];
                var mods1 = modifiers[rand.Next(modifiers.Count - 1)];

                var c = new NamedCommandKeys(commandName)
                {
                    Key = new HotKey(key, mods),
                    AltKey = new HotKey(key1, mods1)
                };

                CommandNames.Add(c);
            }
        }

        public List<NamedCommandKeys> CommandNames { get; private set; }
    }
}
