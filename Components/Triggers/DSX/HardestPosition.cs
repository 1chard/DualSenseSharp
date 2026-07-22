using static DualSenseSharp.Components.AdaptiveTrigger;

namespace DualSenseSharp.Components.Triggers.DSX;

public class HardestPosition : TriggerMode
{
    public override byte Mode => (byte) CustomTriggerValue.DSXTriggerMode.Pulse;
    public readonly byte Position;
    
    public HardestPosition(byte position)
    {
        Position = position;
    }

    internal override byte[] CreateData() => [Position, 255, 255, 255, 255, 255, 255, 0, 0, 0];
}
