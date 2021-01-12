using Microsoft.Extensions.Logging;
using PhotoSi.Interfaces;
using System;
using System.Threading.Tasks;

namespace FakeSendMail
{
    public class SendMail : ISendMail
    {
        private readonly ILogger<SendMail> _logger;

        public SendMail(ILogger<SendMail> logger)
        {
            _logger = logger;
        }

        public async Task SendMailAsync(int orderId)
        {
            _logger.LogInformation($"Send mail order {orderId} for fake");
            await Task.Delay(5000);
            _logger.LogInformation($"Sent mail order {orderId} for fake");
            return;
        }
    }
}
