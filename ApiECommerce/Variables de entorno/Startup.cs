using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiECommerce
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var kafkaServers = _configuration["Kafka:BootstrapServers"];
            var resendApiKey = _configuration["Resend:ApiKey"];
            
            // Usarlos normalmente
        }
    }

}