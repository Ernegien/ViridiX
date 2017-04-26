namespace ViridiX.Linguist.System
{
    /// <summary>
    /// Represents one of the 4 possible Xbox LED states.
    /// </summary>
    public enum LedState : byte
    {
        Off = 0,
        Red = 0x80,
        Green = 8,
        Orange = 0x88
    }
}
