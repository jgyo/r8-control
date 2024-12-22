using HotKeyLibrary;
using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace R8LocoCtrl.Interface
{
    public class GeneralCommands
    {
        private const string COMMAND_NAMES =
            @"AlerterReset          , OemQuotes
            BellOnOff               , Decimal
            DistanceCounterDown     , RShift + Multiply
            DistanceCounterUp       , Multiply
            DynBrakeIncreaseSmall   , Shift + Up
            DynBrakeIncreaseLarge   , Up
            DynBrakeDecreaseSmall   , Shift + Down
            DynBrakeDecreaseLarge   , Down
            HeadlightFrontSwitchDown, LShift + H
            HeadlightFrontSwitchUp  , H
            HeadlightRearSwitchDown , LShift + J
            HeadlightRearSwitchUp   , J
            MuHLSwitch
            Horn                    , Space
            HornSequencer           , Alt + Space
            IndyBrakeIncreaseSmall  , Shift + PageUp
            IndyBrakeIncreaseLarge  , PageUp
            IndyBrakeDecreaseSmall  , Shift + PageDown
            IndyBrakeDecreaseLarge  , Next
            IndyBrakeOff            , RCtrl + Next
            IndyBrakeFull           , RCtrl + PageUp
            IndyBrakeBail           , OemPeriod
            IsolationSwitch
            ParkingBrakeSet         , LShift + F5
            ParkingBrakeReset       , F5
            ReverserLeverUp         , Insert
            ReverserLeverDown       , Delete
            SanderToggle            , NumPad0
            ThrottleIncrease        , Add
            ThrottleDecrease        , Subtract
            TrainBrakeApplySmall    , Shift + Home
            TrainBrakeApplyLarge    , Home
            TrainBrakeReleaseSmall  , Shift + End
            TrainBrakeReleaseLarge  , End
            TrainBrakeSuppression   , RCtrl + Home
            TrainBrakeReleaseFull   , RCtrl + End
            WiperControl            , V
            DTMF0                   , Shift + NumPad0
            DTMF1                   , Shift + NumPad1
            DTMF2                   , Shift + NumPad2
            DTMF3                   , Shift + NumPad3
            DTMF4                   , Shift + NumPad4
            DTMF5                   , Shift + NumPad5
            DTMF6                   , Shift + NumPad6
            DTMF7                   , Shift + NumPad7
            DTMF8                   , Shift + NumPad8
            DTMF9                   , Shift + NumPad9
            DTMFPound               , Shift + Divide    , Shift + D3
            DTMFStar                , Shift + Multiply  , Shift + D8
            RadioVolIncrease        , Ctrl + PageUp
            RadioVolDecrease        , Ctrl + Next
            RadioVolMute            , Ctrl + End
            RadioChannelMode        , Ctrl + Insert
            RadioDTMFMode           , Ctrl + Delete
            CB_Control              , LAlt + D3
            CB_DynBrake             , LAlt + D4
            CB_EngRun               , LAlt + D1
            CB_GenField             , LAlt + D2
            CabLightSwitch          , L
            StepLightSwitch         , RShift + L
            GaugeLightSwitch        , RAlt + L
            EOTEmgStop              , LShift + LCtrl + Back
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
            SlowSpeedToggle         , LCtrl + C
            SlowSpeedIncrease
            SlowSpeedDecrease
            DPUThrottleIncrease     , LShift + OemQuotes
            DPUThrottleDecrease     , LShift + Oem1
            DPUDynBrakeSetup        , LShift + Divide
            DPUFenceIncrease        , LShift + Oem6
            DPUFenceDecrease        , LShift + OemOpenBrackets
            AboutWindow             , Win + A
            Driver1Window           , Win + D1
            Driver2Window           , Win + D2
            AutoWindow              , Win + D3
            StartUpWindow           , Win + D4
            DPUWindow               , Win + D5
            RadioWindow             , Win + D6
            LightsWindow            , Win + D7
            HotKeyWindow            , Win + D8
            SetupWindow             , Win + D9";
        private const string KEY_BINDINGS_FILENAME = "KeyBindings.xml";

        public static readonly List<NamedCommandKeys> CurrentCommands = [];
        public static readonly List<NamedCommandKeys> DefaultCommands = [];

        static GeneralCommands()
        {
            var commandNames = COMMAND_NAMES.Split(
                '\n',
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            foreach(var commandName in commandNames)
            {
                var parts = commandName.Split(',', StringSplitOptions.TrimEntries);
                var namedKey = new NamedCommandKeys(parts[0]);
                if(parts.Length > 1)
                    namedKey.Key = new HotKey(parts[1]);
                if(parts.Length > 2)
                    namedKey.AltKey = new HotKey(parts[2]);
                DefaultCommands.Add(namedKey);
            }

            if(File.Exists(KEY_BINDINGS_FILENAME))
            {
                CurrentCommands = ReadKeyBindingFile();
            }
            else
            {
                foreach(var commandName in DefaultCommands)
                {
                    CurrentCommands.Add(new NamedCommandKeys(commandName));
                }
            }
        }

        private static List<NamedCommandKeys> ReadKeyBindingFile()
        {
            var list = new List<NamedCommandKeys>();

            var reader = new XmlTextReader(KEY_BINDINGS_FILENAME);
            reader.Read(); // move past the root node
            while(reader.Read())
            {
                reader.MoveToElement();
                var name = reader.GetAttribute("name");
                var key = reader.GetAttribute("key");
                var altKey = reader.GetAttribute("AltKey");

                list.Add(
                    new NamedCommandKeys(name!)
                    {
                        Key = string.IsNullOrEmpty(key) ? null : new HotKey(key),
                        AltKey = string.IsNullOrEmpty(altKey) ? null : new HotKey(altKey)
                    });
            }

            return list;
        }

        private static void WriteKeyBindingFile(List<NamedCommandKeys> keys)
        {
            var writer = new XmlTextWriter(KEY_BINDINGS_FILENAME, null);
            writer.WriteStartElement("commands");
            foreach(var key in keys)
            {
                writer.WriteStartElement("Command");
                writer.WriteAttributeString("name", key.Name);
                writer.WriteAttributeString("key", key.Key?.ToString());
                writer.WriteAttributeString("altKey", key.AltKey?.ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
        }

        public static void Save()
        {
            WriteKeyBindingFile(CurrentCommands);
        }
    }
}
