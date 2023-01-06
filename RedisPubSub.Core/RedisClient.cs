using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace RedisPubSub.Core
{
    public class RedisClient : IDisposable, IRedisClient
    {
        protected ConnectionMultiplexer Connection;
        protected string Channel = "channel_one";

        public RedisClient(IConfiguration configuration)
        {
            var options = ConfigurationOptions.Parse(configuration["Redis:RedisHostUrl"] ?? throw new InvalidOperationException());
            options.Password = configuration["Redis:Password"] ?? throw new InvalidOperationException();
            Connection = ConnectionMultiplexer.Connect(options);
        }

        public void Dispose()
        {
            Connection.Dispose();
            GC.SuppressFinalize(this);
        }

        private ISubscriber GetSubscriber()
            => Connection.GetSubscriber();

        private async void SendMessage(string sender, string message)
            => await GetSubscriber().PublishAsync(Channel, $"{sender}: {message}", CommandFlags.FireAndForget);

        #region Service Mappings
        string IRedisClient.Channel => Channel;

        public bool IsConnected()
            => GetSubscriber().IsConnected();

        void IRedisClient.SendMessage(string sender, string message)
            => SendMessage(sender, message);

        ISubscriber IRedisClient.GetSubscriber()
            => GetSubscriber();

        #endregion
    }
}
