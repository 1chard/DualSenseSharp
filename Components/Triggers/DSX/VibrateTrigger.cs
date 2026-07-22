using static DualSenseSharp.Components.AdaptiveTrigger;

namespace DualSenseSharp.Components.Triggers.DSX;

public class VibrateTrigger : TriggerMode
{
    public override byte Mode => (byte) CustomTriggerValue.DSXTriggerMode.Pulse_AB;

    internal override byte[] CreateData() => [37, 35, 6, 39, 33, 35, 34, 0, 0, 0];
}
