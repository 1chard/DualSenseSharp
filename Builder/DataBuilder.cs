namespace DualSenseSharp.Builder;

internal class DataBuilder
{
    private readonly List<DualSenseSharp.Components.Component> _componentsWriters = new List<DualSenseSharp.Components.Component>();
    internal DataBuilder(DualSense dualSense)
    {
        _componentsWriters.Add(dualSense.Rumble);
        _componentsWriters.Add(dualSense.LightBar);
        _componentsWriters.Add(dualSense.PlayerLeds);
        _componentsWriters.Add(dualSense.Microphone);
        _componentsWriters.Add(dualSense.LeftTrigger);
        _componentsWriters.Add(dualSense.RightTrigger);
    }

    internal byte[] BuildData()
    {
        var report = new byte[47];
        foreach (var writer in _componentsWriters)
        {
            writer.Write(report);
        }
        return report;
    }
}
