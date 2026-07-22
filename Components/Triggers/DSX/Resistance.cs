using static DualSenseSharp.Components.AdaptiveTrigger;
using static DualSenseSharp.Utils.ByteUtil;

namespace DualSenseSharp.Components.Triggers.DSX;

public class Resistance : TriggerMode
{
    public override byte Mode => 0x21;
    public readonly byte Start;
    public readonly byte Force;

    public Resistance(byte start, byte force)
    {
        if (start > 9 || start < 0)
            throw new ArgumentOutOfRangeException("'start' is out of range (0-9)");
        if (force > 8 || force < 0)
            throw new ArgumentOutOfRangeException("'force' is out of range (0-8)");
        Start = start;
        Force = force;
    }

    internal override byte[] CreateData()
    {
        byte b = (byte)((Force - 1) & 7);
        uint num = 0;
        ushort num2 = 0;
        for (int i = (int)(Start); i < 10; ++i)
        {
            num |= ((uint)b) << (3 * i);
            num2 |= (ushort)(1 << i);
        }
        return [OffsetByte(num2, 0), OffsetByte(num2, 8), OffsetByte(num, 0), OffsetByte(num, 8), OffsetByte(num, 16), OffsetByte(num, 24), 0, 0, 0, 0];
    }
}
