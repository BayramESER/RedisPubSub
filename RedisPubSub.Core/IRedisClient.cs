using StackExchange.Redis;

namespace RedisPubSub.Core
{
    public interface IRedisClient
    {
        bool IsConnected();
        void SendMessage(string sender, string message);
        ISubscriber GetSubscriber();
        string Channel { get; }
    }
}
