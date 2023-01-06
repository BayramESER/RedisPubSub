using Microsoft.AspNetCore.Mvc;
using RedisPublisher.Model.Request;
using RedisPubSub.Core;

namespace RedisPublisher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedisController : ControllerBase
    {
        private readonly ILogger<RedisController> _logger;
        private readonly IRedisClient redisClient;
        public RedisController(ILogger<RedisController> logger, IRedisClient redisClient)
        {
            _logger = logger;
            this.redisClient = redisClient;
        }

        [HttpGet(Name = "CheckConnection")]
        public bool CheckConnection()
            => redisClient.IsConnected();

        [HttpPost(Name = "SendMessage")]
        public void SendMessage([FromBody] SendMessageRequest request)
            => redisClient.SendMessage(request.Sender, request.Message);


    }
}
