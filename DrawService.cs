using Microsoft.Extensions.Logging;

namespace HostedServiceDemo;

public class DrawService
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async Task MainLoop(CancellationToken cancellationToken)
    {
        Console.WriteLine("Start draw service.");
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await CheckPerMinuteAsync();
                await Task.Delay(100 * 1000, cancellationToken);
            }
        }
        catch (Exception)
        {
            Console.WriteLine("DrawService stop");
        }
    }

    private async Task CheckPerMinuteAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            await Task.Run(() => ProcessDraw1());
        }
        catch
        {
            Console.WriteLine("CheckPerMinuteAsync stop");
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private static void ProcessDraw1()
    {
        Console.WriteLine("ProcessDraw1 start");

        Thread.Sleep(1000000);

        Console.WriteLine("ProcessDraw1 end");
    }
}
