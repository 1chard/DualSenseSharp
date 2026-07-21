namespace DualSenseSharp.Input.State;

public class ButtonsState : InputState
{
    public bool Cross { get; private set; }
    public bool Circle { get; private set; }
    public bool Square { get; private set; }
    public bool Triangle { get; private set; }


    internal override void UpdateState(byte[] data)
    {
        Cross = (data[7] & 0x20) != 0;
        Square = (data[7] & 0x10) != 0;
        Circle = (data[7] & 0x40) != 0;
        Triangle = (data[7] & 0x80) != 0;
    }
}
