using HostedServiceDemo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // 注册 DrawService 和 HostedService
                services.AddSingleton<DrawService>(); // 注册 DrawService 为单例
                services.AddHostedService<HostedService>(); // 注册 IHostedService
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            })
            .Build();

        // 创建一个 CancellationTokenSource, 2 秒后取消
        var cts = new CancellationTokenSource();
        Task.Delay(10000).ContinueWith(_ => cts.Cancel());

        await host.RunAsync(cts.Token);
    }
}
