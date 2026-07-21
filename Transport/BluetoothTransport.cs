namespace DualSenseSharp.Transport;

using HidSharp;

internal sealed class BluetoothTransport : BaseTransport
{
    public override bool IsBluetooth => true;

    public BluetoothTransport(HidDevice device) : base(device)
    {
    }
}