using static DualSenseSharp.Components.AdaptiveTrigger;

namespace DualSenseSharp.Components.Triggers;

public class SimpleFeedback : TriggerMode
{
    public override byte Mode => 0x01;
    public readonly byte Position;
    public readonly byte Strength;

    public SimpleFeedback(byte position, byte strength)
    {
        Position = position;
        Strength = strength;
    }

    internal override byte[] CreateData()
    {
        return [Position, Strength, 0, 0, 0, 0, 0, 0, 0, 0];
    }
}
