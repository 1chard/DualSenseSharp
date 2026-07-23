namespace DualSenseSharp;

using DualSenseSharp.Builder;
using DualSenseSharp.Components;
using DualSenseSharp.Transport;
using DualSenseSharp.Input;
using HidSharp;
using System.Collections.Immutable;

public sealed class DualSense : IDisposable
{
    private readonly ReportBuilder _reportBuilder;
    private readonly DataBuilder _dataBuilder;
    private readonly IInputParser _inputParser;
    private readonly int _inputReportLength;
    private readonly Func<HidDevice, BaseTransport> _transportConstructor;
    private UniqueId? _uniqueId;
    private HidDevice? _hidDevice;
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
    public event EventHandler? Disconnected = null;

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

    public string DevicePath => _hidDevice?.DevicePath ?? throw new ObjectDisposedException(nameof(DualSense));

    public void Open()
    {
        if (Disposed)
            throw new ObjectDisposedException(nameof(DualSense));

        _transport = _transportConstructor(_hidDevice!);
        _transport.Closed += (sender, args) => Disconnected?.Invoke(this, EventArgs.Empty);
    }

    public void Disconnect()
    {
        if (Disposed)
            throw new ObjectDisposedException(nameof(DualSense));

        _transport?.Dispose();
        _transport = null;
    }

    public void Dispose()
    {
        if (!Disposed)
        {
            Disposed = true;
            _transport?.Dispose();
            _transport = null;
            _hidDevice = null;
        }
    }

    public async ValueTask<bool> UpdateInputAsync()
    {
        if (Disposed)
            throw new ObjectDisposedException(nameof(DualSense));

        if (_transport == null)
            throw new InvalidOperationException("Transport is not available. Call Open() first.");

        var buffer = new byte[_inputReportLength];
        var result = await _transport.ReadAsync(buffer);
        if (!result)
            return false;
        Input.UpdateState(_inputParser.GetData(buffer));
        return true;
    }

    public async ValueTask<bool> UpdateOutputAsync()
    {
        if (Disposed)
            throw new ObjectDisposedException(nameof(DualSense));

        if (_transport == null)
            throw new InvalidOperationException("Transport is not available. Call Open() first.");

        var buffer = _reportBuilder.Build();

        return await _transport.WriteAsync(buffer);
    }

    public async ValueTask<UniqueId?> GetUniqueId()
    {
        if (_uniqueId != null)
            return (UniqueId)_uniqueId;

        if (_transport == null)
        {
            if(Disposed)
                throw new InvalidOperationException("Transport is not available. Disposed Object.");
            else
                throw new InvalidOperationException("Transport is not available. Call Open() first.");
        }
            

        var buffer = new byte[20];
        buffer[0] = 0x09;

        var success = await _transport.GetFeature(buffer);
        if (!success)
            return null;
        _uniqueId = new UniqueId(buffer[1..7]);
        return (UniqueId)_uniqueId;
    }

    private static bool IsSupported(HidDevice device)
    {
        return device.ProductID is
            0x0CE6 or // DualSense
            0x0DF2;   // DualSense Egde
    }

    public static List<DualSense> CreateDevices()
    {
        var devices = DeviceList.Local.GetHidDevices(0x054C).Where(IsSupported);

        return devices.Select(device => new DualSense(device)).ToList();
    }

    public static class Manager
    {
        public static event EventHandler<DualSenseEventArgs>? ControllerConnected;
        public static event EventHandler<DualSenseEventArgs>? ControllerDisconnected;
        public static ImmutableList<DualSense> Controllers => _controllers.Values.ToImmutableList();
        private static readonly Dictionary<string, DualSense> _controllers = new();

        private static CancellationTokenSource? _cts;

        public static void Start()
        {
            _cts = new();
            Scan();
            _ = PollLoop(_cts.Token);
        }

        public static void Stop()
        {
            _cts?.Cancel();
            _cts = null;
            _controllers.Clear();
        }

        private static async Task PollLoop(CancellationToken ct)
        {
            while (true)
            {
                await Task.Delay(1000, ct);

                if (ct.IsCancellationRequested)
                    break;

                Scan();
            }
        }

        private static void Scan()
        {
            var devices = DeviceList.Local.GetHidDevices(0x054C).Where(IsSupported);

            var current = devices.ToDictionary(d => d.DevicePath);

            foreach (var pair in current)
            {
                if (_controllers.ContainsKey(pair.Key)) continue;

                var controller = new DualSense(pair.Value);

                _controllers.Add(pair.Key, controller);

                ControllerConnected?.Invoke(
                    null,
                    new DualSenseEventArgs(controller));
            }

            foreach (var path in _controllers.Keys.Except(current.Keys).ToList())
            {
                var controller = _controllers[path];

                _controllers.Remove(path);

                ControllerDisconnected?.Invoke(
                    null,
                    new DualSenseEventArgs(controller));
            }
        }
    }

    public class DualSenseEventArgs : EventArgs
    {
        public readonly DualSense DualSense;
        public DualSenseEventArgs(DualSense sense)
        {
            DualSense = sense;
        }
    }
}