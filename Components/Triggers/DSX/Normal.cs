using static DualSenseSharp.Components.AdaptiveTrigger;

namespace DualSenseSharp.Components.Triggers.DSX;

public class Normal : TriggerMode
{
    public override byte Mode => (byte) CustomTriggerValue.DSXTriggerMode.Rigid_B;

    internal override byte[] CreateData() => [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
}
