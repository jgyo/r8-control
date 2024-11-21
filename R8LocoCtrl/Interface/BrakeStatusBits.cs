namespace R8LocoCtrl.Interface
{
    [Flags]
    public enum BrakeStatusBits
    {
        PenaltyBrakeApplication = 1,
        DynamicBrakeMode = 2,
        AirCompressorOnOff = 4
    }
}
