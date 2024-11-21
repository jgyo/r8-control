namespace R8LocoCtrl.Interface
{
    [Flags]
    public enum EotStatus
    {
        EotDoesNotExist = 0,
        EotMove = 1,
        EotBeaconOn = 2,
        EotCommTest = 4,
        EotError = 8
    }
}
