using DualSenseSharp.Builder;

namespace DualSenseSharp.Components;

public sealed class Rumble : Component
{
    private byte _weakMotor;
    private byte _strongMotor;
    internal bool IsDirty { get; set; }

    public byte WeakMotor
    {
        get
        {
            return _weakMotor;
        }
        set
        {
            _weakMotor = value;
            IsDirty = true;
        }
    }

    public byte StrongMotor
    {
        get
        {
            return _strongMotor;
        }
        set
        {
            _strongMotor = value;
            IsDirty = true;
        }
    }

    private const byte ValidFlag0_Rumble = 0x3;
    internal override void Write(byte[] buffer)
    {
        if (IsDirty)
        {
            buffer[OutputOffset.ValidFlag0] |= ValidFlag0_Rumble;
            buffer[OutputOffset.WeakMotor] = WeakMotor;
            buffer[OutputOffset.StrongMotor] = StrongMotor;
            IsDirty = false;
        }
    }
}