using static DualSenseSharp.Components.AdaptiveTrigger;

namespace DualSenseSharp.Components.Triggers;

public class Off : TriggerMode
{
    public override byte Mode => 0x00;

    internal override byte[] CreateData()
    {
        return [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
    }
}
