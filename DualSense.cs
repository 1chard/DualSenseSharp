namespace DualSenseSharp;

using DualSenseSharp.Builder;
using DualSenseSharp.Components;
using DualSenseSharp.Transport;
using DualSenseSharp.Input;
using HidSharp;

public sealed class DualSense : IDisposable
{
    private readonly HidDevice _hidDevice;
    private readonly ReportBuilder _reportBuilder;
    private readonly DataBuilder _dataBuilder;
    private readonly IInputParser _inputParser;
    private readonly int _inputReportLength;
    private readonly Func<HidDevice, BaseTransport> _transportConstructor;
    private UniqueId? _uniqueId;
    private BaseTransport? _transport;

    public bool Disposed { get; private set; }
    public AdaptiveTrigger LeftTrigger { get; }
    public AdaptiveTrigger RightTrigger { get; }
    public LightBar LightBar { get; }
    public PlayerLeds PlayerLeds { get; }
    public Rumble Rumble { get; }
    public Microphone Microphone { get; }
    public InputReader Input { get; }
    public bool IsConnected => _transport?.IsConnected ?? false;
    public bool IsBluetooth { get; init; }
    internal EventHandler? Closed { get; set; } = null;

    private DualSense(HidDevice device)
    {
        LeftTrigger = new AdaptiveTrigger(AdaptiveTrigger.Side.Left);
        RightTrigger = new AdaptiveTrigger(AdaptiveTrigger.Side.Right);

        LightBar = new LightBar();
        PlayerLeds = new PlayerLeds();
        Rumble = new Rumble();
        Microphone = new Microphone();

        Input = new InputReader();

        _dataBuilder = new DataBuilder(this);
        if (device.GetMaxInputReportLength() == 78)
        {
            _reportBuilder = new BluetoothReportBuilder(_dataBuilder);
            _transportConstructor = hidDevice => new BluetoothTransport(hidDevice);
            _inputParser = new BluetoothInputParser();
            IsBluetooth = true;
        }
        else if (device.GetMaxInputReportLength() == 64)
        {
            _reportBuilder = new UsbReportBuilder(_dataBuilder);
            _transportConstructor = hidDevice => new UsbTransport(hidDevice);
            _inputParser = new UsbInputParser();
            IsBluetooth = false;
        }
        else
        {
            throw new UnknownDualsenseException("failed to identify the controller report length");
        }
        _inputReportLength = device.GetMaxInputReportLength();
        _hidDevice = device;
    }

    public void Open()
    {
        if (Disposed)
            throw new ObjectDisposedException(nameof(DualSense));

        _transport = _transportConstructor(_hidDevice);
        _transport.Closed += (sender, args) => Closed?.Invoke(this, EventArgs.Empty);
    }

    public async ValueTask<bool> UpdateInputAsync()
    {
        if (_transport == null)
            throw new InvalidOperationException("Transport is not initialized. Call Open() first.");

        var buffer = new byte[_inputReportLength];
        var result = await _transport.ReadAsync(buffer);
        if (!result)
            return false;
        Input.UpdateState(_inputParser.GetData(buffer));
        return true;
    }

    public async ValueTask<bool> UpdateOutputAsync()
    {
        if (_transport == null)
            throw new InvalidOperationException("Transport is not initialized. Call Open() first.");

        var buffer = _reportBuilder.Build();

        return await _transport.WriteAsync(buffer);
    }

    public async ValueTask<UniqueId> GetUniqueId()
    {
        if (_uniqueId != null)
            return (UniqueId)_uniqueId;

        if (_transport == null)
            throw new InvalidOperationException("Transport is not initialized. Call Open() first.");

        var buffer = new byte[20];
        buffer[0] = 0x09;

        await _transport.GetFeature(buffer);
        _uniqueId = new UniqueId(buffer[1..7]);
        return (UniqueId)_uniqueId;
    }

    private static bool IsSupported(HidDevice device)
    {
        return device.ProductID is
            0x0CE6 or // DualSense
            0x0DF2;   // DualSense Egde
    }

    public void Dispose()
    {
        if (!Disposed)
        {
            _transport?.Dispose();
            Disposed = true;
        }
    }

    public static List<DualSense> ListDevices()
    {
        var devices = DeviceList.Local.GetHidDevices(0x054C).Where(IsSupported);

        return devices.Select(device => new DualSense(device)).ToList();
    }
}