namespace R8LocoCtrl.Interface
{
    public enum SignalInstructions
    {
        Clear = 0,
        MediumClear = 1,
        LimitedClear = 2,
        SlowClear = 3,
        Approach = 4,
        ApproachLimited = 5,
        ApproachMedium = 6,
        ApproachMediumDoubleYellow = 7,
        ApproachSlow = 8,
        ApproachRestricted = 9,
        ApproachThirty = 10,
        ApproachDiverging = 11,
        LimitedApproach = 12,
        MediumApproach = 13,
        SlowApproach = 14,
        MediumApproachMedium = 15,
        MediumApproachSlow = 16,
        AdvanceApproach = 17,
        MediumAdvanceApproach = 18,
        SlowApproachSlow = 19,
        Restricting = 20,
        Stop = 21,
        StopAndProceed = 22,
        DivergingClear = 23,
        DivergingClear3rdAhead = 24,
        DivergingApproachDiverging = 25,
        DivergingApproachMedium = 26,
        DivergingApproach = 27,
        DivergingApproach3rdAhead = 28,
        DivergingRestricting = 29,
        DivergingApproachRestricting = 30,
        DraggingEquipment = 31,
        Unknown = 32
    }
}
