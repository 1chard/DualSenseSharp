using static DualSenseSharp.Components.AdaptiveTrigger;

namespace DualSenseSharp.Components.Triggers.DSX;

public class VerySoft : TriggerMode
{
    public override byte Mode => (byte) CustomTriggerValue.DSXTriggerMode.Pulse;

    internal override byte[] CreateData() => [128, 160, 255, 0, 0, 0, 0, 0, 0, 0];
}
