using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedisPubSub.Core;

namespace RedisSubscriber
{
    internal class Program
    {
        static IHostBuilder CreateDefaultBuilder()
        => Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(app =>
                {
                    app.AddJsonFile("appsettings.json");
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IRedisClient, RedisClient>();
                });

        static async Task Main(string[] args)
        {
            var host = CreateDefaultBuilder().Build();
            var redisClient = host.Services.CreateScope().ServiceProvider.GetRequiredService<IRedisClient>();

            await redisClient.GetSubscriber().SubscribeAsync(redisClient.Channel, (channel, message) =>
            {
                Console.WriteLine(Environment.NewLine + message);
            });

            Console.ReadLine();
        }
    }
}