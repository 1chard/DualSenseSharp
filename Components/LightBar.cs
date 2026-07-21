using DualSenseSharp.Builder;

namespace DualSenseSharp.Components;

public sealed class LightBar : Component
{
    private byte _red;
    private byte _green;
    private byte _blue;
    internal bool IsDirty { get; set; }

    public byte Red
    {
        get
        {
            return _red;
        }
        set
        {
            _red = value;
            IsDirty = true;
        }
    }

    public byte Green
    {
        get
        {
            return _green;
        }
        set
        {
            _green = value;
            IsDirty = true;
        }
    }

    public byte Blue
    {
        get
        {
            return _blue;
        }
        set
        {
            _blue = value;
            IsDirty = true;
        }
    }

    private const int ValidFlag1_LightBar = 1 << 2;
    internal override void Write(byte[] buffer)
    {
        if (IsDirty)
        {
            buffer[OutputOffset.ValidFlag1] |= ValidFlag1_LightBar;
            buffer[OutputOffset.LightBarRed] = Red;
            buffer[OutputOffset.LightBarGreen] = Green;
            buffer[OutputOffset.LightBarBlue] = Blue;
            IsDirty = false;
        }
    }
}