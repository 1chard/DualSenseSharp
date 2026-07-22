using static DualSenseSharp.Components.AdaptiveTrigger;

namespace DualSenseSharp.Components.Triggers.DSX;

public class Soft : TriggerMode
{
    public override byte Mode => (byte) CustomTriggerValue.DSXTriggerMode.Rigid_A;

    internal override byte[] CreateData() => [69, 160, 255, 0, 0, 0, 0, 0, 0, 0];
}
