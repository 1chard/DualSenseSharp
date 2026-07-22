namespace DualSenseSharp.Utils;

internal class ByteUtil
{
    public static byte OffsetByte(ulong value, byte offset)
    {
        return (byte)((value >> offset) & 0xFF);
    }
}
