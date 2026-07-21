using DualSenseSharp.Builder;

namespace DualSenseSharp.Components;

public sealed class Microphone : Component
{
    private bool _ledOn;
    internal bool IsDirty { get; set; }

    public bool LedOn { 
        get { 
            return _ledOn;
        } 
        set {
            _ledOn = value;
            IsDirty = true;
        }
    }

    private const int ValidFlag1_MicrophoneLed = 1 << 0;
    internal override void Write(byte[] buffer)
    {
        if (IsDirty)
        {
            buffer[OutputOffset.ValidFlag1] |= ValidFlag1_MicrophoneLed;
            buffer[OutputOffset.MicrophoneLed] = (byte)(LedOn ? 1 : 0);
            IsDirty = false;
        }
    }
}