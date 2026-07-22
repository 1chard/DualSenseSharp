using DualSenseSharp.Utils;
using static DualSenseSharp.Components.AdaptiveTrigger;

namespace DualSenseSharp.Components.Triggers.DSX;

public class CustomTriggerValue : TriggerMode
{
    public override byte Mode => (byte)((byte)CustomTriggerMode switch
    {
        0 => DSXTriggerMode.Off,
        1 => DSXTriggerMode.Rigid,
        2 => DSXTriggerMode.Rigid_A,
        3 => DSXTriggerMode.Rigid_B,
        4 => DSXTriggerMode.Rigid_AB,
        5 => DSXTriggerMode.Pulse,
        6 => DSXTriggerMode.Pulse_A,
        7 => DSXTriggerMode.Pulse_B,
        8 => DSXTriggerMode.Pulse_AB,
        9 => DSXTriggerMode.Pulse_B,
        10 => DSXTriggerMode.Pulse_B2,
        11 => DSXTriggerMode.Pulse_B,
        12 => DSXTriggerMode.Pulse_B2,
        13 => DSXTriggerMode.Pulse_AB,
        14 => DSXTriggerMode.Pulse_AB,
        15 => DSXTriggerMode.Pulse_AB,
        16 => DSXTriggerMode.Pulse_AB,
        _ => DSXTriggerMode.Off,
    });
    public readonly CustomTriggerValueMode CustomTriggerMode;
    public readonly byte[] Data;

    public CustomTriggerValue(CustomTriggerValueMode customTriggerValueMode, byte[] data)
    {
        CustomTriggerMode = customTriggerValueMode;
        Data = data;
    }

    public CustomTriggerValue(CustomTriggerValueMode customTriggerValueMode, byte data1 = 0, byte data2 = 0, byte data3 = 0, 
        byte data4 = 0, byte data5 = 0, byte data6 = 0, byte data7 = 0, byte data8 = 0, byte data9 = 0, byte data10 = 0)
    {
        CustomTriggerMode = customTriggerValueMode;
        Data = new byte[] { data1, data2, data3, data4, data5, data6, data7, data8, data9, data10 };
    }

    internal override byte[] CreateData()
    {
        var data = new byte[10];
        ByteArrayUtil.SetBytes(data, 0, Data);
        return data;
    }

    internal enum DSXTriggerMode : byte
    {
        Off = 0x0,
        Rigid = 0x1,
        Pulse = 0x2,
        Rigid_A = 0x1 | 0x20,
        Rigid_B = 0x1 | 0x04,
        Rigid_AB = 0x1 | 0x20 | 0x04,
        Pulse_A = 0x2 | 0x20,
        Pulse_A2 = 35,
        Pulse_B = 0x2 | 0x04,
        Pulse_B2 = 38,
        Pulse_AB = 39,
        Calibration = 0xFC,
        Feedback = 0x21,
        Weapon = 0x25,
        Vibration = 0x26
    };

    public enum CustomTriggerValueMode : byte
    {
        OFF = 0,
        Rigid = 1,
        RigidA = 2,
        RigidB = 3,
        RigidAB = 4,
        Pulse = 5,
        PulseA = 6,
        PulseB = 7,
        PulseAB = 8,
        VibrateResistance = 9,
        VibrateResistanceA = 10,
        VibrateResistanceB = 11,
        VibrateResistanceAB = 12,
        VibratePulse = 13,
        VibratePulseA = 14,
        VibratePulsB = 15,
        VibratePulseAB = 16
    }
}
