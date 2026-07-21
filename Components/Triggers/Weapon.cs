using static DualSenseSharp.Components.AdaptiveTrigger;

namespace DualSenseSharp.Components.Triggers;

public class Weapon : TriggerMode
{
    public override byte Mode => 0x02;
    public readonly byte StartPosition;
    public readonly byte EndPosition;
    public readonly byte Strength;

    public Weapon(byte startPosition, byte endPosition, byte strength)
    {
        StartPosition = startPosition;
        EndPosition = endPosition;
        Strength = strength;
    }

    internal override byte[] CreateData()
    {
        return [StartPosition, EndPosition, Strength, 0, 0, 0, 0, 0, 0, 0];
    }
}
