namespace DualSenseSharp.Input;

internal interface IInputParser
{
    internal byte[] GetData(byte[] buffer);
}
