namespace R8LocoCtrl.Interface
{
    [Flags]
    public enum LocoStatusBits
    {
        None = 0,
        ParkingBrake = 1,
        WheelSlip = 2,
        PcsOpen = 4,
        Sander = 8,
        AlerterPreWarning = 16,
        AlerterWarning = 32,
        HornOn = 64,
        BellOn = 128
    }
}
