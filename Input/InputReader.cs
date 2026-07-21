using DualSenseSharp.Input.State;
using HidSharp;

namespace DualSenseSharp.Input;

public class InputReader
{
    internal InputReader() {
    
    }

    public ButtonsState Buttons { get; } = new ButtonsState();

    internal void UpdateState(byte[] data)
    {
        Buttons.UpdateState(data);
    }
}
