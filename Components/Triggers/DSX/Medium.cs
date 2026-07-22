using static DualSenseSharp.Components.AdaptiveTrigger;

namespace DualSenseSharp.Components.Triggers.DSX;

public class Medium : TriggerMode
{
    public override byte Mode => (byte) CustomTriggerValue.DSXTriggerMode.Rigid_A;

    internal override byte[] CreateData() => [2, 35, 1, 6, 6, 1, 33, 0, 0, 0];
}
