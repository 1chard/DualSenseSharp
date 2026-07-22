using static DualSenseSharp.Components.AdaptiveTrigger;

namespace DualSenseSharp.Components.Triggers.DSX;

public class VeryHard : TriggerMode
{
    public override byte Mode => (byte) CustomTriggerValue.DSXTriggerMode.Rigid_A;

    internal override byte[] CreateData() => [16, 160, 255, 255, 255, 255, 255, 0, 0, 0];
}
