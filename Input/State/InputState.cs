namespace DualSenseSharp.Input.State;

public abstract class InputState{
    internal abstract void UpdateState(byte[] data);
}
