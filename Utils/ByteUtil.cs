namespace DualSenseSharp.Utils;

internal class ByteUtil
{
    public static byte OffsetByte(int value, byte offset)
    {
        return (byte)((value >> offset) & 0xFF);
    }
}
