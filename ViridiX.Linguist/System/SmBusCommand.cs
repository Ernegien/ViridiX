namespace ViridiX.Linguist.System
{
    /// <summary>
    /// Command codes available for the SMBus.
    /// </summary>
    public enum SmBusCommand
    {
        FirmwareVersion = 0x1,
        Reset = 0x2,
        TrayState = 0x3,
        VideoMode = 0x4,
        FanOverride = 0x5,
        RequestFanSpeed = 0x6,
        LedOverride = 0x7,
        LedStates = 0x8,
        CpuTemperature = 0x9,
        AirTemperature = 0xA,
        AudioClamp = 0xB,
        DvdTrayOperation = 0xC,
        OsResume = 0xD,
        WriteErrorCode = 0xE,
        ReadErrorCode = 0xF,
        ReadFanSpeed = 0x10,
        InterruptReason = 0x11,
        WriteRamTestResults = 0x12,
        WriteRamType = 0x13,
        ReadRamTestResults = 0x14,
        ReadRamType = 0x15,
        LastRegisterWritten = 0x16,
        LastByteWritten = 0x17,
        SoftwareInterrupt = 0x18,
        OverrideResetOnTrayOpen = 0x19,
        OsReady = 0x1A,
        ScratchRegister = 0x1B,
        ChallengeValue0 = 0x1C,
        ChallengeValue1 = 0x1D,
        ChallengeValue2 = 0x1E,
        ChallengeValue3 = 0x1F,
        ChallengeResponse0 = 0x20,
        ChallengeResponse1 = 0x21
    }
}
