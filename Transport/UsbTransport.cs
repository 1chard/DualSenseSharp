namespace DualSenseSharp.Transport;

using HidSharp;

internal sealed class UsbTransport : BaseTransport
{
    public override bool IsBluetooth => false;

    public UsbTransport(HidDevice device) : base(device)
    {
    }
}