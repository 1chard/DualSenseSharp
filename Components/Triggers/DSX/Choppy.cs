using static DualSenseSharp.Components.AdaptiveTrigger;

namespace DualSenseSharp.Components.Triggers.DSX;

public class Choppy : TriggerMode
{
    public override byte Mode => (byte) CustomTriggerValue.DSXTriggerMode.Rigid_A;

    internal override byte[] CreateData() => [2, 39, 33, 39, 38, 2, 0, 0, 0, 0];
}
