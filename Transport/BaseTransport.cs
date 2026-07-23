using HidSharp;

namespace DualSenseSharp.Transport;

internal abstract class BaseTransport : IDisposable
{
    protected readonly HidStream _stream;
    public bool Disposed { get; private set; }

    internal BaseTransport(HidDevice device)
    {
        _stream = device.Open();
        IsConnected = true;
        _stream.Closed += (sender, e) => IsConnected = false;
    }

    public abstract bool IsBluetooth { get; }

    public bool IsConnected { get; protected set; }

    public async ValueTask<bool> WriteAsync(byte[] report, CancellationToken cancellationToken = default)
    {
        try
        {
            await _stream.WriteAsync(report, cancellationToken);
            return true;
        }
        catch (IOException)
        {
            Dispose();
            return false;
        }
        catch (TimeoutException)
        {
            Dispose();
            return false;
        }
    }

    public async ValueTask<bool> ReadAsync(byte[] report, CancellationToken cancellationToken = default)
    {
        try
        {
            await _stream.ReadAsync(report);
            return true;
        }
        catch (IOException)
        {
            Dispose();
            return false;
        }
        catch (TimeoutException)
        {
            Dispose();
            return false;
        }
    }

    public async ValueTask<bool> GetFeature(byte[] report)
    {
        return await Task.Run(() =>
        {
            try
            {
                _stream.GetFeature(report);
                return true;
            }
            catch (IOException)
            {
                Dispose();
                return false;
            }
            catch (TimeoutException)
            {
                Dispose();
                return false;
            }
        });
    }

    public async ValueTask<bool> SetFeature(byte[] report)
    {
        return await Task.Run(() =>
        {
            try
            {
                _stream.SetFeature(report);
                return true;
            }
            catch (IOException)
            {
                Dispose();
                return false;
            }
            catch (TimeoutException)
            {
                Dispose();
                return false;
            }
        });
    }

    public void Dispose()
    {
        if (!Disposed)
        {
            _stream.Dispose();
            Closed?.Invoke(this, EventArgs.Empty);
            Disposed = true;
        }

    }

    internal EventHandler? Closed { get; set; } = null;
}