using Consul;

namespace LoginServer.Middleware.Consul
{
    public  class ConsulCenter(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;
        private static ConsulClient? _client;


        public async Task ServiceRegistry()
        {
            _client ??= new ConsulClient();

            var ip = _configuration["http:IP"];
            var port = _configuration["http:Port"];
            var registration = new AgentServiceRegistration()
            {
                ID = "LoginServer_01",
                Name = "LoginServer",
                Address = ip,
                Port = Convert.ToInt32(port),
                Tags = new[] { "登录服务", "LoginServer" },
                Check = new AgentServiceCheck
                {
                    HTTP = $"http://{ip}:{port}/Check",
                    Interval = TimeSpan.FromSeconds(5),
                    Timeout = TimeSpan.FromSeconds(15),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(60)
                }
            };
            await _client.Agent.ServiceRegister(registration);
        }
    }
}
