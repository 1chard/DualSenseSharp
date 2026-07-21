namespace DualSenseSharp.Input;

internal class BluetoothInputParser : IInputParser
{
    public byte[] GetData(byte[] buffer)
    {
        return buffer[2..64];
    }
}
