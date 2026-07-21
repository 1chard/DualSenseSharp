using DualSenseSharp.Components.Triggers;
using DualSenseSharp.Builder;
using DualSenseSharp.Utils;

namespace DualSenseSharp.Components;

public sealed class AdaptiveTrigger : Component
{
    private bool IsDirty;
    private readonly Side CurrentSide;
    private TriggerMode _mode = new Off();

    public TriggerMode Mode
    {
        get => _mode;
        set
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            _mode = value;
            IsDirty = true;
        }
    }

    public AdaptiveTrigger(Side side)
    {
        CurrentSide = side;
    }

    public enum Side
    {
        Left,
        Right
    }


    private const byte ValidFlag0_AdaptiveTrigger = 0xC;
    internal override void Write(byte[] buffer)
    {
        if (IsDirty)
        {
            buffer[OutputOffset.ValidFlag0] |= ValidFlag0_AdaptiveTrigger;
            var offset = CurrentSide == Side.Left ? OutputOffset.LeftTrigger : OutputOffset.RightTrigger;
            buffer[offset] = Mode.Mode;
            ByteArrayUtil.SetBytes(buffer, offset + 1, Mode.CreateData());
            IsDirty = false;
        }
    }

    public abstract class TriggerMode
    {
        public abstract byte Mode { get; }  
        internal abstract byte[] CreateData();
    }
}

