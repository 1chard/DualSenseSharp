using static DualSenseSharp.Components.AdaptiveTrigger;

namespace DualSenseSharp.Components.Triggers;

public class SimpleVibration : TriggerMode
{
    public override byte Mode => 0x06;
    public readonly byte Frequency;
    public readonly byte Force;
    public readonly byte Position;

    public SimpleVibration(byte frequency, byte force, byte position)
    {
        Frequency = frequency;
        Force = force;
        Position = position;
    }

    internal override byte[] CreateData()
    {
        return [Frequency, Force, Position, 0, 0, 0, 0, 0, 0, 0];
    }
}
