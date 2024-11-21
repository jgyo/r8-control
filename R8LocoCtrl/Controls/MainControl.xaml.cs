//-----------------------------------------------------------------------
// <copyright file="MainControl.xaml.cs" company="Xcoder Software">
//     Author: Gil Yoder
//     Copyright (c) Xcoder Software. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using R8LocoCtrl.Interface;
using R8LocoCtrl.Tools;
using R8LocoCtrl.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace R8LocoCtrl.Controls
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {

        public static readonly DependencyProperty AlerterStatusProperty =
    DependencyProperty.Register("AlerterStatus", typeof(AlerterStatus), typeof(MainControl), new PropertyMetadata(AlerterStatus.OK));
        public static readonly DependencyProperty AlertRangeMaxProperty =
    DependencyProperty.Register("AlertRangeMax", typeof(int), typeof(MainControl ), new PropertyMetadata(100));
        public static readonly DependencyProperty AmmeterProperty =
    DependencyProperty.Register("Ammeter", typeof(int), typeof(MainControl), new PropertyMetadata(0));
        public static readonly DependencyProperty BcPsiProperty =
    DependencyProperty.Register("BcPsi", typeof(int), typeof(MainControl), new PropertyMetadata(0));
        public static readonly DependencyProperty BpPsiProperty =
    DependencyProperty.Register("BpPsi", typeof(int), typeof(MainControl), new PropertyMetadata(90));
        public static readonly DependencyProperty BrakeStatusProperty =
    DependencyProperty.Register("BrakeStatus", typeof(BrakeStatusBits), typeof(MainControl), new PropertyMetadata((BrakeStatusBits)0));
        public static readonly DependencyProperty CautionRangeMaxProperty =
    DependencyProperty.Register("CautionRangeMax", typeof(int), typeof(MainControl), new PropertyMetadata(70));
        public static readonly DependencyProperty CFMProperty =
    DependencyProperty.Register("CFM", typeof(int), typeof(MainControl), new PropertyMetadata(0));
        public static readonly DependencyProperty DieselRpmProperty =
    DependencyProperty.Register("DieselRpm", typeof(int), typeof(MainControl), new PropertyMetadata(0));
        // Using a DependencyProperty as the backing store for EotId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EotIdProperty =
    DependencyProperty.Register("EotId", typeof(int), typeof(MainControl), new PropertyMetadata(0));
        public static readonly DependencyProperty EotPsiProperty =
    DependencyProperty.Register("EotPsi", typeof(int), typeof(MainControl), new PropertyMetadata(0));
        // Using a DependencyProperty as the backing store for EotStatus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EotStatusProperty =
    DependencyProperty.Register("EotStatus", typeof(EotStatus), typeof(MainControl), new PropertyMetadata((EotStatus) 0));
        public static readonly DependencyProperty EqPsiProperty =
    DependencyProperty.Register("EqPsi", typeof(int), typeof(MainControl), new PropertyMetadata(90));
        // Using a DependencyProperty as the backing store for FuelLevel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FuelLevelProperty =
    DependencyProperty.Register("FuelLevel", typeof(int), typeof(MainControl), new PropertyMetadata(0));
        public static readonly DependencyProperty IsBellOnProperty =
    DependencyProperty.Register("IsBellOn", typeof(bool), typeof(MainControl), new PropertyMetadata(false));
        public static readonly DependencyProperty IsCompressorOnProperty =
    DependencyProperty.Register("IsCompressorOn", typeof(bool), typeof(MainControl), new PropertyMetadata(false));
        public static readonly DependencyProperty IsHornOnProperty =
    DependencyProperty.Register("IsHornOn", typeof(bool), typeof(MainControl), new PropertyMetadata(false));
        public static readonly DependencyProperty IsPcsOpenProperty =
    DependencyProperty.Register("IsPcsOpen", typeof(bool), typeof(MainControl), new PropertyMetadata(false));
        public static readonly DependencyProperty IsSandOnProperty =
    DependencyProperty.Register("IsSandOn", typeof(bool), typeof(MainControl), new PropertyMetadata(false));
        public static readonly DependencyProperty IsSlippingProperty =
    DependencyProperty.Register("IsSlipping", typeof(bool), typeof(MainControl), new PropertyMetadata(false));
        public static readonly DependencyProperty LocoStatusProperty =
    DependencyProperty.Register("LocoStatus", typeof(LocoStatusBits), typeof(MainControl), new PropertyMetadata((LocoStatusBits)0));
        public static readonly DependencyProperty MainPsiProperty =
    DependencyProperty.Register("MainPsi", typeof(int), typeof(MainControl), new PropertyMetadata(0));
        public static readonly DependencyProperty MaxSpeedValueProperty =
    DependencyProperty.Register("MaxSpeedValue", typeof(int), typeof(MainControl), new PropertyMetadata(100));
        public static readonly DependencyProperty NextMinSpeedProperty =
    DependencyProperty.Register("NextMinSpeed", typeof(double), typeof(MainControl), new PropertyMetadata(0.0, null, CoerceNextMin));
        public static readonly DependencyProperty NextSignalProperty =
    DependencyProperty.Register("NextSignal", typeof(SignalInstructions), typeof(MainControl), new PropertyMetadata(SignalInstructions.Unknown));
        public static readonly DependencyProperty ParkingBrakeAppliedProperty =
    DependencyProperty.Register("ParkingBrakeApplied", typeof(bool), typeof(MainControl), new PropertyMetadata(false));
        public static readonly DependencyProperty PressureReferenceProperty =
    DependencyProperty.Register("PressureReference", typeof(int), typeof(MainControl), new PropertyMetadata(0));
        public static readonly DependencyProperty PreviousSignalProperty =
    DependencyProperty.Register("PreviousSignal", typeof(SignalInstructions), typeof(MainControl), new PropertyMetadata(SignalInstructions.Unknown));
        public static readonly DependencyProperty ReverserPositionProperty =
    DependencyProperty.Register("ReverserPosition", typeof(ReverserPositions), typeof(MainControl), new PropertyMetadata(ReverserPositions.Neutral));
        public static readonly DependencyProperty SafeRangeMaxProperty =
    DependencyProperty.Register("SafeRangeMax", typeof(int), typeof(MainControl), new PropertyMetadata(60));
        public static readonly DependencyProperty SpeedMphPerMinProperty =
    DependencyProperty.Register("SpeedMphPerMin", typeof(double), typeof(MainControl), new PropertyMetadata(0.0));
        public static readonly DependencyProperty SpeedProperty =
    DependencyProperty.Register("Speed", typeof(int), typeof(MainControl), new PropertyMetadata(0));
        public static readonly DependencyProperty ThrottleProperty =
    DependencyProperty.Register("Throttle", typeof(int), typeof(MainControl), new PropertyMetadata(0));
        public static readonly DependencyProperty WaterTempProperty =
    DependencyProperty.Register("WaterTemp", typeof(int), typeof(MainControl), new PropertyMetadata(0));

        public MainControl()
        {
            InitializeComponent();

            DataContextChanged += MainControl_DataContextChanged;
            if (this.DataContext != null)
            {
                ((Run8ListenerClient)DataContext).StartListener();
                var listener = ((Run8ListenerClient)DataContext);
                var udpSender = this.FindResource("actionViewModel") as Run8ActionsViewModel;
                udpSender?.SetEndPoint(listener.EndPoint);
            }
        }

        public AlerterStatus AlerterStatus
        {
            get { return (AlerterStatus)GetValue(AlerterStatusProperty); }
            set { SetValue(AlerterStatusProperty, value); }
        }
        public int AlertRangeMax
        {
            get { return (int)GetValue(AlertRangeMaxProperty); }
            set { SetValue(AlertRangeMaxProperty, value); }
        }
        public int Ammeter
        {
            get { return (int)GetValue(AmmeterProperty); }
            set { SetValue(AmmeterProperty, value); }
        }
        public int BcPsi
        {
            get { return (int)GetValue(BcPsiProperty); }
            set { SetValue(BcPsiProperty, value); }
        }
        public int BpPsi
        {
            get { return (int)GetValue(BpPsiProperty); }
            set { SetValue(BpPsiProperty, value); }
        }
        public BrakeStatusBits BrakeStatus
        {
            get { return (BrakeStatusBits)GetValue(BrakeStatusProperty); }
            set { SetValue(BrakeStatusProperty, value); }
        }
        public int CautionRangeMax
        {
            get { return (int)GetValue(CautionRangeMaxProperty); }
            set { SetValue(CautionRangeMaxProperty, value); }
        }
        public int CFM
        {
            get { return (int)GetValue(CFMProperty); }
            set { SetValue(CFMProperty, value); }
        }
        public int DieselRpm
        {
            get { return (int)GetValue(DieselRpmProperty); }
            set { SetValue(DieselRpmProperty, value); }
        }
        public int EotId
        {
            get { return (int)GetValue(EotIdProperty); }
            set { SetValue(EotIdProperty, value); }
        }
        public int EotPsi
        {
            get { return (int)GetValue(EotPsiProperty); }
            set { SetValue(EotPsiProperty, value); }
        }
        public EotStatus EotStatus
        {
            get { return (EotStatus)GetValue(EotStatusProperty); }
            set { SetValue(EotStatusProperty, value); }
        }
        public int EqPsi
        {
            get { return (int)GetValue(EqPsiProperty); }
            set { SetValue(EqPsiProperty, value); }
        }
        public int FuelLevel
        {
            get { return (int)GetValue(FuelLevelProperty); }
            set { SetValue(FuelLevelProperty, value); }
        }
        public LocoStatusBits LocoStatus
        {
            get { return (LocoStatusBits)GetValue(LocoStatusProperty); }
            set { SetValue(LocoStatusProperty, value); }
        }
        public int MainPsi
        {
            get { return (int)GetValue(MainPsiProperty); }
            set { SetValue(MainPsiProperty, value); }
        }
        public int MaxSpeedValue
        {
            get { return (int)GetValue(MaxSpeedValueProperty); }
            set { SetValue(MaxSpeedValueProperty, value); }
        }
        public double NextMinSpeed
        {
            get { return (double)GetValue(NextMinSpeedProperty); }
            set { SetValue(NextMinSpeedProperty, value); }
        }
        public SignalInstructions NextSignal
        {
            get { return (SignalInstructions)GetValue(NextSignalProperty); }
            set { SetValue(NextSignalProperty, value); }
        }
        public int PressureReference
        {
            get { return (int)GetValue(PressureReferenceProperty); }
            set { SetValue(PressureReferenceProperty, value); }
        }
        public SignalInstructions PreviousSignal
        {
            get { return (SignalInstructions)GetValue(PreviousSignalProperty); }
            set { SetValue(PreviousSignalProperty, value); }
        }
        public ReverserPositions ReverserPosition
        {
            get { return (ReverserPositions)GetValue(ReverserPositionProperty); }
            set { SetValue(ReverserPositionProperty, value); }
        }
        public int SafeRangeMax
        {
            get { return (int)GetValue(SafeRangeMaxProperty); }
            set { SetValue(SafeRangeMaxProperty, value); }
        }
        public int Speed
        {
            get { return (int)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }
        public double SpeedMphPerMin
        {
            get { return (double)GetValue(SpeedMphPerMinProperty); }
            set { SetValue(SpeedMphPerMinProperty, value); }
        }
        public int Throttle
        {
            get { return (int)GetValue(ThrottleProperty); }
            set { SetValue(ThrottleProperty, value); }
        }
        public int WaterTemp
        {
            get { return (int)GetValue(WaterTempProperty); }
            set { SetValue(WaterTempProperty, value); }
        }

        private static object CoerceNextMin(DependencyObject d, object baseValue)
        {
            if (((double)baseValue) < 0.0)
                return 0.0;

            return baseValue;
        }
        private void MainControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                var listener = ((Run8ListenerClient)DataContext);
                var udpSender = this.FindResource("actionViewModel") as Run8ActionsViewModel;

                ((Run8ListenerClient)DataContext).StartListener();
                udpSender?.SetEndPoint(listener.EndPoint);
            }
        }

    }
}
