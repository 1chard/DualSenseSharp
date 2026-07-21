namespace DualSenseSharp;

public readonly struct UniqueId
{
    private readonly ulong _value;

    public UniqueId(ReadOnlySpan<byte> bytes)
    {
        _value =
            ((ulong)bytes[5] << 40) |
            ((ulong)bytes[4] << 32) |
            ((ulong)bytes[3] << 24) |
            ((ulong)bytes[2] << 16) |
            ((ulong)bytes[1] << 8) |
             (ulong)bytes[0];
    }

    public override string ToString()
    {
        return $"{(_value >> 40) & 0xFF:X2}:{(_value >> 32) & 0xFF:X2}:{(_value >> 24) & 0xFF:X2}:{(_value >> 16) & 0xFF:X2}:{(_value >> 8) & 0xFF:X2}:{_value & 0xFF:X2}";
    }
}
