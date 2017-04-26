namespace ViridiX.Linguist.System
{
    /// <summary>
    /// Devices on the System Management Controller.
    /// </summary>
    public enum SmcDevices
    {
        SmBus = 0x20,
        VideoEncoderConnexant = 0x8a,
        VideoEncoderFocus = 0xd4,
        VideoEncoderXcalibur = 0xe0,
        TempMonitor = 0x98,
        Eeprom = 0xA8
    }
}
