namespace DualSenseSharp.Input;

internal class UsbInputParser : IInputParser
{
    public byte[] GetData(byte[] buffer)
    {
        return buffer[1..63];
    }
}
