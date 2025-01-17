using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HostedServiceDemo;

public class HostedService : IHostedService
{
    private readonly DrawService _drawService;
    private readonly CancellationTokenSource _stoppingCts = new();

    private Task _drawTask = default!;

    public HostedService(DrawService drawService)
    {
        _drawService = drawService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("HostedService start");

        _drawTask = _drawService.MainLoop(_stoppingCts.Token);

        Console.WriteLine("HostedService start finish");

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("HostedService stop");

        try
        {
            _stoppingCts.Cancel();

            if (_drawTask != null)
            {
                Console.WriteLine("Wait draw service");
                await _drawTask;
                Console.WriteLine("draw service complete");
            }
        }
        catch
        {
            Console.WriteLine("Error occurred while stopping services");
            throw;
        }
        finally
        {
            _stoppingCts.Dispose();
        }

        Console.WriteLine("HostedService stop finish");
    }
}

