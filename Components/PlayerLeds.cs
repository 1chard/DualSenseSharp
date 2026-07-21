using DualSenseSharp.Builder;

namespace DualSenseSharp.Components;

public sealed class PlayerLeds : Component
{
    private bool _led1;
    private bool _led2;
    private bool _led3;
    private bool _led4;
    private bool _led5;
    private Brightness _brightness;
    internal bool IsDirty { get; set; }
    internal bool IsDirtyBrightness { get; set; }


    public bool Led1
    {
        get
        {
            return _led1;
        }
        set
        {
            _led1 = value;
            IsDirty = true;
        }
    }
    public bool Led2
    {
        get
        {
            return _led2;
        }
        set
        {
            _led2 = value;
            IsDirty = true;
        }
    }
    public bool Led3
    {
        get
        {
            return _led3;
        }
        set
        {
            _led3 = value;
            IsDirty = true;
        }
    }
    public bool Led4
    {
        get
        {
            return _led4;
        }
        set
        {
            _led4 = value;
            IsDirty = true;
        }
    }
    public bool Led5
    {
        get
        {
            return _led5;
        }
        set
        {
            _led5 = value;
            IsDirty = true;
        }
    }
    public Brightness Brightness
    {
        get
        {
            return _brightness;
        }
        set
        {
            _brightness = value;
            IsDirtyBrightness = true;
        }
    }

    private const int ValidFlag1_PlayerLeds = 1 << 4;
    private const int ValidFlag2_LedControl = 1 << 0;

    internal override void Write(byte[] buffer)
    {
        if (IsDirty)
        {
            buffer[OutputOffset.ValidFlag1] |= ValidFlag1_PlayerLeds;
            buffer[OutputOffset.PlayerLeds] = GetPlayerLedsByte(this);
            IsDirty = false;
        }
        if (IsDirtyBrightness)
        {
            buffer[OutputOffset.ValidFlag2] |= ValidFlag2_LedControl;
            buffer[OutputOffset.Brightness] = (byte)Brightness;
            IsDirtyBrightness = false;
        }
    }

    private static byte GetPlayerLedsByte(PlayerLeds playerLeds)
    {
        byte result = 0;
        if (playerLeds.Led1) result |= 1 << 0;
        if (playerLeds.Led2) result |= 1 << 1;
        if (playerLeds.Led3) result |= 1 << 2;
        if (playerLeds.Led4) result |= 1 << 3;
        if (playerLeds.Led5) result |= 1 << 4;
        return result;
    }


}